using HomeEnergyApi.Services;

[TestCaseOrderer("HomeEnergyApi.Tests.Extensions.PriorityOrderer", "HomeEnergyApi.Tests")]
public class ZipLocationServiceTests
{
    [Fact, TestPriority(1)]
    public async Task ZipLocationServiceReportCanReturnPlaceGivenValidZipCode()
    {
        HttpClient client = new HttpClient();
        
        var zipLocationService = new ZipCodeLocationService(client);
        var place = await zipLocationService.Report(50313);
        Assert.NotNull(place);
    }
}