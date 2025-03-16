using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Entities;

namespace Employes.DataLibrary.Context;

/// <summary>
///     Abstract base class for data contexts, providing common database operations
///     and transaction handling across multiple contexts in the application.
/// </summary>
public abstract class DataContext : DbContext, IEmployesDataContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
    protected DataContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{User}" /> representing the Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Gets the current transaction in the context if one exists.
    /// </summary>
    /// <returns>The current transaction, or null if no transaction is active.</returns>
    public virtual IDbContextTransaction? CurrentTransaction()
    {
        return Database.CurrentTransaction;
    }

    /// <summary>
    ///     Begins a new database transaction or returns the existing one.
    /// </summary>
    /// <returns>
    ///     A new <see cref="IDbContextTransaction" /> object if no transaction is active;
    ///     otherwise, returns the current transaction.
    /// </returns>
    public virtual IDbContextTransaction? BeginTransaction()
    {
        if (Database.IsInMemory()) return null;

        if (Database.CurrentTransaction != null) return Database.CurrentTransaction;

        return Database.BeginTransaction();
    }

    /// <summary>
    ///     Checks if the current database is an in-memory database.
    /// </summary>
    /// <returns>true if the database is in-memory; otherwise, false.</returns>
    public virtual bool IsInMemory()
    {
        return Database.IsInMemory();
    }

    /// <summary>
    ///     Commits the specified database transaction.
    /// </summary>
    /// <param name="transaction">The transaction to commit.</param>
    public virtual void Commit(IDbContextTransaction transaction)
    {
        transaction?.Commit();
    }

    /// <summary>
    ///     Rolls back the specified database transaction.
    /// </summary>
    /// <param name="transaction">The transaction to roll back.</param>
    public virtual void RollBack(IDbContextTransaction transaction)
    {
        transaction?.Rollback();
    }

    /// <summary>
    ///     Applies migrations to ensure the database is up to date with the current model.
    /// </summary>
    public virtual void Migrate()
    {
        Database.Migrate();
    }

    /// <summary>
    ///     Locks a specified table for transactional operations.
    /// </summary>
    /// <param name="tableName">The name of the table to lock.</param>
    public virtual void LockTable(string tableName)
    {
        if (!IsInMemory()) Database.ExecuteSqlRaw($"SELECT TOP 1 1 FROM {tableName} WITH (TABLOCKX, HOLDLOCK)");
    }

    /// <summary>
    ///     Gets the database instance used by the context.
    /// </summary>
    /// <returns>A <see cref="DatabaseFacade" /> representing the database operations.</returns>
    public DatabaseFacade GetDatabaseInstance()
    {
        return Database;
    }
}