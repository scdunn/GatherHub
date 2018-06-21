using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cidean.GatherHub.Core.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetById(int id);

        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);

    }
}
