using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;
using System.Text.Json;

namespace Persistence.EntityConfigurations;

internal class ExamConfiguration : IEntityTypeConfiguration<Exam>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ExamLessonName).IsRequired();
        builder.Property(e => e.ExamCode).IsRequired();
        builder.Property(e => e.TotalScoreForString);
        builder.Property(e => e.TotalScore);
        builder.Property(e => e.FooterNote).IsRequired();
        builder.Property(e => e.ExamDate).IsRequired();
        builder.Property(e => e.EvaluationOrigin).IsRequired();
        builder.Property(e => e.QuestionOrderMap).IsRequired()
                                                 .HasConversion(v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default), 
                                                                v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, JsonSerializerOptions.Default)!
                                                                );

        builder.Property(e => e.LessonId).IsRequired();
        builder.Property(e => e.SemesterId).IsRequired();
        builder.Property(e => e.StudentId).IsRequired();
        builder.Property(e => e.SchoolId).IsRequired();
        builder.Property(e => e.ReferenceBenefitId).IsRequired();
        builder.Property(e => e.ExamAuthorId).IsRequired();

        builder.HasOne(e => e.Lesson);
        builder.HasOne(e => e.Semester);
        builder.HasOne(e => e.Student);
        builder.HasOne(e => e.School);
        builder.HasOne(e => e.ReferenceBenefit);
        builder.HasOne(e => e.ExamAuthor).WithMany(t => t.ExamAuthors).HasForeignKey(e => e.ExamAuthorId);
        builder.HasMany(e => e.Analyses);
        builder.HasMany(e => e.Teachers);
        builder.HasMany(e => e.StudentClasses);
        builder.HasMany(e => e.QuizQuestions);
        builder.HasMany(e => e.StudentAnswers);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}