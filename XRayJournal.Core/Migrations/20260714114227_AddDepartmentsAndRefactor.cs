using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace XRayJournal.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentsAndRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepName",
                table: "Hospitals");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepName = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    HospitalId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_DepartmentId",
                table: "Exams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HospitalId",
                table: "Departments",
                column: "HospitalId");

            migrationBuilder.Sql(@"
                INSERT INTO ""Departments"" (""Id"", ""DepName"", ""FullName"", ""HospitalId"")
                VALUES (1, 'н/у', 'Не установлено', 1)
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Departments_DepartmentId",
                table: "Exams",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Departments_DepartmentId",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Exams_DepartmentId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Exams");

            migrationBuilder.AddColumn<string>(
                name: "DepName",
                table: "Hospitals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
