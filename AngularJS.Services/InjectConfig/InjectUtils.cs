using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace AngularJS.Services.InjectConfig
{
    public static class InjectUtils
    {
        // We take it to search for ID and inject one-to-one, the key is string of ID property,
        // we also add new member to the result list 
        public static void InjectFrom<TDest, TSource>(this IEnumerable<TDest> dests, IValueInjection injection, IEnumerable<TSource> sources, string key, bool addRemain = false)
        {
            List<object> ids = new List<object>();
            foreach (TDest dest in dests)
            {
                var idVal = GetPropValue(dest, key);
                var source = sources.Where(x => GetPropValue(x, key) == idVal).Select(x => x).FirstOrDefault();

                // back up for later use
                ids.Add(idVal);
                if (source == null) continue;

                dest.InjectFrom(injection, source);
            }

            // We loop through others and 
            if (addRemain)
            {
                foreach (TSource source in sources.Where(x => ids.Contains(GetPropValue(x, key))).Select(x => x))
                {
                    var newD = Activator.CreateInstance<TDest>();
                    newD.InjectFrom(injection, source);
                    dests = dests.Concat(new[] { newD });
                }
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
