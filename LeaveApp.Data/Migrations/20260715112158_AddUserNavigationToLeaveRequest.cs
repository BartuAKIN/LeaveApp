using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNavigationToLeaveRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_UserId",
                table: "LeaveRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_UserId",
                table: "LeaveRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_AspNetUsers_UserId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_UserId",
                table: "LeaveRequests");
        }
    }
}
