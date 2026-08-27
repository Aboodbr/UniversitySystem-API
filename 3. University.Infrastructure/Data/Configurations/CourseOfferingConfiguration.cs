using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Domain.Entities;

namespace University.Infrastructure.Data.Configurations;

public class CourseOfferingConfiguration : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(EntityTypeBuilder<CourseOffering> builder)
    {
        builder.HasKey(co => co.Id);

        builder.HasOne(co => co.Course)
            .WithMany(c => c.CourseOfferings)
            .HasForeignKey(co => co.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(co => co.Semester)
            .WithMany(s => s.CourseOfferings)
            .HasForeignKey(co => co.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(co => co.Professor)
            .WithMany(p => p.CourseOfferings)
            .HasForeignKey(co => co.ProfessorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(co => co.Room)
            .WithMany(r => r.CourseOfferings)
            .HasForeignKey(co => co.RoomId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
