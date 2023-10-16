using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Population.Api.Controllers;
using Population.Application.Queries;

public class TestPopulationApplicationFactory : WebApplicationFactory<Program>
{
    public IConfiguration Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureServices(services =>
        {
        });
    }
}