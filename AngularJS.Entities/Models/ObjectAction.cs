using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class ObjectAction : Entity
    {

        public int ID { get; set; }
        public string Object { get; set; }
        public string Status { get; set; }
        public Int32? UserID { get; set; }
        public Int32? GroupID { get; set; }
        public string Action { get; set; }
        public int PublicEnabled { get; set; }

    }
}
