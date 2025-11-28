using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCare.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRequestModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<double>(
                name: "TechLatitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TechLongitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TechRate",
                table: "AspNetUsers",
                type: "decimal(2,1)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TechId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TireSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TireCount = table.Column<int>(type: "int", nullable: true),
                    TypeOfFuel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LitersOfFuel = table.Column<int>(type: "int", nullable: true),
                    BettaryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfOil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LitersOfOil = table.Column<int>(type: "int", nullable: true),
                    TypeOfWinch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServicePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasePrice = table.Column<int>(type: "int", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_AspNetUsers_TechId",
                        column: x => x.TechId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequests_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ServiceTypeId",
                table: "ServiceRequests",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_TechId",
                table: "ServiceRequests",
                column: "TechId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_UserId",
                table: "ServiceRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TechLatitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TechLongitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TechRate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "ServiceTypes",
                newName: "Image");
        }
    }
}
