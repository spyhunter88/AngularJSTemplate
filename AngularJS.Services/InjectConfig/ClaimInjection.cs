using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace AngularJS.Services.InjectConfig
{
    /// <summary>
    /// Only property in includeProps is allowed to inject
    /// </summary>
    public class ClaimIncInjection : PropertyInjection
    {
        protected string[] includeProps;

        public ClaimIncInjection()
        {
        }

        public ClaimIncInjection(string[] includeProps)
        {
            this.includeProps = includeProps;
        }

        protected bool IsNotInclude(string property)
        {
            return includeProps == null || !includeProps.Contains(property, StringComparer.OrdinalIgnoreCase);
        }

        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProps();
            foreach (var s in sourceProps)
            {
                Execute(s, source, target);
            }
        }

        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (includeProps != null && includeProps.Contains(sp.Name, StringComparer.OrdinalIgnoreCase))
            {
                var targetProp = target.GetType().GetProperty(sp.Name);
                if (targetProp != null && targetProp.PropertyType == sp.PropertyType)
                {
                    var val = sp.GetValue(source);
                    targetProp.SetValue(target, val);
                }
            }
        }
    }

    /// <summary>
    /// All property not in this list will inject
    /// 
    /// </summary>
    public class ClaimExcInjection : PropertyInjection
    {
        protected string[] excludeProps;

        public ClaimExcInjection()
        {
        }

        public ClaimExcInjection(string[] excludeProps)
        {
            this.excludeProps = excludeProps;
        }

        protected bool IsNotExclude(string property)
        {
            return excludeProps == null || !excludeProps.Contains(property, StringComparer.OrdinalIgnoreCase);
        }

        protected override void Inject(object source, object target)
        {
            var sourceProps = source.GetType().GetProps();
            foreach (var s in sourceProps)
            {
                Execute(s, source, target);
            }
        }

        protected override void Execute(PropertyInfo sp, object source, object target)
        {
            if (excludeProps == null || !excludeProps.Contains(sp.Name, StringComparer.OrdinalIgnoreCase))
            {
                // if (sp.PropertyType == typeof(ICollection<>)) return;
                if (sp.PropertyType != typeof(string))
                 if (typeof(IEnumerable).IsAssignableFrom(sp.PropertyType)) return;

                var targetProp = target.GetType().GetProperty(sp.Name);
                if (targetProp != null && targetProp.PropertyType == sp.PropertyType)
                {
                    var val = sp.GetValue(source);
                    targetProp.SetValue(target, val);
                }
            }
        }
    }
}
