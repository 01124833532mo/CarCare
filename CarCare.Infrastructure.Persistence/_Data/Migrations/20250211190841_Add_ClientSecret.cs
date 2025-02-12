using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_ClientSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "ServiceRequests");
        }
    }
}
