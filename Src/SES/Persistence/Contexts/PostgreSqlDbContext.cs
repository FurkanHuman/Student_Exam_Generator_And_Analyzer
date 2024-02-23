using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts;

public class PostgreSqlDbContext : DbContext
{
    public DbSet<AnalysisHeader> AnalysisHeaders { get; set; }
    public DbSet<Benefit> Benefits { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<LearningArea> LearningAreas { get; set; }
    public DbSet<Principal> Principals { get; set; }
    public DbSet<QuestionScore> QuestionScores { get; set; }
    public DbSet<QuizForAnswers> QuizForAnswers { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<ReferenceBenefit> ReferenceBenefits { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentQuizAnswer> StudentQuizAnswers { get; set; }
    public DbSet<SubLearningArea> SubLearningAreas { get; set; }
    public DbSet<Teacher> Teachers { get; set; }


    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
