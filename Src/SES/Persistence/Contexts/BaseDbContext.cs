using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityConfigurations;

namespace Persistence.Contexts;

public class BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : DbContext(dbContextOptions)
{
    protected IConfiguration Configuration { get; set; } = configuration;
    public DbSet<Analysis> AnalysisHeaders { get; set; }
    public DbSet<Benefit> Benefits { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<LearningArea> LearningAreas { get; set; }
    public DbSet<Principal> Principals { get; set; }
    public DbSet<QuestionScore> QuestionScores { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<ReferenceBenefit> ReferenceBenefits { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentClass> StudentClasses { get; set; }
    public DbSet<SubLearningArea> SubLearningAreas { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<StudentAnswer> StudentAnswers { get; set; }
    public DbSet<Analysis> Analyses { get; set; }
    public DbSet<Personel> Personels { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnalysisConfiguration());
        modelBuilder.ApplyConfiguration(new BenefitConfiguration());
        modelBuilder.ApplyConfiguration(new ExamConfiguration());
        modelBuilder.ApplyConfiguration(new LearningAreaConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        modelBuilder.ApplyConfiguration(new PersonelConfiguration());
        modelBuilder.ApplyConfiguration(new PrincipalConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionOptionConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionScoreConfiguration());
        modelBuilder.ApplyConfiguration(new QuizQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new ReferenceBenefitConfiguration());
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new SemesterConfiguration());
        modelBuilder.ApplyConfiguration(new StudentAnswerConfiguration());
        modelBuilder.ApplyConfiguration(new StudentClassConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new SubLearningAreaConfiguration());
        modelBuilder.ApplyConfiguration(new TeacherConfiguration());

        modelBuilder.HasDefaultSchema("SES_Base_Main");
        base.OnModelCreating(modelBuilder);
    }
}
