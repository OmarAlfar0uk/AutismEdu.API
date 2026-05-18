using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutismEdu.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeChildParentOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildProfiles_AspNetUsers_UserId",
                table: "ChildProfiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ChildProfiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildProfiles_AspNetUsers_UserId",
                table: "ChildProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildProfiles_AspNetUsers_UserId",
                table: "ChildProfiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ChildProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildProfiles_AspNetUsers_UserId",
                table: "ChildProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
