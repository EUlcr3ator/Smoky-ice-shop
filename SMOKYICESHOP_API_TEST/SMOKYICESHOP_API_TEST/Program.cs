using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST;
using SMOKYICESHOP_API_TEST.DbContexts;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });

builder.Build().Run();
