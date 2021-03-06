﻿using System.Data.Entity;
using AngularJS.Entities.Models.Mapping;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public partial class AngularJSContext : DataContext
    {
        static AngularJSContext()
        {
            Database.SetInitializer<AngularJSContext>(null);
        }

        public AngularJSContext()
            : base("Name=AngularJSContext")
        {
            // Logger = new FrameLogModule<ChangeSet, User>(new ChangeSetFactory(), FrameLogContext, filterProvider);
        }        

        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerDemographic> CustomerDemographics { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; }
		
		public DbSet<Claim> Claims { get; set; }
		public DbSet<Request> Requests { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CheckPoint> CheckPoints { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RequirementHistory> RequirementHistories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ProductLine> ProductLines { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CustomerDemographicMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new ShipperMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new TerritoryMap());
            modelBuilder.Configurations.Add(new OrderDetailsExtendedMap());
			modelBuilder.Configurations.Add(new ClaimMap());
			modelBuilder.Configurations.Add(new RequestMap());
            modelBuilder.Configurations.Add(new MenuItemMap());
            modelBuilder.Configurations.Add(new CheckPointMap());
            modelBuilder.Configurations.Add(new RequirementMap());
            modelBuilder.Configurations.Add(new RequirementHistoryMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new ProductLineMap());
            modelBuilder.Configurations.Add(new VendorMap());


            modelBuilder.Entity<CheckPoint>().HasRequired<Claim>(s => s.Claim)
                .WithMany(s => s.CheckPoints).HasForeignKey(s => s.ClaimID);

            modelBuilder.Entity<Requirement>().HasRequired<Claim>(s => s.Claim)
                .WithMany(s => s.Requirements).HasForeignKey(s => s.ClaimID);

            modelBuilder.Entity<RequirementHistory>().HasRequired<Requirement>(s => s.Requirement)
                .WithMany(s => s.RequirementHistories).HasForeignKey(s => s.RequirementID);

            modelBuilder.Entity<RequirementHistory>().HasRequired<CheckPoint>(s => s.CheckPoint)
                .WithMany(s => s.RequirementHistories).HasForeignKey(s => s.CheckPointID);
        }
    }
}
