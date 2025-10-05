using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Session Handling
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

// Add MemoryCache and CacheService
builder.Services.AddSingleton<CacheService>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
    // Test unreachable client origin
    options.AddPolicy("AllowUnreachableClient", policy =>
    {
        policy.WithOrigins("http://localhost:9999") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// middleware pipeline
app.UseRouting(); 
app.UseCors("AllowClient");
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});


app.Run();
