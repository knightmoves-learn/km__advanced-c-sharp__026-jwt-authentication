namespace HomeEnergyApi.Models
{
    public class UtilityProvider
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<string>? ProvidedUtilities { get; set; }
        public ICollection<HomeUtilityProvider> HomeUtilityProviders { get; set; }
    }
}
