namespace HomeEnergyApi.Models
{
    public interface IWriteRepository<TId, T>
    {
        T Save(T entity);
        T Update(TId id, T entity);
        T RemoveById(TId id);
        int Count();
    }
}