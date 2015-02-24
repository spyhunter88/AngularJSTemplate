using System.Data.Entity.ModelConfiguration;

namespace AngularJS.Entities.Models.Mapping
{
    public class VendorMap : EntityTypeConfiguration<Vendor>
    {
        public VendorMap()
        {
            // Primary Key
            this.HasKey(t => t.VendorID);

            // Properties
            this.Property(t => t.VendorName)
                .HasMaxLength(255);

            this.Property(t => t.VendorShortName)
                .HasMaxLength(255);

            // Map
            this.ToTable("Vendor");
            this.Property(t => t.VendorID).HasColumnName("VendorID");
            this.Property(t => t.VendorName).HasColumnName("VendorName");
            this.Property(t => t.VendorShortName).HasColumnName("VendorShortName");
        }
    }
}
