using Microsoft.EntityFrameworkCore;
using StateControlSystem.Entities;
using StateControlSystem.Repositories.Abstract;
using StateControlSystem.Repositories.Concrete;
using StateControlSystem.Services.Abstract;
using StateControlSystem.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// For Services 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injections
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<IInvoiceRepository, InvoiceRepository>();

// DataContext
builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("PgSql")));

// Caching
builder.Services.AddMemoryCache();

var app = builder.Build();

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();

var retryCount = 0;
var maxRetries = 5;
while (retryCount < maxRetries)
{
    try
    {
        context.Database.Migrate();
        break;
    }
    catch (Exception ex)
    {
        retryCount++;
        Console.WriteLine($"Migration failed. Retrying {retryCount} / {maxRetries}...");
        Thread.Sleep(5000);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
