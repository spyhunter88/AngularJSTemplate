using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AngularJS.Entities.Models.Mapping
{
    public class CheckPointMap : EntityTypeConfiguration<CheckPoint>
    {
        public CheckPointMap()
        {
            // Primary Key
            this.HasKey(t => t.CheckPointID);

            // Properties
            this.Property(t => t.Action)
                .HasMaxLength(255);

            this.Property(t => t.Note)
                .HasMaxLength(255);


            // Mapping
            this.ToTable("CheckPoints");
            this.Property(t => t.CheckPointID).HasColumnName("CheckPointID");
            this.Property(t => t.ClaimID).HasColumnName("ClaimID");
            this.Property(t => t.CheckDate).HasColumnName("CheckDate");
            this.Property(t => t.WarningDate).HasColumnName("WarningDate");
            this.Property(t => t.ReportDate).HasColumnName("ReportDate");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SendMailCount).HasColumnName("SendMailCount");
            this.Property(t => t.SendMailMax).HasColumnName("SendMailMax");

            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");

            this.Property(t => t.LastEditBy).HasColumnName("LastEditBy");
            this.Property(t => t.LastEditTime).HasColumnName("LastEditTime");
        }

    }
}
