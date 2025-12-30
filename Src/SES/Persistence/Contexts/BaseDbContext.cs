using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.Contexts;

public class BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : DbContext(dbContextOptions)
{
    protected IConfiguration Configuration { get; set; } = configuration;
    public DbSet<Analysis> Analyses { get; set; }
    public DbSet<Benefit> Benefits { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamConfiguration> ExamConfigurations { get; set; }
    public DbSet<LearningArea> LearningAreas { get; set; }
    public DbSet<Principal> Principals { get; set; }
    public DbSet<QuestionScore> QuestionScores { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<ReferenceBenefit> ReferenceBenefits { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentClass> StudentClasses { get; set; }
    public DbSet<StudentExamAnswer> StudentExamAnswers { get; set; }
    public DbSet<SubLearningArea> SubLearningAreas { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<StudentAnswer> StudentAnswers { get; set; }
    public DbSet<Personel> Personels { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsWithInterface<IMainConfiguration>();

        modelBuilder.HasDefaultSchema("SES_Base_Main");
        base.OnModelCreating(modelBuilder);
    }
}
