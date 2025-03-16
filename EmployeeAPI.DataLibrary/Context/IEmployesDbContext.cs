using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Entities;

namespace Employes.DataLibrary.Context;

public interface IEmployesDataContext
{
    DbSet<User> Users { get; set; }

    IDbContextTransaction? CurrentTransaction();
    IDbContextTransaction? BeginTransaction();
    bool IsInMemory();
    void Commit(IDbContextTransaction transaction);
    void RollBack(IDbContextTransaction transaction);
    void Migrate();
    void LockTable(string tableName);
    DatabaseFacade GetDatabaseInstance();
}