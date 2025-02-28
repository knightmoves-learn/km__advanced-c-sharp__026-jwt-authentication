using HomeEnergyApi.Dtos;
using HomeEnergyApi.Validations;
using System.ComponentModel.DataAnnotations;

public class HomeValidationTests
{
    private HomeDto targetHomeDto;
    public HomeValidationTests()
    {
        targetHomeDto = new HomeDto();
        targetHomeDto.OwnerLastName = "test";
    }

    [Fact]
    public void HomeDtoShouldBeInvalidWhenAppliedToObjectsOtherThanHomeDto()
    {
        var homeValidAttribute = new HomeStreetAddressValidAttribute();
        var result = homeValidAttribute.GetValidationResult("not a HomeDto", new ValidationContext(targetHomeDto));

        Assert.True(result?.ErrorMessage != null,
            $"When provided with a 'string' rather than a 'HomeDto' , HomeStreetAddressValidAttribute did not return an invalid result");
        Assert.True(result?.ErrorMessage == "Invalid home data.",
            $"HomeStreetAddressValidAttribute did not return the correct error message on an invalid result\nExpected:Invalid home data.\nReceived:{result?.ErrorMessage}");
    }

    [Fact]
    public void HomeDtoShouldBeInvalidWhenStreetAddressDoesNotContainDigit()
    {
        targetHomeDto.StreetAddress = "one two three Test St.";
        var homeValidAttribute = new HomeStreetAddressValidAttribute();
        var result = homeValidAttribute.GetValidationResult(targetHomeDto, new ValidationContext(targetHomeDto));

        Assert.True(result?.ErrorMessage != null,
            $"When provided with StreetAddress : '{targetHomeDto.StreetAddress}' , HomeStreetAddressValidAttribute did not return an invalid result");
        Assert.True(result?.ErrorMessage == "Street Address must contain a number and have fewer than 64 characters",
            $"HomeStreetAddressValidAttribute did not return the correct error message on an invalid result\nExpected:Street Address must contain a number and have fewer than 64 characters\nReceived:{result?.ErrorMessage}");
    }

    [Fact]
    public void HomeDtoShouldBeInvalidWhenStreetAddressIsLongerThan64Characters()
    {
        targetHomeDto.StreetAddress = "123 Test St. 123 Test St. 123 Test St. 123 Test St. 123 Test St. 123 Test St. 123 Test St. 123 Test St. 123 Test St. ";
        var homeValidAttribute = new HomeStreetAddressValidAttribute();
        var result = homeValidAttribute.GetValidationResult(targetHomeDto, new ValidationContext(targetHomeDto));

        Assert.True(result?.ErrorMessage != null,
            $"When provided with StreetAddress : '{targetHomeDto.StreetAddress}' , HomeStreetAddressValidAttribute did not return an invalid result");
        Assert.True(result?.ErrorMessage == "Street Address must contain a number and have fewer than 64 characters",
            $"HomeStreetAddressValidAttribute did not return the correct error message on an invalid result\nExpected:Street Address must contain a number and have fewer than 64 characters\nReceived:{result?.ErrorMessage}");
    }

    [Fact]
    public void HomeDtoShouldBeInvalidWhenStreetAddressIsLongerThan64CharactersAndHasNoDigits()
    {
        targetHomeDto.StreetAddress = "one two three Test St one two three Test St one two three Test St one two three Test St one two three Test St one two three Test St ";
        var homeValidAttribute = new HomeStreetAddressValidAttribute();
        var result = homeValidAttribute.GetValidationResult(targetHomeDto, new ValidationContext(targetHomeDto));

        Assert.True(result?.ErrorMessage != null,
            $"When provided with StreetAddress : '{targetHomeDto.StreetAddress}' , HomeStreetAddressValidAttribute did not return an invalid result");
        Assert.True(result?.ErrorMessage == "Street Address must contain a number and have fewer than 64 characters",
            $"HomeStreetAddressValidAttribute did not return the correct error message on an invalid result\nExpected:Street Address must contain a number and have fewer than 64 characters\nReceived:{result?.ErrorMessage}");
    }

    [Fact]
    public void HomeDtoShouldBeValidWhenStreetAddressHasADigitAndFewerThan64Characters()
    {
        targetHomeDto.StreetAddress = "123 Test St.";
        var homeValidAttribute = new HomeStreetAddressValidAttribute();
        var result = homeValidAttribute.GetValidationResult(targetHomeDto, new ValidationContext(targetHomeDto));

        Assert.True(result == ValidationResult.Success,
            $"When provided with StreetAddress : '{targetHomeDto.StreetAddress}' , HomeStreetAddressValidAttribute did not return a valid result\nReceived Error Message: {result?.ErrorMessage}");
    }
}