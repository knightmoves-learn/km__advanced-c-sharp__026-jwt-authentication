using System.ComponentModel.DataAnnotations;
using HomeEnergyApi.Validations;

namespace HomeEnergyApi.Dtos
{
    [HomeStreetAddressValid]
    public class HomeDto
    {
        [Required]
        public string? OwnerLastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public int? MonthlyElectricUsage { get; set; }
        public ICollection<int>? UtilityProviderIds { get; set; }
    }
}
