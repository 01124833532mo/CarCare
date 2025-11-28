using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityForServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LitersOfFuel",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "LitersOfOil",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "TireCount",
                table: "ServiceRequests");

            migrationBuilder.AddColumn<int>(
                name: "ServiceQuantity",
                table: "ServiceRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceQuantity",
                table: "ServiceRequests");

            migrationBuilder.AddColumn<int>(
                name: "LitersOfFuel",
                table: "ServiceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LitersOfOil",
                table: "ServiceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TireCount",
                table: "ServiceRequests",
                type: "int",
                nullable: true);
        }
    }
}
