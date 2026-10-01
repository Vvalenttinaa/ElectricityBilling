using ElectricityBilling.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectricityBilling.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plan> Plans => Set<Plan>();

        public DbSet<PricingTier> PricingTiers => Set<PricingTier>();

        public DbSet<TaxGroup> TaxGroups => Set<TaxGroup>();

        public DbSet<TaxRate> TaxRates => Set<TaxRate>();

        public DbSet<User> Users { get; set; }

        public DbSet<RecommendationLog> RecommendationLogs
    => Set<RecommendationLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PLAN
            modelBuilder.Entity<Plan>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.Discount)
                    .HasPrecision(5, 4);

                entity.HasMany(x => x.PricingTiers)
                    .WithOne(x => x.Plan)
                    .HasForeignKey(x => x.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PRICING TIER
            modelBuilder.Entity<PricingTier>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.PricePerKwh)
                    .HasPrecision(10, 4);

                entity.Property(x => x.Threshold)
                    .HasPrecision(10, 2);
            });

            // TAX GROUP
            modelBuilder.Entity<TaxGroup>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.HasMany(x => x.TaxRates)
                    .WithOne(x => x.TaxGroup)
                    .HasForeignKey(x => x.TaxGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TAX RATE
            modelBuilder.Entity<TaxRate>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Vat)
                    .HasPrecision(5, 4);

                entity.Property(x => x.EcoTax)
                    .HasPrecision(5, 4);
            });

            // USER
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Username)
                    .IsUnique();

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.Role)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // RECOMMENDATION LOG
            modelBuilder.Entity<RecommendationLog>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.RecommendedPlan)
                    .WithMany()
                    .HasForeignKey(x => x.RecommendedPlanId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.TaxGroup)
                    .WithMany()
                    .HasForeignKey(x => x.TaxGroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

