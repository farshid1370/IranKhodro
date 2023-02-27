using IranKhodro;
using Redis.OM;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        var configuration = hostContext.Configuration;
        services.Configure<Settings>(configuration);
        services.AddHostedService<IndexCreationService>();
        services.AddSingleton(new RedisConnectionProvider(configuration["ConnectionString"]));
        services.AddSingleton<ICircularManager, CircularManager>();
        services.AddHostedService<Worker>();
    })
    .Build();


await host.RunAsync();
