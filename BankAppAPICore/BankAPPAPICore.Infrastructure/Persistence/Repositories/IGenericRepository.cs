using System.Data;
using System.Linq.Expressions;

namespace BankAPPAPICoreInfrastructure.Persistence.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        ICollection<TEntity> GetAll();
        IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        void UpdateRange(List<TEntity> entitiesToUpdate);


        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        void Commit();
        void RollBack();
    }
}
