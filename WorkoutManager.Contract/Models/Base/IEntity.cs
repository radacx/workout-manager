using System;

namespace WorkoutManager.Contract.Models.Base
{
    public interface IEntity : IEquatable<IEntity>
    {
        int Id { get; set; }

        IEntity GenericClone();
    }
}