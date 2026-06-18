using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutismEdu.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDataIsolationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ============================================================
            // ISSUE 1 FIX: Add CreatedBy to all BaseEntity tables
            // This enables ownership tracking for data isolation.
            // ============================================================

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ChatBotLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ChildActivities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ChildCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ChildLessonLevels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ChildProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "CommunicationCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Guidelines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PerformanceRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TTSContents",
                type: "uniqueidentifier",
                nullable: true);

            // ============================================================
            // Add SpecialistId to entity-specific tables for ownership
            // ============================================================

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialistId",
                table: "CommunicationCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialistId",
                table: "Guidelines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialistId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop SpecialistId columns
            migrationBuilder.DropColumn(name: "SpecialistId", table: "CommunicationCards");
            migrationBuilder.DropColumn(name: "SpecialistId", table: "Guidelines");
            migrationBuilder.DropColumn(name: "SpecialistId", table: "Lessons");

            // Drop CreatedBy columns
            migrationBuilder.DropColumn(name: "CreatedBy", table: "Activities");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "Appointments");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "ChatBotLogs");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "ChildActivities");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "ChildCards");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "ChildLessonLevels");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "ChildProfiles");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "CommunicationCards");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "Guidelines");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "Lessons");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "PerformanceRecords");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "Reports");
            migrationBuilder.DropColumn(name: "CreatedBy", table: "TTSContents");
        }
    }
}
