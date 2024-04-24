using System.Linq.Expressions;

namespace Micro.Respository
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetByConditionWithCompile(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> where);
        Task<T> GetByIdAsync(int id);
        Task<T> FirstOrDefualt(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
