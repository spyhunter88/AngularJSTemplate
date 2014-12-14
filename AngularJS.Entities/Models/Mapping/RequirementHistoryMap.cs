using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AngularJS.Entities.Models.Mapping
{
    public class RequirementHistoryMap : EntityTypeConfiguration<RequirementHistory>
    {
        public RequirementHistoryMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.UpdateNote)
                .HasMaxLength(255);


            // Mapping
            this.ToTable("RequirementHistory");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RequirementID).HasColumnName("RequirementID");
            this.Property(t => t.CheckPointID).HasColumnName("CheckPointID");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.UpdateAmount).HasColumnName("UpdateAmount");
            this.Property(t => t.UpdateNote).HasColumnName("UpdateNote");

            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");

            this.Property(t => t.LastEditBy).HasColumnName("LastEditBy");
            this.Property(t => t.LastEditTime).HasColumnName("LastEditTime");
        }
    }
}
