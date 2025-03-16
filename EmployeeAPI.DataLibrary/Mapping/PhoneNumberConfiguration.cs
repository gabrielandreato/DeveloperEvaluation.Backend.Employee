using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employes.DataLibrary.Mapping;

/// <summary>
/// Configures the database schema for the PhoneNumber entity.
/// </summary>
public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    /// <summary>
    /// Configures the properties and relationships of the PhoneNumber entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.ToTable("PhoneNumbers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Number)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(p => p.User)
            .WithMany(u => u.PhoneNumbers)
            .HasForeignKey(p => p.UserId);
    }
}