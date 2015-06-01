using System.Collections.Generic;
using System.Linq;
using AngularJS.Entities.Models;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Services.Utility
{
    public class PolicyUtil
    {
        public static List<ObjectConfig> GetObjectConfig(IUnitOfWorkAsync unitOfWorkAsync, int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            // Get roles by user
            var roles = unitOfWorkAsync.Repository<User>().Find(new object[] { userID }).Roles.Select(x => x.Id);

            // 2. Query list
            var list = unitOfWorkAsync.Repository<ObjectConfig>().Query(
                x => x.Object == objectName && 
                (x.Status == objectStatus || x.Status == "") &&
                (x.UserID == userID || roles.Contains(x.GroupID ?? -1)) &&
                (x.PublicEnabled == 1 || incNonPublic)
                ).Select().ToList();

            return list;
        }
    }
}
