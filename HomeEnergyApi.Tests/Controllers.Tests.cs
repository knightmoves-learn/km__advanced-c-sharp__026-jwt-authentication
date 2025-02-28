using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using HomeEnergyApi.Dtos;


[TestCaseOrderer("HomeEnergyApi.Tests.Extensions.PriorityOrderer", "HomeEnergyApi.Tests")]
public class ControllersTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private HomeDto testHomeDto = new();
    private UtilityProviderDto testUtilityProviderDto = new();
    public ControllersTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory, TestPriority(1)]
    [InlineData("/Homes")]
    public async Task HomeEnergyApiReturnsSuccessfulHTTPResponseCodeOnGETHomes(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        Assert.True(response.IsSuccessStatusCode,
            $"HomeEnergyApi did not return successful HTTP Response Code on GET request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");
    }

    [Theory, TestPriority(2)]
    [InlineData("admin/UtilityProviders/")]
    public async Task HomeEnergyApiCanPOSTAUtilityProviderGivenAValidUtilityProviderDto(string url)
    {
        var client = _factory.CreateClient();

        UtilityProviderDto postTestUtilityProviderDto = testUtilityProviderDto;
        postTestUtilityProviderDto.Name = "test energy";
        postTestUtilityProviderDto.ProvidedUtilities = new List<string>() { "electric", "natural gas" };

        string strPostTestUtilityProviderDto = JsonSerializer.Serialize(postTestUtilityProviderDto);

        HttpRequestMessage sendRequest = new HttpRequestMessage(HttpMethod.Post, url);
        sendRequest.Content = new StringContent(strPostTestUtilityProviderDto,
                                                Encoding.UTF8,
                                                "application/json");

        var response = await client.SendAsync(sendRequest);
        Assert.True((int)response.StatusCode == 201,
            $"HomeEnergyApi did not return \"201: Created\" HTTP Response Code on POST request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");

        string responseContent = await response.Content.ReadAsStringAsync();
        responseContent = responseContent.ToLower();

        bool nameMatch = responseContent.Contains("\"name\":\"test energy\"");
        bool provUtilitiesMatch = responseContent.Contains("\"providedutilities\":[\"electric\",\"natural gas\"]");

        bool hasExpected = nameMatch && provUtilitiesMatch;

        Assert.True(hasExpected,
            $"Home Energy Api did not return the correct UtilityProvider being created on POST at {url}\nHomeDto Sent: {strPostTestUtilityProviderDto}\nHome Received:{responseContent}");

    }

    [Theory, TestPriority(3)]
    [InlineData("/Authentication/token")]
    public async Task HomeEnergyApiCanProvideABearerToken(string url)
    {
        var client = _factory.CreateClient();
        HttpRequestMessage sendRequest = new HttpRequestMessage(HttpMethod.Post, url);

        var response = await client.SendAsync(sendRequest);
        var responseStr = await response.Content.ReadAsStringAsync();

        Assert.True((int)response.StatusCode == 200,
            $"HomeEnergyApi did not return \"200: Ok\" HTTP Response Code on POST request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");

        bool validToken = responseStr.Length == 292 && responseStr.Contains("{\"token\":\"");

        // if (validToken)
        // {
        //     jwtBearerToken = responseStr.Trim(new char[] { '{', '}', '"' }).Substring(8);
        // }

        Assert.True(validToken,
            $"The provided bearer token was not in a valid format\nReceived token : {responseStr}");
    }

    [Theory, TestPriority(4)]
    [InlineData("/admin/Homes")]
    public async Task HomeEnergyApiCanPOSTAHomeGivenAValidHomeDto(string url)
    {
        string jwtBearerToken = await GetBearerToken();

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtBearerToken);

        HomeDto postTestHomeDto = testHomeDto;
        postTestHomeDto.OwnerLastName = "Test";
        postTestHomeDto.StreetAddress = "123 Test St.";
        postTestHomeDto.City = "Test City";
        postTestHomeDto.MonthlyElectricUsage = 123;

        string strPostTestHomeDto = JsonSerializer.Serialize(postTestHomeDto);
        HttpRequestMessage sendRequest = new HttpRequestMessage(HttpMethod.Post, url);

        sendRequest.Content = new StringContent(strPostTestHomeDto,
                                                Encoding.UTF8,
                                                "application/json");

        var response = await client.SendAsync(sendRequest);
        Assert.True((int)response.StatusCode == 201,
            $"HomeEnergyApi did not return \"201: Created\" HTTP Response Code on POST request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");

        string responseContent = await response.Content.ReadAsStringAsync();
        responseContent = responseContent.ToLower();

        bool ownerLastMatch = responseContent.Contains("\"ownerlastname\":\"test\"");
        bool streetAddMatch = responseContent.Contains("\"streetaddress\":\"123 test st.\"");
        bool cityMatch = responseContent.Contains("\"city\":\"test city\"");

        string homeUsageResponse = responseContent.Substring(responseContent.IndexOf("homeusagedata"), responseContent.IndexOf("homeutilityproviders") - responseContent.IndexOf("homeusagedata"));
        bool monthlyUsageMatch = homeUsageResponse.Contains("\"monthlyelectricusage\":123,");

        string expectedHomeId = responseContent[6..responseContent.IndexOf(",")];
        bool homeIdMatch = responseContent.Contains($"\"homeid\":{expectedHomeId}");

        bool hasExpectedHomeData = ownerLastMatch && streetAddMatch && cityMatch && monthlyUsageMatch;

        Assert.True(hasExpectedHomeData,
            $"Home Energy Api did not return the correct Home being created on POST at {url}\nHomeDto Sent: {strPostTestHomeDto}\nHome Received:{responseContent}");

        Assert.True(homeIdMatch,
            $"For the Home created on POST at {url}, the home's id did not match the id within the Home Utility Providers property\nExpected Home Id: {expectedHomeId}\nHome Received:{responseContent}");
    }

    [Theory, TestPriority(5)]
    [InlineData("/admin/Homes")]
    public async Task HomeEnergyApiCanPUTAHomeGivenAValidHomeDto(string url)
    {
        var client = _factory.CreateClient();

        var getAllResponse = await client.GetAsync("/Homes");
        string getAllResponseStr = await getAllResponse.Content.ReadAsStringAsync();
        dynamic? getAllResponseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(getAllResponseStr);
        string urlId = getAllResponseObj?[getAllResponseObj.Count - 1].Id ?? "";
        url = url + $"/{urlId}";

        HomeDto putTestHomeDto = testHomeDto;
        putTestHomeDto.OwnerLastName = "Putty";
        putTestHomeDto.StreetAddress = "123 Put St.";
        putTestHomeDto.City = "Put City";
        putTestHomeDto.MonthlyElectricUsage = 456;

        string strPutTestHomeDto = JsonSerializer.Serialize(putTestHomeDto);

        HttpRequestMessage sendRequest = new HttpRequestMessage(HttpMethod.Put, url);
        sendRequest.Content = new StringContent(strPutTestHomeDto,
                                                Encoding.UTF8,
                                                "application/json");

        var response = await client.SendAsync(sendRequest);
        Assert.True((int)response.StatusCode == 200,
            $"HomeEnergyApi did not return \"200: Ok\" HTTP Response Code on PUT request at {url}; instead received {(int)response.StatusCode}: {response.StatusCode}");

        string responseContent = await response.Content.ReadAsStringAsync();
        responseContent = responseContent.ToLower();

        bool ownerLastMatch = responseContent.Contains("\"ownerlastname\":\"putty\"");
        bool streetAddMatch = responseContent.Contains("\"streetaddress\":\"123 put st.\"");
        bool cityMatch = responseContent.Contains("\"city\":\"put city\"");

        string homeUsageResponse = responseContent.Substring(responseContent.IndexOf("homeusagedata"));
        bool monthlyUsageMatch = homeUsageResponse.Contains("\"monthlyelectricusage\":456,");

        bool hasExpected = ownerLastMatch && streetAddMatch && cityMatch && monthlyUsageMatch;

        Assert.True(hasExpected,
            $"Home Energy Api did not return the correct Home being updated on PUT at {url}\nHomeDto Sent: {strPutTestHomeDto}\nHome Received:{responseContent}");
    }

    [Theory, TestPriority(6)]
    [InlineData("/Homes/Bang")]
    public async Task HomeEnergyApiAppliesGlobalExceptionFilter(string url)
    {
        var client = _factory.CreateClient();

        var bangResponse = await client.GetAsync(url);
        string bangResponseStr = await bangResponse.Content.ReadAsStringAsync();
        string expected = "{\"message\":\"An unexpected error occurred.\",\"error\":\"You caused a loud bang.\"}";

        Assert.True((int)bangResponse.StatusCode == 500,
            $"HomeEnergyApi did not return '500: Internal Server Error' HTTP Response Code on GET request at {url}; instead received {(int)bangResponse.StatusCode}: {bangResponse.StatusCode}");

        Assert.True(bangResponseStr == expected,
            $"HomeEnergyApi did not return the expected result on GET request at {url}\nExpected:{expected}\nReceived:{bangResponseStr}");
    }

    public string BuildBase64EncodedAuthString(string username, string password)
    {
        var authenticationString = $"{username}:{password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

        return base64EncodedAuthenticationString;
    }

    public async Task<string> GetBearerToken()
    {
        var client = _factory.CreateClient();
        HttpRequestMessage sendRequest = new HttpRequestMessage(HttpMethod.Post, "/Authentication/token");

        var response = await client.SendAsync(sendRequest);
        var responseStr = await response.Content.ReadAsStringAsync();

        return responseStr.Trim(new char[] { '{', '}', '"' }).Substring(8);
    }
}
