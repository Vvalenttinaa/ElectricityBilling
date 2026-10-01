using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ElectricityBilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PricingTiers",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "TaxGroups",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "TaxGroups",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "CreatedAt", "Description", "Discount", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Standard electricity billing plan", 0.05m, true, "Standard", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium electricity billing plan", 0.10m, true, "Premium", null }
                });

            migrationBuilder.InsertData(
                table: "TaxGroups",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Household tax group", true, "household", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Business tax group", true, "business", null }
                });

            migrationBuilder.InsertData(
                table: "PricingTiers",
                columns: new[] { "Id", "PlanId", "PricePerKwh", "SortOrder", "Threshold" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111"), 0.10m, 1, 100m },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111"), 0.08m, 2, 300m },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111"), 0.07m, 3, 500m },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111"), 0.06m, 4, null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222"), 0.09m, 1, 200m },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222"), 0.07m, 2, 400m },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("22222222-2222-2222-2222-222222222222"), 0.05m, 3, 600m },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("22222222-2222-2222-2222-222222222222"), 0.04m, 4, null }
                });

            migrationBuilder.InsertData(
                table: "TaxRates",
                columns: new[] { "Id", "CreatedAt", "EcoTax", "IsActive", "TaxGroupId", "UpdatedAt", "ValidFrom", "ValidTo", "Vat" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.005m, true, new Guid("33333333-3333-3333-3333-333333333333"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0.17m },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.010m, true, new Guid("44444444-4444-4444-4444-444444444444"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0.20m }
                });
        }
    }
}
