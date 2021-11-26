using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxTemperature = table.Column<int>(type: "int", nullable: false),
                    AvgTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinTemperature = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temperatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "getdate()"),
                    CelsiusDegrees = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temperatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temperatures_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "AvgTemperature", "MaxTemperature", "MinTemperature", "Name" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), 12m, 12, 11, "Kharkiv" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "AvgTemperature", "MaxTemperature", "MinTemperature", "Name" },
                values: new object[] { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), 12m, 12, 11, "Kyiv" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "AvgTemperature", "MaxTemperature", "MinTemperature", "Name" },
                values: new object[] { new Guid("b2434fb3-c190-4402-8a53-933fed036753"), 12m, 12, 11, "Lviv" });

            migrationBuilder.InsertData(
                table: "Temperatures",
                columns: new[] { "Id", "CelsiusDegrees", "CityId", "DateTime" },
                values: new object[,]
                {
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), 15, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), new DateTime(2021, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7eb16423-225d-4ba4-8372-1ce2586fe765"), 14, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), new DateTime(2021, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("def22be0-5a3d-484c-9d34-d7f81322c971"), 17, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), new DateTime(2021, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1ae21e68-a465-4684-961c-1c2fe29c35c2"), 15, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), new DateTime(2021, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f3017f31-ee17-4445-9bf8-0cd153258090"), 12, new Guid("b2434fb3-c190-4402-8a53-933fed036753"), new DateTime(2021, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("01430a7d-b7a6-469b-9618-8fb8217dcfc6"), 10, new Guid("b2434fb3-c190-4402-8a53-933fed036753"), new DateTime(2021, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Temperatures_CityId",
                table: "Temperatures",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Temperatures");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
