using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MijnCopilot.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHATS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActivityOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHATS", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "MESSAGES",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SysId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReduced = table.Column<bool>(type: "bit", nullable: false),
                    TokensUsed = table.Column<int>(type: "int", nullable: false),
                    PostedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MESSAGES", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_MESSAGES_CHATS_ChatId",
                        column: x => x.ChatId,
                        principalTable: "CHATS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHATS_StartedOn",
                table: "CHATS",
                column: "StartedOn");

            migrationBuilder.CreateIndex(
                name: "IX_CHATS_SysId",
                table: "CHATS",
                column: "SysId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_MESSAGES_ChatId",
                table: "MESSAGES",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_MESSAGES_PostedOn",
                table: "MESSAGES",
                column: "PostedOn");

            migrationBuilder.CreateIndex(
                name: "IX_MESSAGES_SysId",
                table: "MESSAGES",
                column: "SysId")
                .Annotation("SqlServer:Clustered", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MESSAGES");

            migrationBuilder.DropTable(
                name: "CHATS");
        }
    }
}
