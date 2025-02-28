namespace HomeEnergyApi.Dtos
{
    public class UtilityProviderDto
    {
        public string Name { get; set; }
        public ICollection<string> ProvidedUtilities { get; set; }
    }
}
