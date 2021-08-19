using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    static public class Helper {
        /// <summary>
        /// Method to define an object using its public properties that
        /// are readable.
        /// </summary>
        /// <param name="obj">The object to define.</param>
        /// <returns>A dictionary containing the public properties of the given object.</returns>
        static public Dictionary<string, object> DefinePublicProps<T>(T obj, WriterStats stats) {

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Dictionary<string, object> def = new();

            if (!(props is null)) {
                foreach (var p in props)
                    if (p.CanRead)
                        def[p.Name] = p.GetValue(obj);
            }

            return def;
        }

    }
}
