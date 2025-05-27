using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TopicTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_Topic_TopicId",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_TopicId",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Test");

            migrationBuilder.AddColumn<Guid>(
                name: "TestId",
                table: "Topic",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_TestId",
                table: "Topic",
                column: "TestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Test_TestId",
                table: "Topic",
                column: "TestId",
                principalTable: "Test",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Test_TestId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_TestId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Topic");

            migrationBuilder.AddColumn<Guid>(
                name: "TopicId",
                table: "Test",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Test_TopicId",
                table: "Test",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Topic_TopicId",
                table: "Test",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
