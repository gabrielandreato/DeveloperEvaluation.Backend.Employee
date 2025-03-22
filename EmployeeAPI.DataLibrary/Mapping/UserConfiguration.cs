using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelLibrary.Entities;
using ModelLibrary.Enums;

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
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.Property(s => s.Id).IsRequired();

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);

            builder.Ignore(u => u.Password);
            builder.Ignore(u => u.RePassword);
            
            builder.HasIndex(u => u.DocumentNumber).IsUnique();
            builder.Property(u => u.DocumentNumber).IsRequired().HasMaxLength(20);
            
            builder.Property(u => u.HashPassword).IsRequired();

            builder.HasOne(u => u.Manager)
                .WithMany()
                .HasForeignKey(u => u.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.Role)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.UserName).IsRequired();
            builder.HasIndex(u => u.UserName).IsUnique();
            
            builder.Property(u => u.DateOfBirth).IsRequired();
            
            builder.HasMany(u => u.PhoneNumbers)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            var user = new User
            {
                Id = -1,
                UserName = "admin",
                Email = "admin@sys.com",
                FirstName = "Admin",
                LastName = "User",
                Password = "admin",
                RePassword = "admin",
                HashPassword = "",
                DocumentNumber = "123",
                DateOfBirth = DateTime.Now,
                Role = Role.Admin,
                PhoneNumbers = new List<PhoneNumber>() 
            };
            
            user.SetEncryptedPassword();

            builder.HasData(user);
        }
    }
}