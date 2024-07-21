using Application.Features.Analyses.Constants;
using Application.Features.Auth.Constants;
using Application.Features.Benefits.Constants;
using Application.Features.Exams.Constants;
using Application.Features.LearningAreas.Constants;
using Application.Features.Lessons.Constants;
using Application.Features.OperationClaims.Constants;
using Application.Features.Personels.Constants;
using Application.Features.Principals.Constants;
using Application.Features.QuestionOptions.Constants;
using Application.Features.QuestionScores.Constants;
using Application.Features.ReferenceBenefits.Constants;
using Application.Features.Schools.Constants;
using Application.Features.Semesters.Constants;
using Application.Features.StudentAnswers.Constants;
using Application.Features.StudentClasses.Constants;
using Application.Features.Students.Constants;
using Application.Features.SubLearningAreas.Constants;
using Application.Features.Teachers.Constants;
using Application.Features.UserOperationClaims.Constants;
using Application.Features.Users.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NArchitecture.Core.Security.Constants;

namespace Persistence.EntityConfigurations;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable("OperationClaims").HasKey(oc => oc.Id);

        builder.Property(oc => oc.Id).HasColumnName("Id").IsRequired();
        builder.Property(oc => oc.Name).HasColumnName("Name").IsRequired();
        builder.Property(oc => oc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(oc => oc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(oc => oc.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(oc => !oc.DeletedDate.HasValue);

        builder.HasData(_seeds);

        builder.HasBaseType((string)null!);
    }

    public static int AdminId => 1;
    private IEnumerable<OperationClaim> _seeds
    {
        get
        {
            yield return new() { Id = AdminId, Name = GeneralOperationClaims.Admin };

            IEnumerable<OperationClaim> featureOperationClaims = getFeatureOperationClaims(AdminId);
            foreach (OperationClaim claim in featureOperationClaims)
                yield return claim;
        }
    }

#pragma warning disable S1854 // Unused assignments should be removed
    private IEnumerable<OperationClaim> getFeatureOperationClaims(int initialId)
    {
        int lastId = initialId;
        List<OperationClaim> featureOperationClaims = new();

        #region Real world claims
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = "Unauthorized" },
                new() { Id = ++lastId, Name="Director" },
                new() { Id = ++lastId, Name="AssistantDirector" },
                new() { Id = ++lastId, Name="Teacher" },
                new() { Id = ++lastId, Name="HeadTeacher" },
                new() { Id = ++lastId, Name="Officer" }
            ]
        );
        #endregion

        #region Auth
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = AuthOperationClaims.Admin },
                new() { Id = ++lastId, Name = AuthOperationClaims.Read },
                new() { Id = ++lastId, Name = AuthOperationClaims.Write },
                new() { Id = ++lastId, Name = AuthOperationClaims.RevokeToken },
            ]
        );
        #endregion

        #region OperationClaims
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Admin },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Read },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Write },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Create },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Update },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Delete },
            ]
        );
        #endregion

        #region UserOperationClaims
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Admin },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Read },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Write },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Create },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Update },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Delete },
            ]
        );
        #endregion

        #region Users
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = UsersOperationClaims.Admin },
                new() { Id = ++lastId, Name = UsersOperationClaims.Read },
                new() { Id = ++lastId, Name = UsersOperationClaims.Write },
                new() { Id = ++lastId, Name = UsersOperationClaims.Create },
                new() { Id = ++lastId, Name = UsersOperationClaims.Update },
                new() { Id = ++lastId, Name = UsersOperationClaims.Delete },
            ]
        );
        #endregion


        #region Analyses CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = AnalysesOperationClaims.Admin },
                new() { Id = ++lastId, Name = AnalysesOperationClaims.Read },
                new() { Id = ++lastId, Name = AnalysesOperationClaims.Write },
                new() { Id = ++lastId, Name = AnalysesOperationClaims.Create },
                new() { Id = ++lastId, Name = AnalysesOperationClaims.Update },
                new() { Id = ++lastId, Name = AnalysesOperationClaims.Delete },
            ]
        );
        #endregion


        #region Benefits CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = BenefitsOperationClaims.Admin },
                new() { Id = ++lastId, Name = BenefitsOperationClaims.Read },
                new() { Id = ++lastId, Name = BenefitsOperationClaims.Write },
                new() { Id = ++lastId, Name = BenefitsOperationClaims.Create },
                new() { Id = ++lastId, Name = BenefitsOperationClaims.Update },
                new() { Id = ++lastId, Name = BenefitsOperationClaims.Delete },
            ]
        );
        #endregion


        #region Exams CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = ExamsOperationClaims.Admin },
                new() { Id = ++lastId, Name = ExamsOperationClaims.Read },
                new() { Id = ++lastId, Name = ExamsOperationClaims.Write },
                new() { Id = ++lastId, Name = ExamsOperationClaims.Create },
                new() { Id = ++lastId, Name = ExamsOperationClaims.Update },
                new() { Id = ++lastId, Name = ExamsOperationClaims.Delete },
            ]
        );
        #endregion


        #region LearningAreas CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Admin },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Read },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Write },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Create },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Update },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Delete },
            ]
        );
        #endregion


        #region LearningAreas CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Admin },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Read },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Write },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Create },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Update },
                new() { Id = ++lastId, Name = LearningAreasOperationClaims.Delete },
            ]
        );
        #endregion


        #region Principals CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = PrincipalsOperationClaims.Admin },
                new() { Id = ++lastId, Name = PrincipalsOperationClaims.Read },
                new() { Id = ++lastId, Name = PrincipalsOperationClaims.Write },
                new() { Id = ++lastId, Name = PrincipalsOperationClaims.Create },
                new() { Id = ++lastId, Name = PrincipalsOperationClaims.Update },
                new() { Id = ++lastId, Name = PrincipalsOperationClaims.Delete },
            ]
        );
        #endregion


        #region QuestionOptions CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = QuestionOptionsOperationClaims.Admin },
                new() { Id = ++lastId, Name = QuestionOptionsOperationClaims.Read },
                new() { Id = ++lastId, Name = QuestionOptionsOperationClaims.Write },
                new() { Id = ++lastId, Name = QuestionOptionsOperationClaims.Create },
                new() { Id = ++lastId, Name = QuestionOptionsOperationClaims.Update },
                new() { Id = ++lastId, Name = QuestionOptionsOperationClaims.Delete },
            ]
        );
        #endregion


        #region QuestionScores CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = QuestionScoresOperationClaims.Admin },
                new() { Id = ++lastId, Name = QuestionScoresOperationClaims.Read },
                new() { Id = ++lastId, Name = QuestionScoresOperationClaims.Write },
                new() { Id = ++lastId, Name = QuestionScoresOperationClaims.Create },
                new() { Id = ++lastId, Name = QuestionScoresOperationClaims.Update },
                new() { Id = ++lastId, Name = QuestionScoresOperationClaims.Delete },
            ]
        );
        #endregion


        #region ReferenceBenefits CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = ReferenceBenefitsOperationClaims.Admin },
                new() { Id = ++lastId, Name = ReferenceBenefitsOperationClaims.Read },
                new() { Id = ++lastId, Name = ReferenceBenefitsOperationClaims.Write },
                new() { Id = ++lastId, Name = ReferenceBenefitsOperationClaims.Create },
                new() { Id = ++lastId, Name = ReferenceBenefitsOperationClaims.Update },
                new() { Id = ++lastId, Name = ReferenceBenefitsOperationClaims.Delete },
            ]
        );
        #endregion


        #region Schools CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = SchoolsOperationClaims.Admin },
                new() { Id = ++lastId, Name = SchoolsOperationClaims.Read },
                new() { Id = ++lastId, Name = SchoolsOperationClaims.Write },
                new() { Id = ++lastId, Name = SchoolsOperationClaims.Create },
                new() { Id = ++lastId, Name = SchoolsOperationClaims.Update },
                new() { Id = ++lastId, Name = SchoolsOperationClaims.Delete },
            ]
        );
        #endregion


        #region Semesters CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = SemestersOperationClaims.Admin },
                new() { Id = ++lastId, Name = SemestersOperationClaims.Read },
                new() { Id = ++lastId, Name = SemestersOperationClaims.Write },
                new() { Id = ++lastId, Name = SemestersOperationClaims.Create },
                new() { Id = ++lastId, Name = SemestersOperationClaims.Update },
                new() { Id = ++lastId, Name = SemestersOperationClaims.Delete },
            ]
        );
        #endregion


        #region Students CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = StudentsOperationClaims.Admin },
                new() { Id = ++lastId, Name = StudentsOperationClaims.Read },
                new() { Id = ++lastId, Name = StudentsOperationClaims.Write },
                new() { Id = ++lastId, Name = StudentsOperationClaims.Create },
                new() { Id = ++lastId, Name = StudentsOperationClaims.Update },
                new() { Id = ++lastId, Name = StudentsOperationClaims.Delete },
            ]
        );
        #endregion


        #region StudentAnswers CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = StudentAnswersOperationClaims.Admin },
                new() { Id = ++lastId, Name = StudentAnswersOperationClaims.Read },
                new() { Id = ++lastId, Name = StudentAnswersOperationClaims.Write },
                new() { Id = ++lastId, Name = StudentAnswersOperationClaims.Create },
                new() { Id = ++lastId, Name = StudentAnswersOperationClaims.Update },
                new() { Id = ++lastId, Name = StudentAnswersOperationClaims.Delete },
            ]
        );
        #endregion


        #region SubLearningAreas CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = SubLearningAreasOperationClaims.Admin },
                new() { Id = ++lastId, Name = SubLearningAreasOperationClaims.Read },
                new() { Id = ++lastId, Name = SubLearningAreasOperationClaims.Write },
                new() { Id = ++lastId, Name = SubLearningAreasOperationClaims.Create },
                new() { Id = ++lastId, Name = SubLearningAreasOperationClaims.Update },
                new() { Id = ++lastId, Name = SubLearningAreasOperationClaims.Delete },
            ]
        );
        #endregion


        #region Teachers CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = TeachersOperationClaims.Admin },
                new() { Id = ++lastId, Name = TeachersOperationClaims.Read },
                new() { Id = ++lastId, Name = TeachersOperationClaims.Write },
                new() { Id = ++lastId, Name = TeachersOperationClaims.Create },
                new() { Id = ++lastId, Name = TeachersOperationClaims.Update },
                new() { Id = ++lastId, Name = TeachersOperationClaims.Delete },
            ]
        );
        #endregion


        #region StudentClasses CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = StudentClassesOperationClaims.Admin },
                new() { Id = ++lastId, Name = StudentClassesOperationClaims.Read },
                new() { Id = ++lastId, Name = StudentClassesOperationClaims.Write },
                new() { Id = ++lastId, Name = StudentClassesOperationClaims.Create },
                new() { Id = ++lastId, Name = StudentClassesOperationClaims.Update },
                new() { Id = ++lastId, Name = StudentClassesOperationClaims.Delete },
            ]
        );
        #endregion


        #region Personels CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Admin },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Read },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Write },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Create },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Update },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Delete },
            ]
        );
        #endregion


        #region Lessons CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = LessonsOperationClaims.Admin },
                new() { Id = ++lastId, Name = LessonsOperationClaims.Read },
                new() { Id = ++lastId, Name = LessonsOperationClaims.Write },
                new() { Id = ++lastId, Name = LessonsOperationClaims.Create },
                new() { Id = ++lastId, Name = LessonsOperationClaims.Update },
                new() { Id = ++lastId, Name = LessonsOperationClaims.Delete },
            ]
        );
        #endregion


        #region Personels CRUD
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Admin },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Read },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Write },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Create },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Update },
                new() { Id = ++lastId, Name = PersonelsOperationClaims.Delete },
            ]
        );
        #endregion

        return featureOperationClaims;
    }
#pragma warning restore S1854 // Unused assignments should be removed
}
