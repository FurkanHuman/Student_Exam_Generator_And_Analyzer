using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class StudentClassConfiguration : IEntityTypeConfiguration<StudentClass>
{
    public void Configure(EntityTypeBuilder<StudentClass> builder)
    {
        builder.ToTable("StudentClasses").HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id).HasColumnName("Id").IsRequired();
        builder.Property(sc => sc.Name).HasColumnName("Name");
        builder.Property(sc => sc.ClassAge).HasColumnName("ClassAge").IsRequired();
        builder.Property(sc => sc.ClassBranch).HasColumnName("ClassBranch").IsRequired();
        builder.Property(sc => sc.Decription).HasColumnName("Decription");
        builder.Property(sc => sc.SchoolId).HasColumnName("SchoolId").IsRequired();
        builder.Property(sc => sc.SemesterId).HasColumnName("SemesterId").IsRequired();
        builder.Property(sc => sc.RefTeacherId).HasColumnName("RefTeacherId").IsRequired();

        builder.HasOne(sc => sc.School);
        builder.HasOne(sc => sc.Semester);
        builder.HasOne(sc => sc.RefTeacher);


        builder.HasMany(sc => sc.Students);
        builder.HasMany(sc => sc.Exams);
        builder.HasMany(sc => sc.Analyses);
        builder.HasMany(sc => sc.Teachers);

        builder.Property(sc => sc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(sc => sc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(sc => sc.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(sc => !sc.DeletedDate.HasValue);
    }
}