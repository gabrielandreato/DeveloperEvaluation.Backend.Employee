using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Entities;

namespace Employes.DataLibrary.Context;

public interface IEmployeeDataContext
{

    bool IsInMemory();
    Task<int> SaveChangesAsync(CancellationToken token = default);
    DbSet<User> Users { get; set; }
}
