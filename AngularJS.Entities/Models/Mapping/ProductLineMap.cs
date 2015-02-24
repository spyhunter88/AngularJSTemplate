using System.Data.Entity.ModelConfiguration;

namespace AngularJS.Entities.Models.Mapping
{
    public class ProductLineMap : EntityTypeConfiguration<ProductLine>
    {
        public ProductLineMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductLineID);

            this.Property(t => t.ProductLineName)
                .HasMaxLength(255);

            // Map
            this.ToTable("ProductLine");
            this.Property(t => t.ProductLineID).HasColumnName("ProductLineID");
            this.Property(t => t.ProductLineName).HasColumnName("ProductLineName");
        }
    }
}
