using System.Collections.Generic;
using Omu.ValueInjecter;

namespace AngularJS.Services.InjectConfig
{
    public class ClaimUpdate : ConventionInjection
    {
        public static List<string> merges;

        protected override bool Match(ConventionInfo c)
        {
            if (c.SourceProp.Type != c.TargetProp.Type || c.SourceProp.Name != c.TargetProp.Name) return false;
            //if (merges != null && merges.Contains(c.SourceProp.Name))
            //    return true;
            //else
            //    return false;
            if (c.SourceProp.Name == "PaymentMethod")
                return true;
            return false;
        }
    }
}
