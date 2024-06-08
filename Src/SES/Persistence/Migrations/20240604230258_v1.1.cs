using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schools_teachers_teacher_id",
                table: "schools");

            migrationBuilder.DropIndex(
                name: "ix_schools_teacher_id",
                table: "schools");

            migrationBuilder.DropColumn(
                name: "teacher_id",
                table: "schools");

            migrationBuilder.AddColumn<int>(
                name: "school_id",
                table: "principals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_teachers_school_id",
                table: "teachers",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_principals_school_id",
                table: "principals",
                column: "school_id");

            migrationBuilder.AddForeignKey(
                name: "fk_principals_schools_school_id",
                table: "principals",
                column: "school_id",
                principalTable: "schools",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_teachers_schools_school_id",
                table: "teachers",
                column: "school_id",
                principalTable: "schools",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_principals_schools_school_id",
                table: "principals");

            migrationBuilder.DropForeignKey(
                name: "fk_teachers_schools_school_id",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "ix_teachers_school_id",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "ix_principals_school_id",
                table: "principals");

            migrationBuilder.DropColumn(
                name: "school_id",
                table: "principals");

            migrationBuilder.AddColumn<int>(
                name: "teacher_id",
                table: "schools",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_schools_teacher_id",
                table: "schools",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schools_teachers_teacher_id",
                table: "schools",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "id");
        }
    }
}
