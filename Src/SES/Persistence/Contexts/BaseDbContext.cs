using App.SchoolFeature;
using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Persistence.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }

    public DbSet<Exam> Exams { get; set; }
    public DbSet<Principal> Principals { get; set; }
    public DbSet<QuestionScore> QuestionScores { get; set; }
    public DbSet<QuizForAnswers> QuizForAnswers { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet< ReferenceBenefit> ReferenceBenefits { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentQuizAnswer> StudentQuizAnswers { get; set; }
    public DbSet<Teacher> Teachers { get; set; }


    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration)
        : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
