using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using ProductService.Cache;
using ProductService.Context;
using ProductService.Contracts;
using ProductService.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpClient(name: "UserCommentService",
configureClient: options =>
{
    options.BaseAddress = new("http://localhost:5245/");
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json", 1.0)
    );
});

builder.Services.AddScoped<ICacheService, PhotoCacheService>();
builder.Services.AddDbContext<SouthWindDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["ConnectionStrings:SouthWindConStr"]);
                options.LogTo(Console.WriteLine, [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
            });

builder.Services
    .AddGraphQLServer()
    .RegisterDbContextFactory<SouthWindDbContext>()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGet("/", () => "For GraphQL server -> http://localhost:5124/graphql");

app.MapGraphQL();
app.Run();