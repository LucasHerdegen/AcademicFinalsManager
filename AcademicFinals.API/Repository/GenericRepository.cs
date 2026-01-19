using Microsoft.EntityFrameworkCore;
using AcademicFinalsSpaceManager.API.Models;

namespace AcademicFinalsSpaceManager.API.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> Get() =>
            await _dbSet.AsNoTracking().ToListAsync();


        public virtual async Task<T?> GetById(int id) =>
            await _dbSet.FindAsync(id);


        public virtual async Task Create(T entity) =>
            await _dbSet.AddAsync(entity);


        public virtual void Update(T entity) =>
            _dbSet.Update(entity);


        public virtual void Delete(T entity) =>
            _dbSet.Remove(entity);


        public async Task Save() =>
            await _context.SaveChangesAsync();

    }
}