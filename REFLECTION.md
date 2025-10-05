# Overview

This document reflects on how Copilot assisted in building and refining this full-stack Blazor application. The project involved integrating client-server communication, handling JSON responses, implementing validation, and optimizing performance through caching. Copilot played a key role in guiding architectural decisions, debugging edge cases, and modularizing logic for maintainability.

# 🤖 How Copilot Helped
## 🔌 Integration Code

- Client–Server Communication: Copilot helped scaffold the initial API calls in `FetchProducts.razor`, ensuring correct use of `HttpClient` and `GetFromJsonAsync`.

- CORS Configuration: It clarified how to configure CORS policies in `Program.cs` to allow cross-origin requests between the Blazor WebAssembly client and the ASP.NET Core server.

- Shared Models: Copilot emphasized the importance of using a shared Product class to avoid serialization mismatches and improve type safety.

## 🐞 Debugging Issues

- Invalid JSON Handling: When the server returned malformed data, Copilot guided the use of `JsonSerializerOptions` and `JsonException` to catch and log deserialization errors.

- Validation Failures: Copilot helped implement recursive validation using DataAnnotations, and later refactored it into a reusable `RecursiveValidator<T>` service.

- DI Errors: When services like ISessionStorageService or `IJsonParser<Product[]>` failed to resolve, Copilot identified missing `using` directives and guided proper DI registration.

## 📦 Structuring JSON Responses

- Copilot encouraged separating concerns by extracting JSON parsing into a dedicated `JsonProductParser`, improving readability and testability.

- It helped define interfaces like `IJsonParser<T>` and `IValidator<T>` to make the system extensible for other models beyond Product.

## 🚀 Performance Optimization

- Client-Side Caching: Copilot introduced Blazored.SessionStorage and helped abstract it into a generic ISessionCacheService with expiration support.

- Server-Side Caching: It guided the use of IMemoryCache and a CacheService to reduce redundant database/API calls.


# 🧠 Lessons Learned About Using Copilot

- Ask precisely, iterate collaboratively: The more context I gave, the more targeted Copilot’s suggestions became, especially when troubleshooting DI or refactoring abstractions.

- Modularize early: Copilot encouraged me to extract parsing, validation, and caching logic into services, which made testing and reuse much easier.

- Balance automation with ownership: Copilot accelerated development, but I stayed in control — reviewing suggestions, adapting them to my domain, and evolving the design.


# ✅ Summary of Activities

## Activity 1

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


## Activity 2

- Update API endpoint
- Update server CORS
- Update client ProductService to handle invalid JSON responses
    - use `JsonSerializerOptions` and `JsonSerializer` for more control
    - catch `JsonException` for more precise diagnostics
- Add logger to log CORS and json errors to browser console
    - We separated the logging logic in its own component for maintainability 

## Activity 3

- Update the API's return value and the shared Product model
- Add data annotation and recursive validation 

## Activity 4

- Identify and reduce redundant API calls in the front-end.
- Implement a session storage cache to minimize server load.
- Refactor the ProductService component 
