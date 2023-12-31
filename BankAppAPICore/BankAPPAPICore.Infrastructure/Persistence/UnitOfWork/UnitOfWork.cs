﻿using BankAPPAPICore.Infrastructure.Persistence.Context;
using BankAPPAPICoreInfrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankAPPAPICoreInfrastructure.Persistence.UnitOfWork
{
    public interface IRepositoryFactory
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
    public class EntityDatabaseTransaction : IDatabaseTransaction
    {
        private IDbContextTransaction _transaction;

        public EntityDatabaseTransaction(DbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork
        where TContext : AppDbContext, IDisposable
    {
        private  Dictionary<Type, object>? _repositories;
        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(Context);
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            Context?.SaveChanges();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);          
            if (!_repositories.ContainsKey(type)) _repositories[type] = new GenericRepository<TEntity>(Context); 
            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
