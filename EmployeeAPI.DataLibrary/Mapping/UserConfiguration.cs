using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelLibrary.Entities;

namespace Employes.DataLibrary.Mapping
{
    /// <summary>
    /// Configures the database schema for the User entity.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the properties and relationships of the User entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);

            builder.HasIndex(u => u.DocumentNumber).IsUnique();
            builder.Property(u => u.DocumentNumber).IsRequired().HasMaxLength(20);

            builder.HasOne(u => u.Manager)
                .WithMany()
                .HasForeignKey(u => u.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.Role)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);

            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(20);  

            builder.Property(u => u.DateOfBirth).IsRequired();
            
            builder.HasMany(u => u.PhoneNumbers)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}