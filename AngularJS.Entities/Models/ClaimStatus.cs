using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class ClaimStatus : Entity
    {
        public ClaimStatus() { }

        public short StatusID { get; set; }
        public string StatusName { get; set; }
        public string Phase { get; set; }
        public int Code { get; set; }
    }
}
