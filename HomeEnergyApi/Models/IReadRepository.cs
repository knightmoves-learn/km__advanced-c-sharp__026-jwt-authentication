namespace HomeEnergyApi.Models
{
    public interface IReadRepository<TId, T>
    {
        List<T> FindAll();
        T FindById(TId id);
    }
}