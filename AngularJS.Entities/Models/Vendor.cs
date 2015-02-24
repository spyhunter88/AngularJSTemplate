using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class Vendor : Entity
    {
        public Vendor() { }

        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorShortName { get; set; }
    }
}
