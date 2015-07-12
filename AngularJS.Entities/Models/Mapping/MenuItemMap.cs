using System.Data.Entity.ModelConfiguration;

namespace AngularJS.Entities.Models.Mapping
{
    class MenuItemMap : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Href)
                .HasMaxLength(255);

            this.Property(t => t.Title)
                .HasMaxLength(255);

            this.Property(t => t.Icon)
                .HasMaxLength(255);
				
			this.Property(t => t.Route)
				.HasMaxLength(255);

            this.Property(t => t.Module)
                .HasMaxLength(50);

            // Mapping
            this.ToTable("MenuItem");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Href).HasColumnName("Href");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Icon).HasColumnName("Icon");
            this.Property(t => t.Route).HasColumnName("Route");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.Module).HasColumnName("Module");
        }
    }
}
