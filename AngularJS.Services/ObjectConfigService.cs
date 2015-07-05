using System;
using System.Collections.Generic;
using System.Linq;
using AngularJS.Entities.Models;
using Repository.Pattern.Infrastructure;
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
        /// Get all actions available with specific object (include from user's roles)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="objectStatus">Compact properties of specific object</param>
        /// <param name="incNonPublic">If specific object is Create by an User, this value is true</param>
        /// <returns></returns>
        List<ObjectAction> GetObjectAction(int userID, string objectName, string objectStatus, bool incNonPublic);

        /// <summary>
        /// Get all configs available with specific object (include from user's roles)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="objectStatus">Compact properties of specific object</param>
        /// <param name="incNonPublic">If specific object is Create by an User, this value is true</param>
        /// <returns></returns>
        List<ObjectConfig> GetObjectConfig(int userID, string objectName, string objectStatus, bool incNonPublic);

        /// <summary>
        /// Get All ObjectAction from only UserID (prefer) or RoleID
        /// </summary>
        List<ObjectAction> GetObjectAction(int userID, int roleID);

        /// <summary>
        /// Get All ObjectConfig from only UserID (prefer) or RoleID
        /// </summary>
        List<ObjectConfig> GetObjectConfig(int userID, int roleID);

        void SaveObjectAction(List<ObjectAction> objectActions, int userID, int roleID);

        void SaveObjectConfig(List<ObjectConfig> objectConfigs, int userID, int roleID);
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

        public List<ObjectAction> GetObjectAction(int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            // 1. Get list of Role
            var roles = _unitOfWorkAsync.Repository<User>().Find(new object[] { userID }).Roles.Select(x => x.Id);

            // 2. Query list
            var list = _unitOfWorkAsync.Repository<ObjectAction>().Query(
                x => x.Object == objectName && x.Status == objectStatus &&
                (x.UserID == userID || roles.Contains(x.RoleID ?? -1)) &&
                (x.PublicEnabled == 1 || incNonPublic)
                ).Select().ToList();

            return list;
        }

        public List<ObjectConfig> GetObjectConfig(int userID, string objectName, string objectStatus, bool incNonPublic)
        {
            // Get roles by user
            var roles = _unitOfWorkAsync.Repository<User>().Find(new object[] { userID }).Roles.Select(x => x.Id);

            // 2. Query list
            var list = _unitOfWorkAsync.Repository<ObjectConfig>().Query(
                x => x.Object == objectName && x.Status == objectStatus &&
                (x.UserID == userID || roles.Contains(x.RoleID ?? -1)) &&
                (x.PublicEnabled == 1 || incNonPublic)
                ).Select().ToList();

            return list;
        }

        public List<ObjectAction> GetObjectAction(int userID, int roleID)
        {
            var list = _unitOfWorkAsync.Repository<ObjectAction>()
                        .Query(x => (x.UserID == userID && userID != 0) || (x.RoleID == roleID && userID == 0))
                        .Select().ToList();

            return list;
        }

        public List<ObjectConfig> GetObjectConfig(int userID, int roleID)
        {
            var list = _unitOfWorkAsync.Repository<ObjectConfig>()
                        .Query(x => (x.UserID == userID && userID != 0) || (x.RoleID == roleID && userID == 0))
                        .Select().ToList();

            return list;
        }

        public void SaveObjectAction(List<ObjectAction> objectActions, int userID, int roleID)
        {
            // Remove current settings
            var _cur = _unitOfWorkAsync.Repository<ObjectAction>()
                        .Query(x => (x.UserID == userID && userID != 0) || (x.RoleID == roleID && userID == 0))
                        .Select().ToList();

            // Remove old
            foreach (ObjectAction _oa in _cur)
            {
                _oa.ObjectState = ObjectState.Deleted;
            }

            // Add new
            foreach (ObjectAction _oa in objectActions)
            {
                _oa.ID = 0;
                _oa.ObjectState = ObjectState.Added;
            }
            _unitOfWorkAsync.Repository<ObjectAction>().InsertRange(objectActions);

            _unitOfWorkAsync.SaveChanges();
        }

        public void SaveObjectConfig(List<ObjectConfig> objectConfigs, int userID, int roleID)
        {
            // Remove current settings
            var _cur = _unitOfWorkAsync.Repository<ObjectConfig>()
                        .Query(x => (x.UserID == userID && userID != 0) || (x.RoleID == roleID && userID == 0))
                        .Select().ToList();

            // Remove old
            foreach (ObjectConfig _oa in _cur)
            {
                _oa.ObjectState = ObjectState.Deleted;
            }

            // Add new
            foreach (ObjectConfig _oa in objectConfigs)
            {
                _oa.ID = 0;
                _oa.ObjectState = ObjectState.Added;
            }
            _unitOfWorkAsync.Repository<ObjectConfig>().InsertRange(objectConfigs);

            _unitOfWorkAsync.SaveChanges();
        }
    }
}
