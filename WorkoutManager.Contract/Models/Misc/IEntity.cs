using System;

namespace WorkoutManager.Contract.Models.Misc
{
    public interface IEntity : IEquatable<IEntity>
    {
        int Id { get; set; }
    }
}