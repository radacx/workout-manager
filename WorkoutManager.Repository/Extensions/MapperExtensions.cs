using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;
using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Repository.Extensions
{
    internal static class MapperExtensions
    {
        public static IEntityBuilder<TObject> For<TObject>(this BsonMapper mapper) => new InternalEntityBuilder<TObject>(mapper.Entity<TObject>());

        private class InternalEntityBuilder<TObject> : IEntityBuilder<TObject>
        {
            private EntityBuilder<TObject> _entityBuilder;

            public InternalEntityBuilder(EntityBuilder<TObject> entityBuilder)
            {
                _entityBuilder = entityBuilder;
            }
            
            public IEntityBuilder<TObject> DbRef<TEntity>(Expression<Func<TObject, TEntity>> selector)
                where TEntity : IEntity
            {
                _entityBuilder = _entityBuilder.DbRef(selector);

                return this;
            }
        
            public IEntityBuilder<TObject> DbRefMany<TEntity>(Expression<Func<TObject, IEnumerable<TEntity>>> selector)
                where TEntity : IEntity
            {
                _entityBuilder = _entityBuilder.DbRef(selector);

                return this;
            }
        }
    }
}