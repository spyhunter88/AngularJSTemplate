using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Entities.Models.Mapping
{
    public class ObjectActionMap : EntityTypeConfiguration<ObjectAction>
    {
        public ObjectActionMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            this.Property(t => t.Object)
                .HasMaxLength(50);

            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.Action)
                .HasMaxLength(50);

            this.ToTable("ObjectActions");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Object).HasColumnName("Object");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.PublicEnabled).HasColumnName("PublicEnabled");
        }
    }
}
