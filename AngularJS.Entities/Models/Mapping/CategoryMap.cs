using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AngularJS.Entities.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryID);

            // Properties
            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Category");
            this.Property(t => t.CategoryID).HasColumnName("ID");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
