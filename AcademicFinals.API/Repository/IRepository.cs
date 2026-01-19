using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademicFinalsSpaceManager.API.Models;

namespace AcademicFinalsSpaceManager.API.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T?> GetById(int id);
        Task Create(T create);
        void Update(T update);
        void Delete(T delete);
        Task Save();
    }
}