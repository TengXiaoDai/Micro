using System.Linq.Expressions;

namespace Micro.Respository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> where);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
