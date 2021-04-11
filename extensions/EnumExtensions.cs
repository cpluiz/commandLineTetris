using System;
using System.Linq;

namespace Extensions{
    public static class EnumExtensions
    {
        public static T GetRandomEnumValue<T>() where T : Enum
        {
            return (T)Enum.GetValues(typeof(T))          // get values from Type provided
                .OfType<Enum>()               // casts to Enum
                .OrderBy(e => Guid.NewGuid()) // mess with order of results
                .FirstOrDefault();            // take first item in result
        }
    }
}