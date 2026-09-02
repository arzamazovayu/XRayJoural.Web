using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XRayJournal.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashAndPwDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "PwDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1845, 3, 27));

            migrationBuilder.AddColumn<string>(
                name: "PwHash",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                @"UPDATE ""Users""
                  SET ""PasswordHash"" = '$2a$10$EixZaYVKzfsbwjZGlc1b4eJk4Q5HfZ6qK7d8L9mN0pO1rS2tU3vW4x',
                      ""PwDate"" = '1845-03-27 00:00:00'
                  WHERE ""PasswordHash"" = '';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PwDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PwHash",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
