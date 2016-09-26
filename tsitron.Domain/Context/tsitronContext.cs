using System.Data.Entity;
using tsitron.Domain.Entitys.Customers;
using tsitron.Domain.Entitys.Goods;
using tsitron.Domain.Entitys.Orders;
using tsitron.Domain.Entitys.Secure;

namespace tsitron.Domain.Context
{
    public class TsitronContext:DbContext
    {
        public TsitronContext():base("tsitron")
        {
           // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TsitronContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UsrRole> UsrRoles { get; set; }
        public DbSet<RegistersLog> RegistersLogs { get; set; }
        public DbSet<Bouquet> Bouquets { get; set; }
        public DbSet<Details> Detailses { get; set; }
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<FlowerType> FlowerTypes { get; set; }
        public DbSet<SingleFlower> SingleFlowers { get; set; }
        public DbSet<FlowerHeight> FlowerHeights { get; set; }
        public DbSet<FlowerImage> FlowerImages { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Requisites> Requisiteses { get; set; }
        public DbSet<FilterColor> FilterColors { get; set; }
        public DbSet<MyShop> MyShops { get; set; }
        public DbSet<Contacts> Contactses { get; set; }
        public DbSet<ImageForCust> ImageForCusts { get; set; }
        public DbSet<GoodsColor> Colors { get; set; }
        public DbSet<FilteredEvents> FilteredEventses { get; set; }
        public DbSet<MyHolydays> Holydayses { get; set; }
        public DbSet<WorkDays> WorkDayses { get; set; }
        public DbSet<ShopGraphic> ShopGraphics { get; set; }
        public DbSet<Accessories> Accessorieses { get; set; } 


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.Entity<Bouquet>().HasRequired(b => b.Price).WithRequiredPrincipal(p=>p.Bouquet).WillCascadeOnDelete();
        }

    }
}
