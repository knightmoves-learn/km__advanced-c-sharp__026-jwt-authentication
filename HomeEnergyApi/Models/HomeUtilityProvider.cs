using System.Text.Json.Serialization;

namespace HomeEnergyApi.Models
{
    public class HomeUtilityProvider
    {
        public int Id { get; set; }
        public int UtilityProviderId { get; set; }
        [JsonIgnore]
        public UtilityProvider UtilityProvider { get; set; }
        public int HomeId { get; set; }
        [JsonIgnore]
        public Home Home { get; set; }
    }
}