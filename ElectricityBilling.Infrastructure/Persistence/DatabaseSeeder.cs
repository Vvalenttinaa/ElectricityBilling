using ElectricityBilling.Domain.Entities;
using ElectricityBilling.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectricityBilling.Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {
            if (!await context.Plans.AnyAsync())
            {
                var standardPlan = new Plan
                {
                    Id = Guid.NewGuid(),
                    Name = "Standard",
                    Description = "Standard electricity billing plan",
                    Discount = 0.05m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                    PricingTiers = new List<PricingTier>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = 100,
                            PricePerKwh = 0.10m,
                            SortOrder = 1
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = 300,
                            PricePerKwh = 0.08m,
                            SortOrder = 2
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = 500,
                            PricePerKwh = 0.07m,
                            SortOrder = 3
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = null,
                            PricePerKwh = 0.06m,
                            SortOrder = 4
                        }
                    }
                };

                var premiumPlan = new Plan
                {
                    Id = Guid.NewGuid(),
                    Name = "Premium",
                    Description = "Premium electricity billing plan",
                    Discount = 0.10m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                    PricingTiers = new List<PricingTier>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = 200,
                            PricePerKwh = 0.09m,
                            SortOrder = 1
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = 400,
                            PricePerKwh = 0.07m,
                            SortOrder = 2
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = 600,
                            PricePerKwh = 0.05m,
                            SortOrder = 3
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Threshold = null,
                            PricePerKwh = 0.04m,
                            SortOrder = 4
                        }
                    }
                };

                context.Plans.AddRange(
                    standardPlan,
                    premiumPlan);
            }

            if (!await context.TaxGroups.AnyAsync())
            {
                var household = new TaxGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "household",
                    Description = "Household tax group",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                    TaxRates = new List<TaxRate>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Vat = 0.17m,
                            EcoTax = 0.005m,
                            IsActive = true,
                            ValidFrom = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                };

                var business = new TaxGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "business",
                    Description = "Business tax group",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                    TaxRates = new List<TaxRate>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Vat = 0.20m,
                            EcoTax = 0.010m,
                            IsActive = true,
                            ValidFrom = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                };

                context.TaxGroups.AddRange(
                    household,
                    business);
            }

            if (!await context.Users.AnyAsync())
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Role = "Admin"
                };

                var hasher = new PasswordHasher<User>();
                admin.PasswordHash = hasher.HashPassword(admin, "admin");

                context.Users.Add(admin);

                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
        }
    }
}