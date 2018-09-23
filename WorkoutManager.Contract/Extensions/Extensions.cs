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
        
        public static T Clone<T>(this T source)
        {
            var ofT = typeof(T);
            var result = (T) ofT.GetConstructor(Type.EmptyTypes)?.Invoke(null);

            var objectFields = ofT.GetFields(
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.FlattenHierarchy
            );

            foreach (var fi in objectFields) {
                if (fi.FieldType == typeof(string)) {
                    var sourceString = (string) fi.GetValue(source);
                    fi.SetValue(result, new string(sourceString.ToCharArray()));
                }
                else
                {
                    fi.SetValue(result, fi.GetValue(source));
                }
            }

            return result;
        }
    }
}