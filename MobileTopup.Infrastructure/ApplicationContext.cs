using Microsoft.EntityFrameworkCore;
using MobileTopup.Contracts.Domain.Entities;

namespace MobileTopup.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<TopupOption> TopupOptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TopupHistory> TopupHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // build model
            modelBuilder.Entity<TopupOption>(entity =>
            {
                entity.HasKey(e => e.Id);
                //Id is auto increment 
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Amount).IsRequired();
            });

            //seed data
            modelBuilder.Entity<TopupOption>().HasData(
                new TopupOption { Id = 1, Name = "AED5", Amount = 5 },
                new TopupOption { Id = 2, Name = "AED10", Amount = 10 },
                new TopupOption { Id = 3, Name = "AED20", Amount = 20 },
                new TopupOption { Id = 4, Name = "AED30", Amount = 30 },
                new TopupOption { Id = 5, Name = "AED50", Amount = 50 },
                new TopupOption { Id = 6, Name = "AED75", Amount = 75 },
                new TopupOption { Id = 7, Name = "AED100", Amount = 100 }
            );

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                //UAE phone number LENTH is 12 max phone number lenis international standard is 15
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(20).IsRequired();
            });

            // add seed to user 
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    PhoneNumber = "1234567890",
                    Name = "John Doe",
                    Remark = "This is active user for active beneficiary",
                    IsVerified = true,
                }
            );

            modelBuilder.Entity<Beneficiary>(entity =>
            {
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.NickName).IsRequired().HasMaxLength(100);
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.HasOne(e => e.User).WithMany(e => e.Beneficiaries).HasForeignKey(e => e.UserId);
            });

            // add seed to user 
            modelBuilder.Entity<Beneficiary>().HasData(
                new Beneficiary
                {
                    Id = 1,
                    PhoneNumber = "1234567890",
                    NickName = "John Doe",
                    UserId = 1,
                    IsActive = true
                }
            );

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                // decimal(18,4) as standard recomeneded for money   
                entity.Property(e => e.Balance).HasPrecision(18,4);
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.HasOne(e => e.User).WithOne(e => e.Account).HasForeignKey<Account>(e => e.UserId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Amount).HasPrecision(18,4);
                entity.Property(e => e.Date).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.HasOne(e => e.Account).WithMany(e => e.Transactions).HasForeignKey(e => e.AccountId);
            });
            
            modelBuilder.Entity<TopupHistory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Amount).HasPrecision(18,4);
                entity.Property(e => e.Date).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.HasOne(e => e.User).WithMany(e => e.TopupHistories).HasForeignKey(e => e.UserId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
