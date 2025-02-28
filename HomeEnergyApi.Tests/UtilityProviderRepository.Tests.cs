using HomeEnergyApi.Models;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

[TestCaseOrderer("HomeEnergyApi.Tests.Extensions.PriorityOrderer", "HomeEnergyApi.Tests")]
public class UtilityProviderRepositoryTests
{
    private UtilityProvider testUtilityProviderToSave = new();
    private UtilityProvider testUtilityProviderToUpdate = new();

    private List<UtilityProvider> testUtilityProviders = new List<UtilityProvider>()
        {
            new UtilityProvider(),
            new UtilityProvider()
        };

    private readonly DbConnection _connection;
    private readonly DbContextOptions<HomeDbContext> _contextOptions;

    public UtilityProviderRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<HomeDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new HomeDbContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT *
FROM UtilityProviders;";
            viewCommand.ExecuteNonQuery();
        }

        testUtilityProviders[0].Name = "test comp";
        testUtilityProviders[1].Name = "assert comp";

        context.AddRange(testUtilityProviders);
        context.SaveChanges();
    }

    HomeDbContext CreateContext() => new HomeDbContext(_contextOptions);

    [Fact, TestPriority(1)]
    public void UtilityProviderRepositoryCanFindAllHomes()
    {
        using var context = CreateContext();
        var utilityProviderRepository = new UtilityProviderRepository(context);

        var utilityProviders = utilityProviderRepository.FindAll();

        Assert.Collection(
            utilityProviders,
            u => Assert.True(u.Name == testUtilityProviders[0].Name),
            u => Assert.True(u.Name == testUtilityProviders[1].Name));
    }

    [Fact, TestPriority(2)]
    public void UtilityProviderRepositoryCanFindHomeById()
    {
        using var context = CreateContext();
        var utilityProviderRepository = new UtilityProviderRepository(context);

        var utilityProvider = utilityProviderRepository.FindById(1);

        Assert.True(utilityProvider.Name == testUtilityProviders[0].Name);
    }

    [Fact, TestPriority(3)]
    public void UtilityProviderRepositoryCanSaveANewHome()
    {
        using var context = CreateContext();
        var utilityProviderRepository = new UtilityProviderRepository(context);

        testUtilityProviderToSave.Name = "save prov";
        utilityProviderRepository.Save(testUtilityProviderToSave);
        var utilProv = utilityProviderRepository.FindById(3);

        Assert.True(utilProv.Name == testUtilityProviderToSave.Name);
    }

    [Fact, TestPriority(4)]
    public void UtilityProviderRepositoryCanUpdateAHome()
    {
        using var context = CreateContext();
        var utilityProviderRepository = new UtilityProviderRepository(context);

        testUtilityProviderToSave.Name = "I'm updated";
        utilityProviderRepository.Update(1, testUtilityProviderToUpdate);
        var utilProv = utilityProviderRepository.FindById(1);

        Assert.True(utilProv.Name == testUtilityProviderToUpdate.Name);
    }

    [Fact, TestPriority(5)]
    public void UtilityProviderRepositoryCanRemoveById()
    {
        using var context = CreateContext();
        var utilityProviderRepository = new UtilityProviderRepository(context);

        utilityProviderRepository.RemoveById(2);
        var utilProv = utilityProviderRepository.FindById(2);

        Assert.True(utilProv == null);
    }

    [Fact, TestPriority(6)]
    public void UtilityProviderRepositoryCanReturnCount()
    {
        using var context = CreateContext();
        var utilityProviderRepository = new UtilityProviderRepository(context);

        var count = utilityProviderRepository.Count();

        Assert.True(count == 2);
    }

    [Fact, TestPriority(7)]
    public void UtilityProviderRepositoryImplementsIReadRepositoryINTHOME()
    {
        Assert.True(typeof(IReadRepository<int, UtilityProvider>).IsAssignableFrom(typeof(UtilityProviderRepository)),
            "The class UtilityProviderRepository Does Not Implement \"IRepository<int, UtilityProvider>\"");
    }

    [Fact, TestPriority(8)]
    public void UtilityProviderRepositoryImplementsIWriteRepositoryINTHOME()
    {
        Assert.True(typeof(IWriteRepository<int, UtilityProvider>).IsAssignableFrom(typeof(UtilityProviderRepository)),
            "The class UtilityProviderRepository Does Not Implement \"IRepository<int, UtilityProvider>\"");
    }
}
