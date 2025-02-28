namespace HomeEnergyApi.Models
{
    public interface IOwnerLastNameQueryable<T>
    {
        List<T> FindByOwnerLastName(string ownerLastName);
    }
}