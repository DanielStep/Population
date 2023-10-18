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
    public async Task AgeStructure_GivenRegionCodeIsASGSType_ShouldReturnSA4SpecificPopulationData()
    {
        _factPopulationId = Guid.NewGuid();
        _dataSeeder.Seed(_factPopulationId);

        var response = await _client.GetAsync($"/api/age-structure/9999/1");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(stringResponse);
        var resultData = parsed["data"].ToObject<List<SA4PopulationDTO>>();
        var result = resultData.Single();

        parsed.ToObject<SA4PopulationQueryResultDTO>().regionCode.Should().Be("9999");
        result.population.Should().Be(999);
        result.censusYear.Should().Be(2011);
        result.age.Should().Be("10");
        result.sex.Should().Be("Males");
        result.region.Should().BeNull();
    }

    [Test]
    public async Task AgeStructure_GivenNoDataPresent_ShouldReturnEmpty()
    {
        var response = await _client.GetAsync($"/api/age-structure/9999/3");
        var stringResult = await response.Content.ReadAsStringAsync();
        var result = JObject.Parse(stringResult);
        var dataResult = result["data"].ToObject<List<SA4PopulationDTO>>();
        dataResult.Count.Should().Be(0);
    }

    [Test]
    public async Task AgeDiffStructure_GivenRegionCodeIsStateType_ShouldReturnDataOverRegions()
    {
        var response = await _client.GetAsync($"/api/age-structure-diff/1/1/2011/2016");
        var stringResponse = await response.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(stringResponse);
        var dataResult = parsed["data"].ToObject<List<SA4PopulationDiffDTO>>();

        var queryResult = parsed.ToObject<SA4PopulationDiffResultDTO>();
        queryResult.censusYear.Should().Be("2011-2016");
        queryResult.regionCode.Should().Be("1");
        queryResult.regionName.Should().Be("New South Wales");
        var regionResult = dataResult.Select(x => x.region);
        regionResult.Should().NotContainNulls();
    }

}