public class FileTests
{
    private static string programFilePath = @"../../../../HomeEnergyApi/Program.cs";
    private static string homeDtoFilePath = @"../../../../HomeEnergyApi/Dtos/HomeDto.cs";
    private string programContent = File.ReadAllText(programFilePath);
    private string homeDtoContent = File.ReadAllText(homeDtoFilePath);

    [Fact]
    public void DoesProgramFileAddScopedServiceHomeRepository()
    {
        bool containsHomeRepositoryScoped = programContent.Contains("builder.Services.AddScoped<HomeRepository>();");
        Assert.True(containsHomeRepositoryScoped,
            "HomeEnergyApi/Program.cs does not add a Scoped Service of type `HomeRepository`");
    }

    [Fact]
    public void DoesProgramFileAddScopedServiceIReadRepositoryWithRequiredServiceProviderHomeRepository()
    {
        bool containsIReadScoped = programContent.Contains("builder.Services.AddScoped<IReadRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());");
        Assert.True(containsIReadScoped,
            "HomeEnergyApi/Program.cs does not add a Scoped Service of type `IReadRepository` with the required Service Provider of type `HomeRepository`");
    }

    [Fact]
    public void DoesProgramFileAddScopedServiceIWriteRepositoryWithRequiredServiceHomeProviderRepository()
    {
        bool containsIWriteScoped = programContent.Contains("builder.Services.AddScoped<IWriteRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());");
        Assert.True(containsIWriteScoped,
            "HomeEnergyApi/Program.cs does not add a Scoped Service of type `IWriteRepository` with the required Service Provider of type `HomeRepository`");
    }

    [Fact]
    public void DoesProgramFileAddTransientForZipLocationService()
    {
        bool containsTransient = programContent.Contains("builder.Services.AddTransient<ZipCodeLocationService>();");
        Assert.True(containsTransient,
            "HomeEnergyApi/Program.cs does not add a Transient Service of type `ZipLocationService`");
    }

    [Fact]
    public void DoesProgramFileAddHttpClientForZipLocationService()
    {
        bool containsHttpClient = programContent.Contains("builder.Services.AddHttpClient<ZipCodeLocationService>();");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not add a HttpClient Service of type `ZipLocationService`");
    }

    [Fact]
    public void DoesProgramFileAddDBContextService()
    {
        bool containsHttpClient = programContent.Contains("builder.Services.AddDbContext<HomeDbContext>");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not add a DbContext Service with type HomeDBContext");
    }

    [Fact]
    public void DoesProgramFileDBContextServiceHaveUseSQLITEOption()
    {
        bool containsHttpClient = programContent.Contains("options.UseSqlite");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not add a DbContext Service that uses the option UseSqlite");
    }

    [Fact]
    public void DoesProgramFileGetRequiredServiceOfTypeHomeDbContext()
    {
        bool containsHttpClient = programContent.Contains(".ServiceProvider.GetRequiredService<HomeDbContext>();");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not get a required service of type HomeDbContext");
    }

    [Fact]
    public void DoesProgramFileCallDatabaseMigrate()
    {
        bool containsHttpClient = programContent.Contains(".Database.Migrate();");
        Assert.True(containsHttpClient,
            "HomeEnergyApi/Program.cs does not call Database.Migrate()");
    }

    [Fact]
    public void DoesProgramAddAutoMapperWithTypeHomeProfile()
    {
        bool containsAutoMapperHomeProfile = programContent.Contains("builder.Services.AddAutoMapper(typeof(HomeProfile));");
        Assert.True(containsAutoMapperHomeProfile,
            "HomeEnergyApi/Program.cs adds an Automapper service with type HomeProfile");
    }

    [Fact]
    public void DoesProgramAddScopedServicesUtilityProviderRepository()
    {
        bool containsUtilityProviderRepo1 = programContent.Contains("builder.Services.AddScoped<UtilityProviderRepository>();");
        Assert.True(containsUtilityProviderRepo1,
            "HomeEnergyApi/Program.cs does not add a scoped service for UtilityProviderRepository");
    }

    [Fact]
    public void DoesProgramAddScopedServicesHomeUtilityProviderRepository()
    {
        bool containsHomeUtilityProviderRepo1 = programContent.Contains("builder.Services.AddScoped<HomeUtilityProviderRepository>();");
        Assert.True(containsHomeUtilityProviderRepo1,
            "HomeEnergyApi/Program.cs does not add a scoped service for HomeUtilityProviderRepository");
    }

    [Fact]
    public void DoesProgramAddTransientHomeUtilityProviderService()
    {
        bool containsTransientHomeUtilityProviderService = programContent.Contains("builder.Services.AddTransient<HomeUtilityProviderService>();");
        Assert.True(containsTransientHomeUtilityProviderService,
            "HomeEnergyApi/Program.cs does not add a transient service with type HomeUtilityProviderService");
    }

    [Fact]
    public void DoesHomeDtoAddHomeStreetAddressValidation()
    {
        bool containsHomeStreetAddressValidation = homeDtoContent.Contains("[HomeStreetAddressValid]");
        Assert.True(containsHomeStreetAddressValidation,
            "HomeEnergyApi/Dtos/HomeDto.cs does not contain the attribute '[HomeStreetAddressValid]'");
    }

        [Fact]
    public void DoesProgramAddControllerOptionToAddGlobalExceptionFilter()
    {
        bool containsOptionAddingGlobalExceptionFilter = programContent.Contains("options.Filters.Add<GlobalExceptionFilter>();");
        Assert.True(containsOptionAddingGlobalExceptionFilter,
            "HomeEnergyApi/Program.cs does not add a controller option adding 'GlobalExceptionFilter' as a filter");
    }
}
