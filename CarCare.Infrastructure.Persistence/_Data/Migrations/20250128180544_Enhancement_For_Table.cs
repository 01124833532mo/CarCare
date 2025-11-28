using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class Enhancement_For_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CarNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceTypes");

            migrationBuilder.RenameColumn(
                name: "CarPlate",
                table: "Vehicles",
                newName: "VIN_Number");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_CarPlate",
                table: "Vehicles",
                newName: "IX_Vehicles_VIN_Number");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumber",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PlateNumber",
                table: "Vehicles",
                column: "PlateNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_PlateNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PlateNumber",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "VIN_Number",
                table: "Vehicles",
                newName: "CarPlate");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_VIN_Number",
                table: "Vehicles",
                newName: "IX_Vehicles_CarPlate");

            migrationBuilder.AddColumn<int>(
                name: "CarNumber",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceTypes",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CarNumber",
                table: "Vehicles",
                column: "CarNumber",
                unique: true);
        }
    }
}
