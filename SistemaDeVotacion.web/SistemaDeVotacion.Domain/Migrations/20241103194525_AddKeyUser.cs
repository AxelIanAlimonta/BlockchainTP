using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeVotacion.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_IdentityUserId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdentityUserId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdentityUserId1",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_Id",
                table: "Users",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_Id",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId1",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityUserId1",
                table: "Users",
                column: "IdentityUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_IdentityUserId1",
                table: "Users",
                column: "IdentityUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
