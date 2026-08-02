using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamTaskManager.Domain;

namespace TeamTaskManager.Data.Specifications
{
    public class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .HasDefaultValueSql("NEWID()")
                   .IsRequired();
                  
            builder.Property(p => p.Name)
                   .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(p => p.CreatedBy)
                   .WithMany()
                   .HasForeignKey(p => p.CreatedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
