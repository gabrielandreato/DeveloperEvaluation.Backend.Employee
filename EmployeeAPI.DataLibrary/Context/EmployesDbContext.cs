using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Employes.DataLibrary.Context;

/// <summary>
///     Represents the database context for the Employes domain, responsible for managing
///     data access and providing configurations for entities in a SQL database.
/// </summary>
public class EmployesDbContext : DataContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmployesDbContext" /> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
    public EmployesDbContext(DbContextOptions<EmployesDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Configures the model for the context by applying entity configurations
    ///     from the executing assembly.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}