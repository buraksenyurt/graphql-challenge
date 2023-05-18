using CommentService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/ping", () => "Pong").ExcludeFromDescription();

app.MapGet("api/user/comments/{productId}", (
    [FromRoute] string productId) =>
        {
            string[] dummy_contents = new[] {
                "Eh işte çok da beğenmedim.",
                "Bence biraz daha kaliteli bir malzeme kullanılabilirmiş.",
                "Çok kötü.",
                "Üründen memnun kaldım sayılır.",
                "Harika. Çok teşekkürler. Çok çok memnun kaldım.",
                "Güzel ürün, kargo zamanında geldi, hasarsız geldi.",
                "Bu nası bişidir yeaa! :D"
            };

            return Enumerable.Range(1, 4).Select(index => new UserComment
            {
                Date = DateTime.Now.AddDays(-index),
                Star = Random.Shared.Next(1, 10),
                Content = dummy_contents[Random.Shared.Next(dummy_contents.Length)]
            }).ToArray();
        })
    .WithName("GetUserComments")
    .WithOpenApi(operation =>
    {
        operation.Description = "Ürüne ait kullanıcı yorumları";
        return operation;
    })
    .Produces<List<UserComment>>(StatusCodes.Status200OK);

app.Run();

