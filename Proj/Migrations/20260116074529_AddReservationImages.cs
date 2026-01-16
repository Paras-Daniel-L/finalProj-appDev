using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proj.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProofOfPaymentPath",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ValidIdPath",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProofOfPaymentPath",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ValidIdPath",
                table: "Reservations");
        }
    }
}
