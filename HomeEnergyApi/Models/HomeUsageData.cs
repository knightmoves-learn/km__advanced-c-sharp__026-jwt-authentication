using System.Text.Json.Serialization;

namespace HomeEnergyApi.Models
{
    public class HomeUsageData
    {
        public int Id { get; set; }
        public int? MonthlyElectricUsage { get; set; }
        public int HomeId { get; set; }

        [JsonIgnore]
        public Home? Home { get; set; } = null!;

    }

}