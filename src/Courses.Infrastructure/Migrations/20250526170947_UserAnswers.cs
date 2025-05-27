using System;
using System.Collections.Generic;
using Courses.Domain.Questions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTestAnswer");

            migrationBuilder.DropTable(
                name: "UserTestAttempt");

            migrationBuilder.CreateTable(
                name: "TestAttempt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAttempt_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestAttempt_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttemptQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Answers = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttemptQuestion_TestAttempt_TestAttemptId",
                        column: x => x.TestAttemptId,
                        principalTable: "TestAttempt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttemptQuestion_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttemptQuestion_Id",
                table: "AttemptQuestion",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttemptQuestion_TestAttemptId",
                table: "AttemptQuestion",
                column: "TestAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptQuestion_TestId",
                table: "AttemptQuestion",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_TestId",
                table: "TestAttempt",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempt_UserId",
                table: "TestAttempt",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttemptQuestion");

            migrationBuilder.DropTable(
                name: "TestAttempt");

            migrationBuilder.CreateTable(
                name: "UserTestAttempt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTestAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTestAttempt_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTestAttempt_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTestAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTestAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    PossibleAnswers = table.Column<IReadOnlyCollection<Answer>>(type: "jsonb", nullable: false),
                    QuestionContent = table.Column<string>(type: "text", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionOrder = table.Column<int>(type: "integer", nullable: false),
                    SelectedAnswerIds = table.Column<IReadOnlyCollection<Guid>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTestAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTestAnswer_UserTestAttempt_UserTestAttemptId",
                        column: x => x.UserTestAttemptId,
                        principalTable: "UserTestAttempt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTestAnswer_UserTestAttemptId",
                table: "UserTestAnswer",
                column: "UserTestAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestAttempt_Id",
                table: "UserTestAttempt",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTestAttempt_TestId",
                table: "UserTestAttempt",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestAttempt_UserId",
                table: "UserTestAttempt",
                column: "UserId");
        }
    }
}
