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
            //seed data
            modelBuilder.Entity<TopupOption>().HasData(
                new TopupOption("AED5", 5),
                new TopupOption("AED10", 10),
                new TopupOption("AED20", 20),
                new TopupOption("AED30", 30),
                new TopupOption("AED50", 50),
                new TopupOption("AED75", 75),
                new TopupOption("AED100", 100)
            );
            // build model
            modelBuilder.Entity<TopupOption>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
            });

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
                    PhoneNumber = "1234567890",
                    Name = "John Doe",
                    Remark = "This is active user for active beneficiary",
                    IsVerified = true,
                    Beneficiaries = new List<Beneficiary>
                    {
                        new Beneficiary
                        {
                            NickName = "Jane Doe",
                            PhoneNumber = "1234567890",
                            IsActive = true
                        }
                    }
                }
            );

            modelBuilder.Entity<Beneficiary>(entity =>
            {
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.NickName).IsRequired().HasMaxLength(100);
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.HasOne(e => e.User).WithMany(e => e.Beneficiaries).HasForeignKey(e => e.UserPhoneNumber);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                // decimal(18,4) as standard recomeneded for money   
                entity.Property(e => e.Balance).HasPrecision(18,4);
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);
                entity.HasOne(e => e.User).WithOne(e => e.Account).HasForeignKey<Account>(e => e.UserPhoneNumber);
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
                entity.HasOne(e => e.User).WithMany(e => e.TopupHistories).HasForeignKey(e => e.UserPhoneNumber);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
