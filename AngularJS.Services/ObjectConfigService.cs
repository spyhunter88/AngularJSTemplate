using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularJS.Entities.Models;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Services
{
    public interface IObjectConfigService
    {
        string GetObjectAction(int userID, string objectName, string objectStatus, bool incNonPublic);

        string GetObjectConfig(int userID, string objectName, string objectStatus, bool incNonPublic);
    }

    public class ObjectConfigService : IObjectConfigService
    {
        #region Inject by Unity
        public ObjectConfigService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        #endregion

        /// <summary>
        /// Load Action to trigger button in a form
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="objectStatus"></param>
        /// <param name="incNonPublic">Use when object is created by userId</param>
        /// <returns></returns>
        public string GetObjectAction(int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            // 1. Get public action by userID with objectName and objectStatus
            // 1.1. Get list of Role
            var roles = _unitOfWorkAsync.Repository<User>().Find(new object[] { userID }).Roles.Select(x => x.Id);

            // 1.2. Query list
            var publicList = _unitOfWorkAsync.Repository<ObjectAction>().Query(
                x => x.Object == objectName && x.Status == objectStatus && 
                (x.UserID == userID || roles.Contains(x.GroupID ?? -1)) &&
                (x.PublicEnabled == 1 || incNonPublic)
                ).Select().ToList();

            // 2. Get private action by userID with objectName and objectStatus if objectID is create by userID
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load Object properties' attributes
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="objectStatus"></param>
        /// <param name="incNonPublic">Use when object is created by userId</param>
        /// <returns></returns>
        public string GetObjectConfig(int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            throw new NotImplementedException();
        }
    }
}
