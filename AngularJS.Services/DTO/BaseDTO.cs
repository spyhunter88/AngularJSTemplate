using System;
using System.Collections.Generic;
using System.Linq;
using AngularJS.Entities.Models;
using Newtonsoft.Json;

namespace AngularJS.Services.DTO
{
    public class BaseDTO
    {
        private string _objectAction;
        private string _objectConfig;

        public String ObjectAction { get; private set; }

        // Save the config for Enable/Disable/Visible and Validate
        public String ObjectConfig { get; private set; }

        /// <summary>
        /// Set both actions and configs for one method
        /// </summary>
        /// <param name="objectActions"></param>
        /// <param name="objectConfigs"></param>
        public void SetObjectConfig(List<ObjectAction> objectActions, List<ObjectConfig> objectConfigs)
        {
            var _actions = objectActions.Select(x => x.Action).ToArray();
            var _configs = objectConfigs.Select(x => new { x.ObjectField, x.FieldProperty }).ToArray();

            ObjectAction = JsonConvert.SerializeObject(_actions);
            ObjectConfig = JsonConvert.SerializeObject(_configs);
        }
    }
}
