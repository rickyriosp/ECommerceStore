using Polly;
using ShoppingCart.ProductCatalog;

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

// Error-Handling Policy with Polly
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
    // Wrap http calls made in ProductCatalogClient in a Polly policy
    .AddTransientHttpErrorPolicy(p => 
        // Uses Polly's fluent API to set up a retry policy with an exponential back-off
        p.WaitAndRetryAsync(
            3,
            attempt => TimeSpan.FromMilliseconds(100*Math.Pow(2, attempt))
        )
    );

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
