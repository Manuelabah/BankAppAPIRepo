using BankAPPAPICore.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace BankAPPAPICoreInfrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal AppDbContext? _appDbContext;
        internal DbSet<TEntity>? dbSet;
        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            dbSet = appDbContext.Set<TEntity>();

        }
        public ICollection<TEntity> GetAll()
        {
            return dbSet.ToList();
        }
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (_appDbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _appDbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void UpdateRange(List<TEntity> entitiesToUpdate)
        {
            dbSet.AttachRange(entitiesToUpdate.ToArray());

            foreach (var entity in entitiesToUpdate)
            {
                _appDbContext.Entry(entity).State = EntityState.Modified;
            }
        }


        public IDbContextTransaction? _dbContextTransaction { get; private set; }

        public void BeginTransaction()
        {
            _dbContextTransaction = _appDbContext.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _dbContextTransaction = _appDbContext.Database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            if (_dbContextTransaction != null)
            {
                _dbContextTransaction.Commit();
            }
        }
        public void RollBack()
        {
            if (_dbContextTransaction != null)
            {
                _dbContextTransaction.Rollback();
            }
        }

    }
}
