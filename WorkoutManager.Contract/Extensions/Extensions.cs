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
            var stringValue = value.ToString();
            var enumMember = value.GetType().GetMember(stringValue).FirstOrDefault();

            var descriptionAttribute =
                enumMember?.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

            return descriptionAttribute?.Description ?? stringValue;
        }
    }
}