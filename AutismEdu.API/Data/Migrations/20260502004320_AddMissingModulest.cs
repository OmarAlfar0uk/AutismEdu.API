using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutismEdu.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingModulest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "PerformanceRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpeechUrl",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FocusArea",
                table: "ChildProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentEmail",
                table: "ChildProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillTags",
                table: "ChildProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ChildProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ChildCards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ChildCards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChildCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ChildCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ChildActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ChildActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChildActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ChildActivities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FocusArea = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_ChildProfiles_ChildId",
                        column: x => x.ChildId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_SpecialistId",
                        column: x => x.SpecialistId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChildLessonLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MasteryPercent = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildLessonLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildLessonLevels_ChildProfiles_ChildId",
                        column: x => x.ChildId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildLessonLevels_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guidelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSizeKb = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guidelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicalObservations = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_ChildProfiles_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ChildId",
                table: "Appointments",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SpecialistId",
                table: "Appointments",
                column: "SpecialistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildLessonLevels_ChildId_LessonId",
                table: "ChildLessonLevels",
                columns: new[] { "ChildId", "LessonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChildLessonLevels_LessonId",
                table: "ChildLessonLevels",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StudentId",
                table: "Reports",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ChildLessonLevels");

            migrationBuilder.DropTable(
                name: "Guidelines");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "PerformanceRecords");

            migrationBuilder.DropColumn(
                name: "SpeechUrl",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "FocusArea",
                table: "ChildProfiles");

            migrationBuilder.DropColumn(
                name: "ParentEmail",
                table: "ChildProfiles");

            migrationBuilder.DropColumn(
                name: "SkillTags",
                table: "ChildProfiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ChildProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ChildCards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChildCards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChildCards");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ChildCards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ChildActivities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChildActivities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChildActivities");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ChildActivities");
        }
    }
}
