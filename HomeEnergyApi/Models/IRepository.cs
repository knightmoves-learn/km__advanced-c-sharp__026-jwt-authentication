using System.Collections.Generic;

namespace HomeEnergyApi.Models
{
    public interface IRepository<TId, T>
    {
        T Save(T entity);
        T Update(TId id, T entity);
        List<T> FindAll();
        T FindById(TId id);
        T RemoveById(TId id);
    }
}