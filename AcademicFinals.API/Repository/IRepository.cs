using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AcademicFinals.API.Models;

namespace AcademicFinals.API.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T?> GetById(int id);
        Task Create(T create);
        void Update(T update);
        void Delete(T delete);
        Task Save();
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>?> Get(Expression<Func<T, bool>> predicate);
        Task<int> Count(Expression<Func<T, bool>> predicate);
    }
}