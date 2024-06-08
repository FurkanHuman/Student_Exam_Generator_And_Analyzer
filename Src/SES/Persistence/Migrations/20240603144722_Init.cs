using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "principals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    sur_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_principals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exam_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    sur_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schools",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schools", x => x.id);
                    table.ForeignKey(
                        name: "fk_schools_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "analysis_headers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_age = table.Column<int>(type: "integer", nullable: false),
                    alt_class = table.Column<char>(type: "character(1)", nullable: false),
                    exam_semester_year = table.Column<string>(type: "text", nullable: false),
                    lesson_name = table.Column<string>(type: "text", nullable: false),
                    semester = table.Column<string>(type: "text", nullable: false),
                    semester_session = table.Column<string>(type: "text", nullable: false),
                    exam_code = table.Column<string>(type: "text", nullable: false),
                    footer_note = table.Column<string>(type: "text", nullable: false),
                    benefit_id = table.Column<int>(type: "integer", nullable: false),
                    question_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    principal_id = table.Column<int>(type: "integer", nullable: false),
                    student_quiz_answer_id = table.Column<int>(type: "integer", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    ref_score_per_questions = table.Column<int[]>(type: "integer[]", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analysis_headers", x => x.id);
                    table.ForeignKey(
                        name: "fk_analysis_headers_principals_principal_id",
                        column: x => x.principal_id,
                        principalTable: "principals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_analysis_headers_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_analysis_headers_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reference_benefits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reference_benefit_name = table.Column<string>(type: "text", nullable: false),
                    reference_benefit_season = table.Column<string>(type: "text", nullable: false),
                    school_year = table.Column<int>(type: "integer", nullable: false),
                    end_schcool_year = table.Column<int>(type: "integer", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    exam_id = table.Column<int>(type: "integer", nullable: false),
                    learning_area_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reference_benefits", x => x.id);
                    table.ForeignKey(
                        name: "fk_reference_benefits_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reference_benefits_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_age = table.Column<int>(type: "integer", nullable: false),
                    class_branch = table.Column<char>(type: "character(1)", nullable: false),
                    school_number = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<char>(type: "character(1)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    exam_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    sur_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                    table.ForeignKey(
                        name: "fk_students_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "learning_areas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    sub_learning_area_id = table.Column<int>(type: "integer", nullable: false),
                    reference_benefit_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_learning_areas", x => x.id);
                    table.ForeignKey(
                        name: "fk_learning_areas_reference_benefits_reference_benefit_id",
                        column: x => x.reference_benefit_id,
                        principalTable: "reference_benefits",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quiz_questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    benefit_id = table.Column<int>(type: "integer", nullable: false),
                    exam_id = table.Column<int>(type: "integer", nullable: false),
                    question = table.Column<string>(type: "text", nullable: false),
                    question_image = table.Column<string>(type: "text", nullable: false),
                    analysis_header_id = table.Column<int>(type: "integer", nullable: true),
                    reference_benefit_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quiz_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_quiz_questions_analysis_headers_analysis_header_id",
                        column: x => x.analysis_header_id,
                        principalTable: "analysis_headers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quiz_questions_reference_benefits_reference_benefit_id",
                        column: x => x.reference_benefit_id,
                        principalTable: "reference_benefits",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exam_semester_year = table.Column<string>(type: "text", nullable: false),
                    lesson_name = table.Column<string>(type: "text", nullable: false),
                    semester = table.Column<string>(type: "text", nullable: false),
                    semester_sesion = table.Column<string>(type: "text", nullable: false),
                    exam_code = table.Column<string>(type: "text", nullable: false),
                    footer_note = table.Column<string>(type: "text", nullable: false),
                    analysis_header_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    reference_benefit_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exams", x => x.id);
                    table.ForeignKey(
                        name: "fk_exams_analysis_headers_analysis_header_id",
                        column: x => x.analysis_header_id,
                        principalTable: "analysis_headers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exams_reference_benefits_reference_benefit_id",
                        column: x => x.reference_benefit_id,
                        principalTable: "reference_benefits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exams_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exams_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exams_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_teacher",
                columns: table => new
                {
                    students_id = table.Column<int>(type: "integer", nullable: false),
                    teachers_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_teacher", x => new { x.students_id, x.teachers_id });
                    table.ForeignKey(
                        name: "fk_student_teacher_students_students_id",
                        column: x => x.students_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_teacher_teachers_teachers_id",
                        column: x => x.teachers_id,
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sub_learning_areas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    benefit_id = table.Column<int>(type: "integer", nullable: false),
                    learning_area_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sub_learning_areas", x => x.id);
                    table.ForeignKey(
                        name: "fk_sub_learning_areas_learning_areas_learning_area_id",
                        column: x => x.learning_area_id,
                        principalTable: "learning_areas",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exam_quiz_question",
                columns: table => new
                {
                    exams_id = table.Column<int>(type: "integer", nullable: false),
                    quiz_questions_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_quiz_question", x => new { x.exams_id, x.quiz_questions_id });
                    table.ForeignKey(
                        name: "fk_exam_quiz_question_exams_exams_id",
                        column: x => x.exams_id,
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_quiz_question_quiz_questions_quiz_questions_id",
                        column: x => x.quiz_questions_id,
                        principalTable: "quiz_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_quiz_answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_number = table.Column<string>(type: "text", nullable: false),
                    exam_id = table.Column<int>(type: "integer", nullable: false),
                    total_score = table.Column<int>(type: "integer", nullable: false),
                    special_statuses = table.Column<char[]>(type: "character(1)[]", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    analysis_header_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_quiz_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_quiz_answers_analysis_headers_analysis_header_id",
                        column: x => x.analysis_header_id,
                        principalTable: "analysis_headers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_quiz_answers_exams_exam_id",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_quiz_answers_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "benefits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sub_learning_area_id = table.Column<int>(type: "integer", nullable: false),
                    reference_benefit_number = table.Column<string>(type: "text", nullable: false),
                    reference_benefit_comments = table.Column<string>(type: "text", nullable: false),
                    analysis_header_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_benefits", x => x.id);
                    table.ForeignKey(
                        name: "fk_benefits_analysis_headers_analysis_header_id",
                        column: x => x.analysis_header_id,
                        principalTable: "analysis_headers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_benefits_sub_learning_areas_sub_learning_area_id",
                        column: x => x.sub_learning_area_id,
                        principalTable: "sub_learning_areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question_scores",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    score = table.Column<int>(type: "integer", nullable: false),
                    max_score = table.Column<int>(type: "integer", nullable: false),
                    student_quiz_answer_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question_scores", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_scores_student_quiz_answers_student_quiz_answer_id",
                        column: x => x.student_quiz_answer_id,
                        principalTable: "student_quiz_answers",
                        principalColumn: "id");
                });

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
                name: "ix_analysis_headers_principal_id",
                table: "analysis_headers",
                column: "principal_id");

            migrationBuilder.CreateIndex(
                name: "ix_analysis_headers_school_id",
                table: "analysis_headers",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_analysis_headers_teacher_id",
                table: "analysis_headers",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_benefit_quiz_question_quiz_questions_id",
                table: "benefit_quiz_question",
                column: "quiz_questions_id");

            migrationBuilder.CreateIndex(
                name: "ix_benefits_analysis_header_id",
                table: "benefits",
                column: "analysis_header_id");

            migrationBuilder.CreateIndex(
                name: "ix_benefits_sub_learning_area_id",
                table: "benefits",
                column: "sub_learning_area_id");

            migrationBuilder.CreateIndex(
                name: "ix_exam_quiz_question_quiz_questions_id",
                table: "exam_quiz_question",
                column: "quiz_questions_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_analysis_header_id",
                table: "exams",
                column: "analysis_header_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_reference_benefit_id",
                table: "exams",
                column: "reference_benefit_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_school_id",
                table: "exams",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_student_id",
                table: "exams",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_exams_teacher_id",
                table: "exams",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_learning_areas_reference_benefit_id",
                table: "learning_areas",
                column: "reference_benefit_id");

            migrationBuilder.CreateIndex(
                name: "ix_question_scores_student_quiz_answer_id",
                table: "question_scores",
                column: "student_quiz_answer_id");

            migrationBuilder.CreateIndex(
                name: "ix_quiz_questions_analysis_header_id",
                table: "quiz_questions",
                column: "analysis_header_id");

            migrationBuilder.CreateIndex(
                name: "ix_quiz_questions_reference_benefit_id",
                table: "quiz_questions",
                column: "reference_benefit_id");

            migrationBuilder.CreateIndex(
                name: "ix_reference_benefits_school_id",
                table: "reference_benefits",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_reference_benefits_teacher_id",
                table: "reference_benefits",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_schools_teacher_id",
                table: "schools",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_quiz_answers_analysis_header_id",
                table: "student_quiz_answers",
                column: "analysis_header_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_quiz_answers_exam_id",
                table: "student_quiz_answers",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_quiz_answers_student_id",
                table: "student_quiz_answers",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_teacher_teachers_id",
                table: "student_teacher",
                column: "teachers_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_school_id",
                table: "students",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_sub_learning_areas_learning_area_id",
                table: "sub_learning_areas",
                column: "learning_area_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "benefit_quiz_question");

            migrationBuilder.DropTable(
                name: "exam_quiz_question");

            migrationBuilder.DropTable(
                name: "question_scores");

            migrationBuilder.DropTable(
                name: "student_teacher");

            migrationBuilder.DropTable(
                name: "benefits");

            migrationBuilder.DropTable(
                name: "quiz_questions");

            migrationBuilder.DropTable(
                name: "student_quiz_answers");

            migrationBuilder.DropTable(
                name: "sub_learning_areas");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "learning_areas");

            migrationBuilder.DropTable(
                name: "analysis_headers");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "reference_benefits");

            migrationBuilder.DropTable(
                name: "principals");

            migrationBuilder.DropTable(
                name: "schools");

            migrationBuilder.DropTable(
                name: "teachers");
        }
    }
}
