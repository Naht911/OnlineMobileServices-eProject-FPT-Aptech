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
            modelBuilder.Entity<RechargeHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.RechargePackageHistories)
                .HasForeignKey(s => s.UserID);
            //  RechargePackageHistory -> RechargePackage
            modelBuilder.Entity<RechargeHistory>()
                .HasOne<RechargePackage>(s => s.OriginalPackage)
                .WithMany(g => g.RechargePackageHistories)
                .HasForeignKey(s => s.PackageID);
            //  SpecialRechargePackageHistory -> User
            modelBuilder.Entity<SpecialRechargeHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.SpecialRechargePackageHistories)
                .HasForeignKey(s => s.UserID);
            //  SpecialRechargePackageHistory -> SpecialRechargePackage
            modelBuilder.Entity<SpecialRechargeHistory>()
                .HasOne<SpecialRechargePackage>(s => s.OriginalPackage)
                .WithMany(g => g.SpecialRechargePackageHistories)
                .HasForeignKey(s => s.PackageID);
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
            //Đổi lại tên bảng RechargePackageHistory -> RechargeHistory
            modelBuilder.Entity<RechargeHistory>().ToTable("RechargeHistory");
            //Đổi lại tên bảng SpecialRechargePackageHistory -> SpecialRechargeHistory
            modelBuilder.Entity<SpecialRechargeHistory>().ToTable("SpecialRechargeHistory");
            //DoNotDisturbHistory->User
            modelBuilder.Entity<DoNotDisturbHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.DoNotDisturbHistories)
                .HasForeignKey(s => s.UserID);
            //CallerTunesHistory->User
            modelBuilder.Entity<CallerTunesHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.CallerTunesHistories)
                .HasForeignKey(s => s.UserID);
            //CallerTunesHistory->CallerTunesPackage
            modelBuilder.Entity<CallerTunesHistory>()
                .HasOne<CallerTunesPackage>(s => s.CallerTunesPackage)
                .WithMany(g => g.CallerTunesHistories)
                .HasForeignKey(s => s.PackageID);
                //PostPaidHistory->User
            modelBuilder.Entity<PostPaidHistory>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.PostPaidHistories)
                .HasForeignKey(s => s.UserID);


        }
        public DbSet<User> Users { get; set; }
        public DbSet<RechargePackage> RechargePackages { get; set; }
        public DbSet<SpecialRechargePackage> SpecialRechargePackages { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<RechargeHistory> RechargeHistories { get; set; }
        public DbSet<SpecialRechargeHistory> SpecialRechargeHistories { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<Telco> Telcos { get; set; }
        public DbSet<WebsiteSettings> WebsiteSettings { get; set; }
        public DbSet<DoNotDisturbHistory> DoNotDisturbHistories { get; set; }
        public DbSet<CallerTunesPackage> CallerTunesPackages { get; set; }
        public DbSet<CallerTunesHistory> CallerTunesHistories { get; set; }
        public DbSet<PostPaidHistory> PostPaidHistories { get; set; }


    }
}
