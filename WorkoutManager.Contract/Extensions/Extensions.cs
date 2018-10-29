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

        public static void RemoveByReference<T>(this IList<T> collection, T itemToRemove)
        {
            var index = -1;

            for (var i = 0; i < collection.Count; i++)
            {
                var item = collection.ElementAt(i);

                if (!ReferenceEquals(item, itemToRemove))
                {
                    continue;
                }

                index = i;
                break;
            }
            
            collection.RemoveAt(index);
        }
    }
}