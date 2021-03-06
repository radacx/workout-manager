﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Base;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository.Extensions;
using WorkoutManager.Repository.Repositories;

namespace WorkoutManager.Repository
{
    internal class CustomMapper : BsonMapper
    {
        private static Type GetEntityInterface(Type type)
        {
            var interfaces = type.GetInterfaces();
            var entityInterface = interfaces.FirstOrDefault(typ => typ == typeof(IEntity));

            return entityInterface;
        }
        
        protected override IEnumerable<MemberInfo> GetTypeMembers(Type type)
        {
            var memberInfos = base.GetTypeMembers(type).ToList();

            var hasId = memberInfos.OfType<PropertyInfo>()
                .ToList()
                .Exists(
                    property => property.PropertyType == typeof(IEntity).GetProperty(nameof(IEntity.Id))?.PropertyType
                );

            if (hasId)
            {
                return memberInfos;
            }

            var entityInterface = GetEntityInterface(type);

            return entityInterface != null ? memberInfos.Concat(base.GetTypeMembers(entityInterface)) : memberInfos;
        }
    }   
    
    public class Repository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DatabaseConfiguration _configuration;

        static Repository()
        {
            var mapper = new CustomMapper();
            
            mapper.For<ExercisedMuscle>().DbRef(x => x.Muscle);
            mapper.For<SessionExercise>().DbRef(x => x.Exercise);

            mapper.RegisterType(
                category => new Dictionary<string, BsonValue>
                {
                    { "_id", category.Id },
                    { "name", category.Name },
                    { "type", category.ItemType.AssemblyQualifiedName },
                    { "items", new BsonArray(category.Items.Select(x => new BsonValue(x.Id))) },
                },
                bsonVal =>
                {
                    var doc = bsonVal.AsDocument;

                    return new Category
                    {
                        Id = doc["_id"],
                        Name = doc["name"],
                        Items = doc["items"].AsArray.Select(id => new EntityReference(id)).ToArray(),
                        ItemType = Type.GetType(doc["type"]),
                    };
                }
            );
            
            ProgressFilterRepository.Register(mapper);
            
            BsonMapper.Global = mapper;
        }
        
        public Repository(DatabaseConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string FileName => _configuration.FileName;
        
        protected void Execute(Action<LiteCollection<TEntity>> action)
        {
            using (var db = new LiteDatabase(FileName))
            {
                var collection = db.GetCollection<TEntity>();
                action(collection);
            }    
        }
        
        protected TResult Execute<TResult>(Func<LiteCollection<TEntity>, TResult> action)
        {
            using (var db = new LiteDatabase(FileName))
            {
                var collection = db.GetCollection<TEntity>();
                return action(collection);
            }    
        }
        
        public virtual IEnumerable<TEntity> GetAll() => Execute(collection => collection.FindAll().ToList());

        public void Create(TEntity item) => Execute(collection => collection.Insert(item));
        
        public void CreateRange(IEnumerable<TEntity> items) => Execute(collection => collection.Insert(items));

        public void Update(TEntity item) => Execute(collection => collection.Update(item));

        public void UpdateRange(IEnumerable<TEntity> items) => Execute(
            collection =>
            {
                foreach (var item in items)
                {
                    collection.Update(item);
                }
            }
        );
        
        public void Delete(TEntity item) => Execute(collection => collection.Delete(item.Id));
        
        public void DeleteRange(IEnumerable<TEntity> items) => Execute(
            collection =>
            {
                foreach (var item in items)
                {
                    collection.Delete(item.Id);
                }
            });

        public void DeleteAll()
        {
            using (var db = new LiteDatabase(FileName))
            {
                var collection = db.GetCollection<TEntity>();
                db.DropCollection(collection.Name);
            }   
        }
    }
}