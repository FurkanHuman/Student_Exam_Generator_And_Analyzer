using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Persistence.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
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

    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration)
        : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
