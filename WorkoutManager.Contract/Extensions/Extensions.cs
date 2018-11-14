using System;
using System.Collections.Generic;
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

        public static int FindIndexByReference<T>(this ICollection<T> collection, T item)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (ReferenceEquals(collection.ElementAt(i), item))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static void RemoveByReference<T>(this IList<T> collection, T item)
        {
            var index = collection.FindIndexByReference(item);

            if (index == -1)
            {
                return;
            }
            
            collection.RemoveAt(index);
        }
    }
}