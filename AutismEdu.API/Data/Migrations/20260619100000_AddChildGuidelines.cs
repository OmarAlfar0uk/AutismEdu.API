using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutismEdu.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChildGuidelines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChildGuidelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuidelineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildGuidelines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildGuidelines_ChildProfiles_ChildId",
                        column: x => x.ChildId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildGuidelines_Guidelines_GuidelineId",
                        column: x => x.GuidelineId,
                        principalTable: "Guidelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildGuidelines_ChildId",
                table: "ChildGuidelines",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildGuidelines_GuidelineId",
                table: "ChildGuidelines",
                column: "GuidelineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildGuidelines");
        }
    }
}
