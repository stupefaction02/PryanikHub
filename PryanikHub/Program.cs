using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PryanikHub;
using PryanikHub.Entities;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

ConfigureMSSQL(services, configuration);

ConfigureRepositories(services, configuration);

void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
{
    services.AddScoped<IOrdersRepository, OrdersRepository>();
}

WebApplication app = builder.Build();

app.MapGet("/pryaniks", ([FromServices] IOrdersRepository orders, int s = 0, int c = 0) =>
{
    if (s == 0 && c == 0)
    {
        return orders.OrdersFullInclude.ToList();
    }

    return orders.GetInterval(s, c).ToList();
});

app.MapPost("/pryaniks", async ([FromBody] Order[] orders, [FromServices] IOrdersRepository ordersRep, HttpContext context) =>
{
    if (orders != null)
    {
        List<Order> createdOrders = new List<Order>();
        foreach (Order order in orders)
        {
            order.Uid = Guid.NewGuid().ToString();
            order.CreatedAt = DateTime.Now;

            createdOrders.Add(order);
        }

        await ordersRep.AddRangeAsync(createdOrders);

        context.Response.StatusCode = 200;
        return 1;
    }

    context.Response.StatusCode = 400;
    return 0;
});

app.MapDelete("/pryaniks", async ([FromBody] IdsJsonModel json, [FromServices] IOrdersRepository orders, HttpContext context) =>
{
    if (json != null)
    {
        int[] ids = json.Ids;
        if (ids.Length >= 1)
        {
            if (ids.Length == 1)
            {
                await orders.RemoveByIdAsync(ids[0]);
            } 
            else
            {
                await orders.RemoveByIdsAsync(ids);
            }
        }

        await orders.SaveChangesAsync();

        context.Response.StatusCode = 200;
        return 1;
    }

    context.Response.StatusCode = 400;
    return 0;
});

app.Run();

void ConfigureMSSQL(IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<CoreDbContext>(options => {
        Console.WriteLine($"Seting up connection string for {nameof(CoreDbContext)}");

        string? connectionString = configuration["Database:MSSQL:CoreConnectionString"];

        if (String.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Value of Database:MSSQL:CoreConnectionString is no set in configuration file or not found.");
        }

        options.UseSqlServer(connectionString, x => {
            x.EnableRetryOnFailure(1, TimeSpan.FromSeconds(10), null);
        });

        //options.EnableSensitiveDataLogging();
        //options.LogTo(x => { });
        //options.EnableDetailedErrors(true);
        //options.EnableSensitiveDataLogging(true);

    }, ServiceLifetime.Transient);
}

public class IdsJsonModel
{
    [JsonPropertyName("ids")]
    public int[] Ids { get; set; }
}