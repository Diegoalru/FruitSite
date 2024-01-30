using FruitAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FruitDb>(opt => opt.UseInMemoryDatabase("FruitList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Fruit API",
        Description = "API for managing a list of fruit and their stock status."
    });
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<FruitDb>();
    dbContext.Database.EnsureCreated();
}

app.MapGet("/fruitlist", async (FruitDb db) =>
        await db.Fruits.ToListAsync())
    .WithTags("Get all fruit");

app.MapGet("/fruitlist/InStock", async (FruitDb db) =>
        await db.Fruits.Where(t => t.InStock).ToListAsync())
    .WithTags("Get all fruit that is in stock");

app.MapGet("/fruitlist/{id:int}", async (int id, FruitDb db) =>
        await db.Fruits.FindAsync(id)
            is { } fruit
            ? Results.Ok(fruit)
            : Results.NotFound())
    .WithTags("Get fruit by Id");

app.MapPost("/fruitlist", async (Fruit fruit, FruitDb db) =>
    {
        db.Fruits.Add(fruit);
        await db.SaveChangesAsync();

        return Results.Created($"/fruitlist/{fruit.Id}", fruit);
    })
    .WithTags("Add fruit to list");

app.MapPut("/fruitlist/{id:int}", async (int id, Fruit inputFruit, FruitDb db) =>
    {
        var fruit = await db.Fruits.FindAsync(id);

        if (fruit is null) return Results.NotFound();

        fruit.Name = inputFruit.Name;
        fruit.InStock = inputFruit.InStock;

        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithTags("Update fruit by Id");

app.MapDelete("/fruitlist/{id:int}", async (int id, FruitDb db) =>
    {
        if (await db.Fruits.FindAsync(id) is not { } fruit)
            return Results.NotFound();

        db.Fruits.Remove(fruit);
        await db.SaveChangesAsync();
        return Results.Ok(fruit);
    })
    .WithTags("Delete fruit by Id");


app.UseSwagger();
app.UseSwaggerUI();

app.Run();