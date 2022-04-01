using ShoppingCart.ShoppingCart;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add custom services using Scrutor
builder.Services.Scan(selector => selector.FromAssemblyOf<Program>()    // Scan the assemblies containing the provided Type
                                          .AddClasses()                 // Add all public, non-abstract classes
                                          .AsMatchingInterface()        // Registering an implementation using forwarded services
                                          .WithScopedLifetime());       // Use the same service scoped to the lifetime of a request

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
