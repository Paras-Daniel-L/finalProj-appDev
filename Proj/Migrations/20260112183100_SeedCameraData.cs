using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Proj.Migrations
{
    /// <inheritdoc />
    public partial class SeedCameraData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cameras",
                columns: new[] { "Id", "Brand", "Description", "ImageUrl", "IsAvailable", "Model", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Canon", "Compact digital camera", "ixus.jpg", true, "IXUS", "CANON IXUS", 500m },
                    { 2, "Canon", "Professional DSLR camera", "eos.jpg", true, "EOS", "CANON EOS", 1500m },
                    { 3, "Canon", "High-end vlogging camera", "g7x.jpg", true, "G7X", "CANON G7X", 800m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cameras",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cameras",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cameras",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
