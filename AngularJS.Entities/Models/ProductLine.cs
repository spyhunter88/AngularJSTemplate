using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class ProductLine : Entity
    {
        public ProductLine() { }

        public int ProductLineID { get; set; }
        public string ProductLineName { get; set; }
    }
}
