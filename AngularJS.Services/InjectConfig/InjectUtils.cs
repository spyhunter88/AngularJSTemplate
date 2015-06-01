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
            List<TSource> srcs = sources.ToList();
            foreach (TDest dest in dests)
            {
                object idVal = GetPropValue(dest, key);
                var source = srcs.Where(x => idVal.Equals(GetPropValue(x, key))).Select(x => x).FirstOrDefault();
                srcs.Remove(source);

                // back up for later use
                ids.Add(idVal);
                if (source == null) continue;

                dest.InjectFrom(injection, source);
            }

            // We loop through others
            if (addRemain)
            {
                // List<TDest> add = dests.ToList();
                // foreach (TSource source in srcs)
                // {
                //    var newD = Activator.CreateInstance<TDest>();
                //    newD.InjectFrom(injection, source);
                    // dests.Add(newD);
                // }
                // dests = add;
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
