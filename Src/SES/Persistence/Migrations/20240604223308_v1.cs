using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_benefits_analysis_headers_analysis_header_id",
                table: "benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_exams_analysis_headers_analysis_header_id",
                table: "exams");

            migrationBuilder.DropForeignKey(
                name: "fk_quiz_questions_analysis_headers_analysis_header_id",
                table: "quiz_questions");

            migrationBuilder.DropForeignKey(
                name: "fk_student_quiz_answers_analysis_headers_analysis_header_id",
                table: "student_quiz_answers");

            migrationBuilder.DropTable(
                name: "benefit_quiz_question");

            migrationBuilder.DropIndex(
                name: "ix_quiz_questions_analysis_header_id",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "end_schcool_year",
                table: "reference_benefits");

            migrationBuilder.DropColumn(
                name: "reference_benefit_season",
                table: "reference_benefits");

            migrationBuilder.DropColumn(
                name: "analysis_header_id",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "exam_semester_year",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "semester",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "semester_sesion",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "semester",
                table: "analysis_headers");

            migrationBuilder.RenameColumn(
                name: "analysis_header_id",
                table: "student_quiz_answers",
                newName: "analysis_id");

            migrationBuilder.RenameIndex(
                name: "ix_student_quiz_answers_analysis_header_id",
                table: "student_quiz_answers",
                newName: "ix_student_quiz_answers_analysis_id");

            migrationBuilder.RenameColumn(
                name: "school_year",
                table: "reference_benefits",
                newName: "semester_id");

            migrationBuilder.RenameColumn(
                name: "analysis_header_id",
                table: "exams",
                newName: "analysis_id");

            migrationBuilder.RenameIndex(
                name: "ix_exams_analysis_header_id",
                table: "exams",
                newName: "ix_exams_analysis_id");

            migrationBuilder.RenameColumn(
                name: "analysis_header_id",
                table: "benefits",
                newName: "analysis_id");

            migrationBuilder.RenameIndex(
                name: "ix_benefits_analysis_header_id",
                table: "benefits",
                newName: "ix_benefits_analysis_id");

            migrationBuilder.RenameColumn(
                name: "semester_session",
                table: "analysis_headers",
                newName: "lesson_session");

            migrationBuilder.AddColumn<int>(
                name: "semester_id",
                table: "teachers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "semester_id",
                table: "students",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "question_body",
                table: "quiz_questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "question_type",
                table: "quiz_questions",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "score",
                table: "quiz_questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "semester_id",
                table: "principals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "semester_id",
                table: "exams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_score",
                table: "exams",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "total_score_for_string",
                table: "exams",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quiz_question_id",
                table: "benefits",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "semester_id",
                table: "analysis_headers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "question_options",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quiz_question_id = table.Column<int>(type: "integer", nullable: false),
                    option_text = table.Column<string>(type: "text", nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_options_quiz_questions_quiz_question_id",
                        column: x => x.quiz_question_id,
                        principalTable: "quiz_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "semesters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    begin_semester_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_semester_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_semesters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "student_answers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    quiz_question_id = table.Column<int>(type: "integer", nullable: false),
                    question_option_id = table.Column<Guid>(type: "uuid", nullable: true),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    analysis_id = table.Column<int>(type: "integer", nullable: true),
                    benefit_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_answers_analysis_headers_analysis_id",
                        column: x => x.analysis_id,
                        principalTable: "analysis_headers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_answers_benefits_benefit_id",
                        column: x => x.benefit_id,
                        principalTable: "benefits",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_answers_question_options_question_option_id",
                        column: x => x.question_option_id,
                        principalTable: "question_options",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_answers_quiz_questions_quiz_question_id",
                        column: x => x.quiz_question_id,
                        principalTable: "quiz_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_answers_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_teachers_semester_id",
                table: "teachers",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_semester_id",
                table: "students",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "ix_reference_benefits_semester_id",
                table: "reference_benefits",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "ix_principals_semester_id",
                table: "principals",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_semester_id",
                table: "exams",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "ix_benefits_quiz_question_id",
                table: "benefits",
                column: "quiz_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_analysis_headers_semester_id",
                table: "analysis_headers",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "ix_question_options_quiz_question_id",
                table: "question_options",
                column: "quiz_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_answers_analysis_id",
                table: "student_answers",
                column: "analysis_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_answers_benefit_id",
                table: "student_answers",
                column: "benefit_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_answers_question_option_id",
                table: "student_answers",
                column: "question_option_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_answers_quiz_question_id",
                table: "student_answers",
                column: "quiz_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_answers_student_id",
                table: "student_answers",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "fk_analysis_headers_semesters_semester_id",
                table: "analysis_headers",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_benefits_analysis_headers_analysis_id",
                table: "benefits",
                column: "analysis_id",
                principalTable: "analysis_headers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_benefits_quiz_questions_quiz_question_id",
                table: "benefits",
                column: "quiz_question_id",
                principalTable: "quiz_questions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_exams_analysis_headers_analysis_id",
                table: "exams",
                column: "analysis_id",
                principalTable: "analysis_headers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_exams_semesters_semester_id",
                table: "exams",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_principals_semesters_semester_id",
                table: "principals",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reference_benefits_semesters_semester_id",
                table: "reference_benefits",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_student_quiz_answers_analysis_headers_analysis_id",
                table: "student_quiz_answers",
                column: "analysis_id",
                principalTable: "analysis_headers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_students_semesters_semester_id",
                table: "students",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_teachers_semesters_semester_id",
                table: "teachers",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_analysis_headers_semesters_semester_id",
                table: "analysis_headers");

            migrationBuilder.DropForeignKey(
                name: "fk_benefits_analysis_headers_analysis_id",
                table: "benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_benefits_quiz_questions_quiz_question_id",
                table: "benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_exams_analysis_headers_analysis_id",
                table: "exams");

            migrationBuilder.DropForeignKey(
                name: "fk_exams_semesters_semester_id",
                table: "exams");

            migrationBuilder.DropForeignKey(
                name: "fk_principals_semesters_semester_id",
                table: "principals");

            migrationBuilder.DropForeignKey(
                name: "fk_reference_benefits_semesters_semester_id",
                table: "reference_benefits");

            migrationBuilder.DropForeignKey(
                name: "fk_student_quiz_answers_analysis_headers_analysis_id",
                table: "student_quiz_answers");

            migrationBuilder.DropForeignKey(
                name: "fk_students_semesters_semester_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "fk_teachers_semesters_semester_id",
                table: "teachers");

            migrationBuilder.DropTable(
                name: "semesters");

            migrationBuilder.DropTable(
                name: "student_answers");

            migrationBuilder.DropTable(
                name: "question_options");

            migrationBuilder.DropIndex(
                name: "ix_teachers_semester_id",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "ix_students_semester_id",
                table: "students");

            migrationBuilder.DropIndex(
                name: "ix_reference_benefits_semester_id",
                table: "reference_benefits");

            migrationBuilder.DropIndex(
                name: "ix_principals_semester_id",
                table: "principals");

            migrationBuilder.DropIndex(
                name: "ix_exams_semester_id",
                table: "exams");

            migrationBuilder.DropIndex(
                name: "ix_benefits_quiz_question_id",
                table: "benefits");

            migrationBuilder.DropIndex(
                name: "ix_analysis_headers_semester_id",
                table: "analysis_headers");

            migrationBuilder.DropColumn(
                name: "semester_id",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "semester_id",
                table: "students");

            migrationBuilder.DropColumn(
                name: "question_body",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "question_type",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "score",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "semester_id",
                table: "principals");

            migrationBuilder.DropColumn(
                name: "semester_id",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "total_score",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "total_score_for_string",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "quiz_question_id",
                table: "benefits");

            migrationBuilder.DropColumn(
                name: "semester_id",
                table: "analysis_headers");

            migrationBuilder.RenameColumn(
                name: "analysis_id",
                table: "student_quiz_answers",
                newName: "analysis_header_id");

            migrationBuilder.RenameIndex(
                name: "ix_student_quiz_answers_analysis_id",
                table: "student_quiz_answers",
                newName: "ix_student_quiz_answers_analysis_header_id");

            migrationBuilder.RenameColumn(
                name: "semester_id",
                table: "reference_benefits",
                newName: "school_year");

            migrationBuilder.RenameColumn(
                name: "analysis_id",
                table: "exams",
                newName: "analysis_header_id");

            migrationBuilder.RenameIndex(
                name: "ix_exams_analysis_id",
                table: "exams",
                newName: "ix_exams_analysis_header_id");

            migrationBuilder.RenameColumn(
                name: "analysis_id",
                table: "benefits",
                newName: "analysis_header_id");

            migrationBuilder.RenameIndex(
                name: "ix_benefits_analysis_id",
                table: "benefits",
                newName: "ix_benefits_analysis_header_id");

            migrationBuilder.RenameColumn(
                name: "lesson_session",
                table: "analysis_headers",
                newName: "semester_session");

            migrationBuilder.AddColumn<int>(
                name: "end_schcool_year",
                table: "reference_benefits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "reference_benefit_season",
                table: "reference_benefits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "analysis_header_id",
                table: "quiz_questions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "exam_semester_year",
                table: "exams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "semester",
                table: "exams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "semester_sesion",
                table: "exams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "semester",
                table: "analysis_headers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "benefit_quiz_question",
                columns: table => new
                {
                    benefits_id = table.Column<int>(type: "integer", nullable: false),
                    quiz_questions_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_benefit_quiz_question", x => new { x.benefits_id, x.quiz_questions_id });
                    table.ForeignKey(
                        name: "fk_benefit_quiz_question_benefits_benefits_id",
                        column: x => x.benefits_id,
                        principalTable: "benefits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_benefit_quiz_question_quiz_questions_quiz_questions_id",
                        column: x => x.quiz_questions_id,
                        principalTable: "quiz_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_quiz_questions_analysis_header_id",
                table: "quiz_questions",
                column: "analysis_header_id");

            migrationBuilder.CreateIndex(
                name: "ix_benefit_quiz_question_quiz_questions_id",
                table: "benefit_quiz_question",
                column: "quiz_questions_id");

            migrationBuilder.AddForeignKey(
                name: "fk_benefits_analysis_headers_analysis_header_id",
                table: "benefits",
                column: "analysis_header_id",
                principalTable: "analysis_headers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_exams_analysis_headers_analysis_header_id",
                table: "exams",
                column: "analysis_header_id",
                principalTable: "analysis_headers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_quiz_questions_analysis_headers_analysis_header_id",
                table: "quiz_questions",
                column: "analysis_header_id",
                principalTable: "analysis_headers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_student_quiz_answers_analysis_headers_analysis_header_id",
                table: "student_quiz_answers",
                column: "analysis_header_id",
                principalTable: "analysis_headers",
                principalColumn: "id");
        }
    }
}
