using System.Collections.Generic;
using WorkoutManager.Contract.Models.Misc;
using WorkoutManager.Repository;

namespace WorkoutManager.Service
{
    public abstract class Service<TEntity>
        where TEntity : class, IEntity
    {
        protected Service(Repository<TEntity> repository)
        {
            Repository = repository;
        }

        protected readonly Repository<TEntity> Repository;

        public virtual IEnumerable<TEntity> GetAll() => Repository.GetAll();
    }
}