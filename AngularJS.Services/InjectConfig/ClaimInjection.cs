﻿using System.Linq;
using System.Reflection;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace AngularJS.Services.InjectConfig
{
    /// <summary>
    /// Only property in includeProps is allowed to inject
    /// </summary>
    public class ClaimInjection : PropertyInjection
    {
        protected string[] includeProps;

        public ClaimInjection()
        {
        }

        public ClaimInjection(string[] includeProps)
        {
            this.includeProps = includeProps;
        }

        protected bool IsNotInclude(string property)
        {
            return includeProps == null || !includeProps.Contains(property);
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
            if (includeProps != null && includeProps.Contains(sp.Name))
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
}
