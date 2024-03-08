using Microsoft.EntityFrameworkCore;
using OnlineMobileServices_Models.Models;
namespace OnlineMobileServices_API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //  RechargePackageHistory -> User
            modelBuilder.Entity<RechargePackageHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.RechargePackageHistories)
                .HasForeignKey(s => s.UserID);
            //  RechargePackageHistory -> RechargePackage
            modelBuilder.Entity<RechargePackageHistory>()
                .HasOne<RechargePackage>(s => s.RechargePackage)
                .WithMany(g => g.RechargePackageHistories)
                .HasForeignKey(s => s.RechargePackageID);
            //  SpecialRechargePackageHistory -> User
            modelBuilder.Entity<SpecialRechargePackageHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.SpecialRechargePackageHistories)
                .HasForeignKey(s => s.UserID);
            //  SpecialRechargePackageHistory -> SpecialRechargePackage
            modelBuilder.Entity<SpecialRechargePackageHistory>()
                .HasOne<SpecialRechargePackage>(s => s.SpecialRechargePackage)
                .WithMany(g => g.SpecialRechargePackageHistories)
                .HasForeignKey(s => s.SpecialRechargePackageID);
            //  ServiceHistory -> User
            modelBuilder.Entity<ServiceHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.ServiceHistories)
                .HasForeignKey(s => s.UserID);
            //  ServiceHistory -> Services
            modelBuilder.Entity<ServiceHistory>()
                .HasOne<Services>(s => s.Service)
                .WithMany(g => g.ServiceHistories)
                .HasForeignKey(s => s.ServiceID);
            //WebsiteSettings.LastEditedBy -> User
            modelBuilder.Entity<WebsiteSettings>()
                .HasOne<User>(s => s.LastEditedBy)
                .WithMany(g => g.WebsiteSettings)
                .HasForeignKey(s => s.LastEditedByID);
            //RechargePackage.Telco -> Telco
            modelBuilder.Entity<RechargePackage>()
                .HasOne<Telco>(s => s.Telco)
                .WithMany(g => g.RechargePackages)
                .HasForeignKey(s => s.TelcoID);
            //SpecialRechargePackage.Telco -> Telco
            modelBuilder.Entity<SpecialRechargePackage>()
                .HasOne<Telco>(s => s.Telco)
                .WithMany(g => g.SpecialRechargePackages)
                .HasForeignKey(s => s.TelcoID);


        }
        public DbSet<User> Users { get; set; }
        public DbSet<RechargePackage> RechargePackages { get; set; }
        public DbSet<SpecialRechargePackage> SpecialRechargePackages { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<RechargePackageHistory> RechargePackageHistories { get; set; }
        public DbSet<SpecialRechargePackageHistory> SpecialRechargePackageHistories { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<Telco> Telcos { get; set; }
        public DbSet<WebsiteSettings> WebsiteSettings { get; set; }


    }
}
