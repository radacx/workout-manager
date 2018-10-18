using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Repository.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(DatabaseConfiguration configuration) : base(configuration) {}

        private static IEntity CreateItem(Type itemType, int id)
        {
            if (itemType == typeof(Exercise))
            {
                return new Exercise
                {
                    Id = id
                };
            }

            if (itemType == typeof(Muscle))
            {
                return new Muscle
                {
                    Id = id
                };
            }

            throw new ArgumentException("Unknown type");
        }
        
        public static void Register(BsonMapper mapper)
        {
            mapper.RegisterType(
                category => new Dictionary<string, BsonValue>
                {
                    { "_id", category.Id },
                    { "name", category.Name },
                    { "itemType", category.ItemType.AssemblyQualifiedName },
                    { "items", new BsonArray(category.Items.Select(item => new BsonValue(item.Id))) }
                },
                bsonValue =>
                {
                    var doc = bsonValue.AsDocument;

                    var type = Type.GetType(doc["itemType"]);
                    var items = doc["items"].AsArray.Select(bsonId => CreateItem(type, bsonId.AsInt32)).ToArray();

                    return new Category
                    {
                        Id = doc["_id"],
                        Name = doc["name"],
                        ItemType = type,
                        Items = items
                    };
                }
            );
        }

        public override IEnumerable<Category> GetAll()
            => Execute(collection => collection.Include(x => x.Items).FindAll().ToList());
    }
}