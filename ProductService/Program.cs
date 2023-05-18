using Microsoft.EntityFrameworkCore;
using ProductService.Context;
using ProductService.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SouthWindDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["ConnectionStrings:SouthWindConStr"]);
                options.LogTo(Console.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
            });

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<SouthWindDbContext>()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGet("/", () => "For GraphQL server -> http://localhost:5124/graphql");

app.MapGraphQL();
app.Run();