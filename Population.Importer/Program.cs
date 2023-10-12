using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Population.Application;

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var configuration = configBuilder.Build();


var serviceCollection = new ServiceCollection()
    .AddDbContext<PopulationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DbConnection")))
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.RegisterServicesFromAssembly(typeof(IngestPopulationDataCmd).Assembly);
    })
    .AddScoped<IRepository, Repository>()
    .AddScoped<IParser, CsvParser>()
    .AddSingleton<IConfiguration>(configuration)
    .BuildServiceProvider();

var mediator = serviceCollection.GetRequiredService<IMediator>();
await mediator.Send(new IngestPopulationDataCmd { CsvPath = args[0] });