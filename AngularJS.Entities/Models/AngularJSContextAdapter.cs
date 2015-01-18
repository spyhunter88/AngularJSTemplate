using FrameLog.Contexts;
using FrameLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Entities.Models
{
    public class AngularJSContextAdapter : DbContextAdapter<ChangeSet, User>
    {
        private AngularJSContext context;

        public AngularJSContextAdapter(AngularJSContext context)
            : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<User>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<User>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<User>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }
        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }
        public override Type UnderlyingContextType
        {
            get { return typeof(AngularJSContext); }
        }
    }
}
