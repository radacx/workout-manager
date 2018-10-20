using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Misc;
using WorkoutManager.Contract.Models.Progress;

namespace WorkoutManager.Repository.Repositories
{
    public class ProgressFilterRepository : Repository<ProgressFilter>
    {
        public ProgressFilterRepository(DatabaseConfiguration configuration) : base(configuration) { }

        public static void Register(BsonMapper mapper)
        {
            IEntity GetValue(Type type, int id)
            {
                if (type == typeof(Exercise))
                {
                    return new Exercise
                    {
                        Id = id
                    };
                }

                if (type == typeof(Muscle))
                {
                    return new Muscle
                    {
                        Id = id
                    };
                }

                if (type == typeof(Category))
                {
                    return new Category
                    {
                        Id = id
                    };
                }

                throw new ArgumentException("Invalid value type");
            }

            Type GetType(string typeName) => Type.GetType(typeName);
            
            mapper.RegisterType(
                filter =>
                {
                    var doc = new Dictionary<string, BsonValue>
                    {
                        { "_id", filter.Id },
                        { "name", filter.Name },
                    };

                    if (filter.Metric.HasValue)
                    {
                        doc.Add("metric", (int) filter.Metric.Value);
                    }
                    
                    if (filter.GroupBy.HasValue)
                    {
                        doc.Add("groupBy", (int) filter.GroupBy.Value);
                    }
                    
                    if (filter.FilterBy.HasValue)
                    {
                        doc.Add("filterBy", (int) filter.FilterBy.Value);
                        doc.Add("valueType", filter.FilteringValue.GetType().AssemblyQualifiedName);
                        doc.Add("valueId", filter.FilteringValue.Id);
                    }
                    
                    return doc;
                },
                bsonValue =>
                {
                    var doc = bsonValue.AsDocument;

                    var filterBy = doc["filterBy"].RawValue;
                    var groupBy = doc["groupBy"].RawValue;
                    var metric = doc["metric"].RawValue;

                    var filter = new ProgressFilter
                    {
                        Id = doc["_id"],
                        Name = doc["name"],
                    };

                    if (filterBy != null)
                    {
                        filter.FilterBy = (FilterBy) filterBy;
                        filter.FilteringValue = GetValue(GetType(doc["valueType"].AsString), doc["valueId"].AsInt32);
                    }

                    if (groupBy != null)
                    {
                        filter.GroupBy = (GroupBy) groupBy;
                    }

                    if (metric != null)
                    {
                        filter.Metric = (FilterMetric) metric;
                    }
                    
                    return filter;
                }
            );
        }

        public override IEnumerable<ProgressFilter> GetAll() => Execute(
            collection => collection.Include(x => x.FilteringValue).FindAll().ToList()
        );
    }
}