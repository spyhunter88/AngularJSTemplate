using System;
using System.Collections.Generic;
using System.Linq;
using AngularJS.Entities.Models;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Services
{
    /// <summary>
    /// This interface include function to retrieve all actions and configs of an object
    /// objectID must not be input, we use inNonPublic instead for an easy code.
    /// (This is not use for take available options when Search or Create).
    /// </summary>
    public interface IObjectConfigService
    {
        /// <summary>
        /// Get all actions available with an object.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="objectStatus">Compact properties of an object</param>
        /// <param name="incNonPublic">If object is Create by an User, this value is true</param>
        /// <returns></returns>
        List<ObjectAction> GetObjectAction(int userID, string objectName, string objectStatus, bool incNonPublic);

        List<ObjectConfig> GetObjectConfig(int userID, string objectName, string objectStatus, bool incNonPublic);
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
        public List<ObjectAction> GetObjectAction(int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            // 1. Get list of Role
            var roles = _unitOfWorkAsync.Repository<User>().Find(new object[] { userID }).Roles.Select(x => x.Id);

            // 2. Query list
            var list = _unitOfWorkAsync.Repository<ObjectAction>().Query(
                x => x.Object == objectName && x.Status == objectStatus &&
                (x.UserID == userID || roles.Contains(x.GroupID ?? -1)) &&
                (x.PublicEnabled == 1 || incNonPublic)
                ).Select().ToList();

            return list;
        }

        /// <summary>
        /// Load Object properties' attributes
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="objectStatus"></param>
        /// <param name="incNonPublic">Use when object is created by userId</param>
        /// <returns></returns>
        public List<ObjectConfig> GetObjectConfig(int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            // Get roles by user
            var roles = _unitOfWorkAsync.Repository<User>().Find(new object[] { userID }).Roles.Select(x => x.Id);

            // 2. Query list
            var list = _unitOfWorkAsync.Repository<ObjectConfig>().Query(
                x => x.Object == objectName && x.Status == objectStatus &&
                (x.UserID == userID || roles.Contains(x.GroupID ?? -1)) &&
                (x.PublicEnabled == 1 || incNonPublic)
                ).Select().ToList();

            return list;
        }
    }
}
