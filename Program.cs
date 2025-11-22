var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<SomeStuff.Application.UseCases.GetItems.IGetItemsUseCase, SomeStuff.Application.UseCases.GetItems.GetItemsUseCase>();
builder.Services.AddScoped<SomeStuff.Application.UseCases.CreateItem.ICreateItemUseCase, SomeStuff.Application.UseCases.CreateItem.CreateItemUseCase>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
