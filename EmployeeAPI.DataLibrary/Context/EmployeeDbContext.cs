using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Entities;

namespace Employes.DataLibrary.Context;

/// <summary>
///     Represents the database context for the Employes domain, responsible for managing
///     data access and providing configurations for entities in a SQL database.
/// </summary>
public class EmployeeDbContext : DbContext, IEmployeeDataContext
{

    public EmployeeDbContext(DbContextOptions options) : base(options)
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
    
    /// <summary>
    ///     Gets or sets the <see cref="DbSet{User}" /> representing the Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Checks if the current database is an in-memory database.
    /// </summary>
    /// <returns>true if the database is in-memory; otherwise, false.</returns>
    public virtual bool IsInMemory()
    {
        return Database.IsInMemory();
    }
}