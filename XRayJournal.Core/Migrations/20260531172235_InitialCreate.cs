using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace XRayJournal.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepName = table.Column<string>(type: "text", nullable: false),
                    Clinic = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Numbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YearlyNum = table.Column<int>(type: "integer", nullable: false),
                    DailyNum = table.Column<int>(type: "integer", nullable: false),
                    XRayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Numbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SecondName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    ThirdName = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Sex = table.Column<string>(type: "text", nullable: false),
                    MedNumber = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    FIO = table.Column<string>(type: "text", nullable: false),
                    FIOshort = table.Column<string>(type: "text", nullable: false),
                    CabinetId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cabinets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CabinetName = table.Column<string>(type: "text", nullable: false),
                    Clinic = table.Column<string>(type: "text", nullable: true),
                    CabNum = table.Column<string>(type: "text", nullable: true),
                    Modality = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    IdClinic = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabinets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cabinets_Hospitals_IdClinic",
                        column: x => x.IdClinic,
                        principalTable: "Hospitals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CabinetsUsers",
                columns: table => new
                {
                    CabinetsId = table.Column<int>(type: "integer", nullable: false),
                    UsersID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinetsUsers", x => new { x.CabinetsId, x.UsersID });
                    table.ForeignKey(
                        name: "FK_CabinetsUsers_Cabinets_CabinetsId",
                        column: x => x.CabinetsId,
                        principalTable: "Cabinets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CabinetsUsers_Users_UsersID",
                        column: x => x.UsersID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    XRayName = table.Column<string>(type: "text", nullable: false),
                    XRayDose = table.Column<float>(type: "real", nullable: false),
                    XRayShots = table.Column<int>(type: "integer", nullable: false),
                    XRayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    XRayDiagnose = table.Column<string>(type: "text", nullable: true),
                    XRayPatology = table.Column<bool>(type: "boolean", nullable: true),
                    XRayCost = table.Column<int>(type: "integer", nullable: true),
                    Doctor = table.Column<string>(type: "text", nullable: true),
                    Laborant = table.Column<string>(type: "text", nullable: true),
                    XRayModality = table.Column<string>(type: "text", nullable: true),
                    InOperation = table.Column<bool>(type: "boolean", nullable: true),
                    Contrast = table.Column<bool>(type: "boolean", nullable: true),
                    IdCabinet = table.Column<int>(type: "integer", nullable: true),
                    Side = table.Column<string>(type: "text", nullable: true),
                    Area = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Cabinets_IdCabinet",
                        column: x => x.IdCabinet,
                        principalTable: "Cabinets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    NumberId = table.Column<int>(type: "integer", nullable: false),
                    ExamId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Records_Numbers_NumberId",
                        column: x => x.NumberId,
                        principalTable: "Numbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Records_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Records_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cabinets_IdClinic",
                table: "Cabinets",
                column: "IdClinic");

            migrationBuilder.CreateIndex(
                name: "IX_CabinetsUsers_UsersID",
                table: "CabinetsUsers",
                column: "UsersID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_IdCabinet",
                table: "Exams",
                column: "IdCabinet");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ExamId",
                table: "Records",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_NumberId",
                table: "Records",
                column: "NumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_PatientId",
                table: "Records",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserId",
                table: "Records",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabinetsUsers");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Numbers");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cabinets");

            migrationBuilder.DropTable(
                name: "Hospitals");
        }
    }
}
