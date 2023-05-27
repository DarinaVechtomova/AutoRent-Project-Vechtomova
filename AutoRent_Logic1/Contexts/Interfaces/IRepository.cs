using System.Collections.Generic;
namespace AutoRent_Logic.Contexts.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        bool Remove(T entity);
        bool UpData(T entity);
        T FindByID(int id);
        IEnumerable<T> GetAll();
    }
}
