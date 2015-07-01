using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class ObjectConfig : Entity
    {
        public int ID { get; set; }
        public string Object { get; set; }
        public string Status { get; set; }
        public Int32? UserID { get; set; }
        public Int32? RoleID { get; set; }
        public string ObjectField { get; set; }
        public string FieldProperty { get; set; }
        public Byte PublicEnabled { get; set; }

    }
}
