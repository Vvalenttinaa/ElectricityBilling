using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Domain.Entities;
using ElectricityBilling.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Json;



namespace ElectricityBilling.Tests.Integration;

public class RecommendationsControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RecommendationsControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostRecommendation_WithValidRequest_ReturnsCheapestPlan()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();

        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Ukloni PostgreSQL DbContext registraciju
                services.RemoveAll<ApplicationDbContext>();

                services.RemoveAll<
                    DbContextOptions<ApplicationDbContext>>();

                services.RemoveAll<
                    IDbContextOptionsConfiguration<ApplicationDbContext>>();

                // Za integration test koristimo InMemory bazu
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                });
            });
        });

        using var scope = factory.Services.CreateScope();

        var context =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var householdId = Guid.NewGuid();

        var household = new TaxGroup
        {
            Id = householdId,
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

        var standard = new Plan
        {
            Id = Guid.NewGuid(),
            Name = "Standard",
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
                    Threshold = null,
                    PricePerKwh = 0.06m,
                    SortOrder = 3
                }
            }
        };

        var premium = new Plan
        {
            Id = Guid.NewGuid(),
            Name = "Premium",
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
                    Threshold = null,
                    PricePerKwh = 0.04m,
                    SortOrder = 3
                }
            }
        };

        context.TaxGroups.Add(household);
        context.Plans.AddRange(standard, premium);

        await context.SaveChangesAsync();

        var client = factory.CreateClient();

        var request = new RecommendationRequestDto
        {
            MonthlyConsumption = 250,
            TaxGroupId = householdId
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Recommendations",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<RecommendationResponseDto>();

        Assert.NotNull(result);
        Assert.NotNull(result.RecommendedPlan);

        Assert.Equal(
            "Premium",
            result.RecommendedPlan.PlanName);

        Assert.Equal(2, result.AllPlans.Count);

        Assert.Equal(
            result.AllPlans.Min(p => p.TotalCost),
            result.RecommendedPlan.TotalCost);
    }
    [Fact]
    public async Task PostRecommendation_WithNegativeConsumption_ReturnsBadRequest()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();

        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ApplicationDbContext>();

                services.RemoveAll<
                    DbContextOptions<ApplicationDbContext>>();

                services.RemoveAll<
                    IDbContextOptionsConfiguration<ApplicationDbContext>>();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                });
            });
        });

        var client = factory.CreateClient();

        var request = new RecommendationRequestDto
        {
            MonthlyConsumption = -100,
            TaxGroupId = Guid.NewGuid()
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Recommendations",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
    [Fact]
    public async Task PostRecommendation_WithNonExistingTaxGroup_ReturnsBadRequest()
    {
        // Arrange
        var databaseName = Guid.NewGuid().ToString();

        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ApplicationDbContext>();

                services.RemoveAll<
                    DbContextOptions<ApplicationDbContext>>();

                services.RemoveAll<
                    IDbContextOptionsConfiguration<ApplicationDbContext>>();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                });
            });
        });

        var client = factory.CreateClient();

        var request = new RecommendationRequestDto
        {
            MonthlyConsumption = 250,
            TaxGroupId = Guid.NewGuid()
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/Recommendations",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}