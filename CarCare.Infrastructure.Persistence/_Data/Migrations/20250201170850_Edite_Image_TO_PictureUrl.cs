using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class Edite_Image_TO_PictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ServiceTypes",
                newName: "PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "ServiceTypes",
                newName: "Image");
        }
    }
}
