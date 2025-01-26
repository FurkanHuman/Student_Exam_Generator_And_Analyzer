using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

public class StudentClassConfiguration : IEntityTypeConfiguration<StudentClass>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<StudentClass> builder)
    {
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id).IsRequired();
        builder.Property(sc => sc.Name);
        builder.Property(sc => sc.ClassAge).IsRequired();
        builder.Property(sc => sc.ClassBranch).IsRequired();
        builder.Property(sc => sc.Decription);
        builder.Property(sc => sc.SchoolId).IsRequired();
        builder.Property(sc => sc.SemesterId).IsRequired();
        builder.Property(sc => sc.RefTeacherId).IsRequired();

        builder.HasOne(sc => sc.School);
        builder.HasOne(sc => sc.Semester);
        builder.HasOne(sc => sc.RefTeacher);


        builder.HasMany(sc => sc.Students);
        builder.HasMany(sc => sc.Exams);
        builder.HasMany(sc => sc.Analyses);
        builder.HasMany(sc => sc.Teachers);

        builder.Property(sc => sc.CreatedDate).IsRequired();
        builder.Property(sc => sc.UpdatedDate);
        builder.Property(sc => sc.DeletedDate);

        builder.HasQueryFilter(sc => !sc.DeletedDate.HasValue);
    }
}