using SomeStuff.Application.Mediator.HandleR;
using SomeStuff.Application.UseCases.BookClass;
using SomeStuff.Application.UseCases.CreateItem;
using SomeStuff.Application.UseCases.GetItems;
using SomeStuff.Domain.Repositories;
using SomeStuff.Filters;
using SomeStuff.Infrastructure.CircuitBreaker;
using SomeStuff.Infrastructure.RateLimiting;
using SomeStuff.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DI for Use Cases
builder.Services.AddScoped<IGetItemsUseCase, GetItemsUseCase>();
builder.Services.AddScoped<ICreateItemUseCase, CreateItemUseCase>();
builder.Services.AddScoped<IBookClassUseCase, BookClassUseCase>();

// DI for in-memory repositories and filters
builder.Services.AddSingleton<IClassRepository, InMemoryClassRepository>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<IBookingRateLimiter, InMemoryBookingRateLimiter>();
builder.Services.AddSingleton<ICircuitBreaker, PollyCircuitBreaker>();
builder.Services.AddScoped<BookingRateLimitFilter>();

// DI for MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// DI for HandleR
builder.Services.AddScoped<IHandleR, HandleR>();

var handlerTypes = typeof(Program).Assembly
    .GetTypes()
    .Where(t => t
        .GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
    );

foreach (var type in handlerTypes)
{
    foreach (var implementedInterface in type.GetInterfaces())
    {
        if (implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
        {
            builder.Services.AddScoped(implementedInterface, type);
        }
    }
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Seed in-memory class data
var classRepository = (InMemoryClassRepository)app.Services.GetRequiredService<IClassRepository>();
classRepository.Seed(new SomeStuff.Domain.Entities.ClassEntity
{
    Id = Guid.Parse("5a602076-f03c-4363-9e96-9334f4506054"),
    Title = "Yoga Basics",
    Capacity = 15
});
classRepository.Seed(new SomeStuff.Domain.Entities.ClassEntity
{
    Id = Guid.Parse("07fdb499-c0c1-4fb9-8fea-34111073eafa"),
    Title = "HIIT Training",
    Capacity = 25
});
classRepository.Seed(new SomeStuff.Domain.Entities.ClassEntity
{
    Id = Guid.Parse("2c018edf-de56-45a0-b0bf-b8244bd861db"),
    Title = "Pilates Intermediate",
    Capacity = 10
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
