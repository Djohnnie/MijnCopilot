using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MijnCopilot.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MessageAgentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgentName",
                table: "MESSAGES",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentName",
                table: "MESSAGES");
        }
    }
}
