using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicalProfit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TechProfit",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechProfit",
                table: "AspNetUsers");
        }
    }
}
