using System;
using System.Collections;
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
        public string[] ExcludeProps { get; set; }

        public ClaimExcInjection()
        {
        }

        public ClaimExcInjection(string[] excludeProps)
        {
            this.ExcludeProps = excludeProps;
        }

        protected bool IsNotExclude(string property)
        {
            return ExcludeProps == null || !ExcludeProps.Contains(property, StringComparer.OrdinalIgnoreCase);
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
            if (ExcludeProps == null || !ExcludeProps.Contains(sp.Name, StringComparer.OrdinalIgnoreCase))
            {
                // Trace.WriteLine(sp.Name + "-" + sp.PropertyType.Name);

                // For some reason, String is determine as IEnumerable
                if (typeof(IEnumerable).IsAssignableFrom(sp.PropertyType)
                    && sp.PropertyType.Name != "String") return;

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
