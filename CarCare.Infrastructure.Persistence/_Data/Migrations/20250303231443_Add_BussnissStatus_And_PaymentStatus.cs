using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_BussnissStatus_And_PaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ServiceRequests",
                newName: "PaymentStatus");

            migrationBuilder.AddColumn<string>(
                name: "BusnissStatus",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusnissStatus",
                table: "ServiceRequests");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "ServiceRequests",
                newName: "Status");
        }
    }
}
