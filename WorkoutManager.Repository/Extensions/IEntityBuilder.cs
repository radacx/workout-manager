using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WorkoutManager.Contract.Models.Base;

namespace WorkoutManager.Repository.Extensions
{
    internal interface IEntityBuilder<TObject>
    {
        IEntityBuilder<TObject> DbRef<TEntity>(Expression<Func<TObject, TEntity>> selector)
            where TEntity : IEntity;

        IEntityBuilder<TObject> DbRefMany<TEntity>(Expression<Func<TObject, IEnumerable<TEntity>>> selector)
            where TEntity : IEntity;
    }
}