using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Population.Test;

public class AgeStructureIntegrationTests
{
    private IServiceScope _scope;
    private PopulationDbContext _dbContext;
    private TestPopulationApplicationFactory _factory;
    private HttpClient _client;
    private DataSeeder _dataSeeder;
    private Guid _factPopulationId;

    [SetUp]
    public void Setup()
    {
        _factory = new TestPopulationApplicationFactory();
        _client = _factory.CreateClient();

        _scope = _factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PopulationDbContext>();

        _dataSeeder = new DataSeeder(_dbContext);
        _factPopulationId = Guid.NewGuid();
        _dataSeeder.Seed(_factPopulationId);

    }

    [TearDown]
    public void TearDown()
    {
        _dataSeeder.DeleteSeededData(_factPopulationId);
        _client.Dispose();
        _factory.Dispose();
        _dbContext.Dispose();
        _scope.Dispose();
    }


    [Test]
    public async Task GivenRegionCodeIsASGSType_ShouldReturnSA4SpecificPopulationData()
    {        
        var response = await _client.GetAsync($"/api/age-structure/9999/1");
        var stringResult = await response.Content.ReadAsStringAsync();
        var result = JObject.Parse(stringResult);
        var dataResult = result["data"].ToObject<List<SA4PopulationDTO>>();
        dataResult.Single().population.Should().Be(9999);
    }  
}