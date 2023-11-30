using BankAPPAPICoreInfrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankAPPAPICoreInfrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IDatabaseTransaction BeginTransaction();
    }
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }

    public interface IDatabaseTransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
