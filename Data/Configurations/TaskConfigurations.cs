using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamTaskManager.Domain;

namespace TeamTaskManager.Data.Specifications
{
    public class TaskConfigurations : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(t => t.Description)
                .HasMaxLength(1500);

            builder.HasOne(t => t.Project)
                   .WithMany()
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.User) 
                   .WithMany() 
                   .HasForeignKey(t => t.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Property(t => t.TaskStatus)
                   .HasConversion<string>()
                   .IsRequired();
        }
    }
}
