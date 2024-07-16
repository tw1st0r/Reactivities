using System.Data.Common;
using API.Middlewares;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<MigrationMiddleware>();
builder.Services.AddScoped<TimingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseTiming(); // from TimingExtensions instead of calling explicitly 'app.UseMiddleware<TimingMiddleware>()'
//app.UseMiddleware<TimingMiddleware>();
// app.Use(async (context, next) => {
//     var startTime = DateTime.UtcNow;
//     await next.Invoke(context);
//     app.Logger.LogInformation($"Request {context.Request.Path}: {DateTime.UtcNow - startTime}");
// });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// using var scope = app.Services.CreateScope();
// try
// {
//     var context = scope.ServiceProvider.GetRequiredService<DataContext>();
//     await context.Database.MigrateAsync();
//     await Seed.SeedData(context);
// }
// catch (Exception e)
// {
//     app.Logger.LogError(e, "An error occurred during migration");
//     // var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//     // logger.LogError(e, "An error occurred during migration");
// }

app.UseMiddleware<MigrationMiddleware>();

app.Run();
