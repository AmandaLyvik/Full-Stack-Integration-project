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
- use a ProductService for centralising error handling
- improve UI Feedback for Errors on the `fetchproducts` page


# Activity 2

- Update API endpoint
- Update server CORS
- Update client ProductService to handle invalid JSON responses
    - use `JsonSerializerOptions` and `JsonSerializer` for more control
    - catch `JsonException` for more precise diagnostics
- Add logger to log CORS and json errors to browser console
    - We separated the logging logic in its own component for maintainability 

# Activity 3

- Update the API's return value and the shared Product model
- Add data annotation and recursive validation 

# Activity 4

- Identify and reduce redundant API calls in the front-end.
- Implement a session storage cache to minimize server load.
- Refactor the ProductService component 
