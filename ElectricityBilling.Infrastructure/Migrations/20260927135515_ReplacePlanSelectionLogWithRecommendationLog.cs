using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityBilling.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePlanSelectionLogWithRecommendationLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanSelectionLogs");

            migrationBuilder.CreateTable(
                name: "RecommendationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumptionKwh = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecommendedPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationLogs_Plans_RecommendedPlanId",
                        column: x => x.RecommendedPlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecommendationLogs_TaxGroups_TaxGroupId",
                        column: x => x.TaxGroupId,
                        principalTable: "TaxGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationLogs_RecommendedPlanId",
                table: "RecommendationLogs",
                column: "RecommendedPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationLogs_TaxGroupId",
                table: "RecommendationLogs",
                column: "TaxGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendationLogs");

            migrationBuilder.CreateTable(
                name: "PlanSelectionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Consumption = table.Column<decimal>(type: "numeric", nullable: true),
                    SelectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanSelectionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanSelectionLogs_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanSelectionLogs_PlanId",
                table: "PlanSelectionLogs",
                column: "PlanId");
        }
    }
}
