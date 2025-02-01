using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_NormalizedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "ServiceTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "ServiceTypes");
        }
    }
}
