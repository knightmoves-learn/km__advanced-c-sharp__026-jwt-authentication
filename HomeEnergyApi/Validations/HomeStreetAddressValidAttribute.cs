using System.ComponentModel.DataAnnotations;
using HomeEnergyApi.Dtos;

namespace HomeEnergyApi.Validations
{
    public class HomeStreetAddressValidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not HomeDto home)
            {
                return new ValidationResult("Invalid home data.");
            }

            // if ((forecast.Summary == "Hot" && forecast.TemperatureF < 90) ||
            //     (forecast.Summary == "Cold" && forecast.TemperatureF > 30))
            // {
            //     return new ValidationResult("The temperature does not match the summary description.");
            // }
            if (!home.StreetAddress.Any(c => char.IsDigit(c)) || home.StreetAddress.Length > 64)
            {
                return new ValidationResult("Street Address must contain a number and have fewer than 64 characters");
            }

            return ValidationResult.Success;
        }
    }
}
