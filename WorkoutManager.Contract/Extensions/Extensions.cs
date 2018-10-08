using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace WorkoutManager.Contract.Extensions
{
    public static class Extensions
    {
        public static string GetDescription(this Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();

            var descriptionAttribute =
                enumMember?.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

            return descriptionAttribute?.Description;
        }
    }
}