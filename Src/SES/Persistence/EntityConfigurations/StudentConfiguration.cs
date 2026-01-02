using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class StudentConfiguration : IEntityTypeConfiguration<Student>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.SurName).IsRequired();
        builder.Property(s => s.SchoolNumber).IsRequired();
        builder.Property(s => s.Gender).IsRequired();
        builder.Property(s => s.IsGhostStudent).IsRequired().HasDefaultValue(false);
        builder.Property(s => s.Description);

        builder.Property(s => s.PreviousStudentId).IsRequired(false);
        builder.Property(s => s.SchoolId);
        builder.Property(s => s.StudentClassId);


        builder.HasOne(s => s.PreviousStudent);
        builder.HasOne(s => s.School);
        builder.HasOne(s => s.StudentClass);

        builder.HasMany(s => s.Exams);
        builder.HasMany(s => s.Teachers);

        builder.Property(s => s.CreatedDate).IsRequired();
        builder.Property(s => s.UpdatedDate);
        builder.Property(s => s.DeletedDate);

        builder.HasQueryFilter(s => !s.DeletedDate.HasValue);
    }
}


