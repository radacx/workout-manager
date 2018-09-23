using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.Repository
{
    public class Repository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly string _dbFileName;

        public Repository(string dbFileName)
        {
            _dbFileName = dbFileName;
        }

        protected void Execute(Action<LiteCollection<TEntity>> action)
        {
            using (var db = new LiteDatabase(_dbFileName))
            {
                var collection = db.GetCollection<TEntity>();
                action(collection);
            }    
        }
        
        protected TResult Execute<TResult>(Func<LiteCollection<TEntity>, TResult> action)
        {
            using (var db = new LiteDatabase(_dbFileName))
            {
                var collection = db.GetCollection<TEntity>();
                return action(collection);
            }    
        }

        public virtual IEnumerable<TEntity> GetAll() => Execute(collection => collection.FindAll().ToList());

        public void Create(TEntity item) => Execute(collection => collection.Insert(item));
        
        public void CreateRange(IEnumerable<TEntity> items) => Execute(collection => collection.Insert(items));

        public void Update(TEntity item) => Execute(collection => collection.Update(item));

        public void Delete(TEntity item) => Delete(item.Id);
        
        public void Delete(int itemId) => Execute(collection => collection.Delete(itemId));
    }
}