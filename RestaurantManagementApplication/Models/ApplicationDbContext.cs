using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Menu { get; set; }
        //public DbSet<Bill> Carts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=RestaurantManagementDb;Trusted_Connection=True;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u=>u.Profile)
                .WithMany(p=>p.Users)
                .HasForeignKey(u=>u.ProfileId);
            
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bills)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(b => b.Booking)
                .WithMany(u => u.Orders)
                .HasForeignKey(b => b.BookingId);

            modelBuilder.Entity<Order>()
                .HasOne(i => i.Item)
                .WithMany(u => u.Orders)
                .HasForeignKey(i => i.ItemId);

            modelBuilder.Entity<Booking>()
                .Property(e => e.BookingTime)
                .HasConversion(
                    v => DateTime.ParseExact(v, "g", CultureInfo.InvariantCulture),
                    v => v.ToString("g", CultureInfo.InvariantCulture));

            modelBuilder.Entity<Bill>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Bill>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Item>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Item>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);
        }
    }
}
