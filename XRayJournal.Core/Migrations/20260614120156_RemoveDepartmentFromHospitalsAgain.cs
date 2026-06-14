using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XRayJournal.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepartmentFromHospitalsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Hospitals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Hospitals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
