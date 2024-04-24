using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Micro.Respository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MicroContext _context;
        public BaseRepository(MicroContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> where)
        {
            return await _context.Set<T>().Where(where).ToListAsync();
        }
        public IEnumerable<T> GetByConditionWithCompile(Expression<Func<T, bool>> where) 
        {
            Func<MicroContext, IEnumerable<T>> compileQuery =EF.CompileQuery((MicroContext context) =>
          context.Set<T>().Where(where))!;
          return  compileQuery(_context).ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> FirstOrDefualt(Expression<Func<T,bool>> where) 
        {
            return await _context.Set<T>().FirstOrDefaultAsync(where);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Entry<T>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
