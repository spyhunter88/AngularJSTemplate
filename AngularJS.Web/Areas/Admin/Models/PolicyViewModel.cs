using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularJS.Entities.Models;

namespace AngularJS.Web.Areas.Admin.Models
{
    public class PolicyViewModel
    {
        #region Initialize
        public PolicyViewModel()
        {
            this.ObjectActions = new List<ObjectAction>();
            this.ObjectConfigs = new List<ObjectConfig>();
        }

        public PolicyViewModel(int userId, int roleId)
            : base()
        {
            this.UserID = userId;
            this.RoleID = roleId;
        }
        #endregion

        public int UserID { get; set; }
        public int RoleID { get; set; }
        public List<ObjectConfig> ObjectConfigs { get; set; }
        public List<ObjectAction> ObjectActions { get; set; }
    }
}