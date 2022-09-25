using Demo.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IProductCache, ProductCache>();

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(5);
});



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseOutputCache();

app.MapControllers();

app.Run();
