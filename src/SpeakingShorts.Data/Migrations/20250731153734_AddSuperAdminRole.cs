using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakingShorts.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 15, 37, 33, 960, DateTimeKind.Utc).AddTicks(7776));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 31, 15, 37, 33, 960, DateTimeKind.Utc).AddTicks(7777));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 17, 29, 11, 538, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 17, 29, 11, 538, DateTimeKind.Utc).AddTicks(4869));
        }
    }
}
