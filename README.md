# Activity 1

## FetchProducts.razor API logic

Client side 

```
// FetchProducts.razor

@inject HttpClient Http

protected override async Task OnInitializedAsync()
{
    products = await Http.GetFromJsonAsync<Product[]>("/api/products");
}

// Program.cs
BaseAddress = new Uri("http://localhost:5086")

```

Server Side

```
// Program.cs

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("http://localhost:5235") // your client URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

```

best practices for readability and maintainability
- use shared Product class
- use ProductService for error handling


# Activity 2

- Update API endpoint
- Update server CORS
- Update client ProductService to handle invalid JSON responses