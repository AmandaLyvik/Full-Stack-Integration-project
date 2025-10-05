# 🧩 Blazor Product Catalog

A full-stack Blazor WebAssembly + ASP.NET Core Web API application with a shared class library for models and contracts. This project demonstrates clean architecture, robust validation, client/server caching, and modular service design — built for maintainability, performance, and clarity.

---

## 🚀 Features

- **Shared Class Library**: Strongly typed models shared between client and server.
- **Modular Client Services**: Includes `ProductService`, `JsonProductParser`, `RecursiveValidator`, and `SessionCacheService`.
- **Recursive Validation**: Validates deeply nested models using `DataAnnotations`.
- **Client-Side Caching**: Uses `Blazored.SessionStorage` to reduce redundant API calls.
- **Server-Side Caching**: Uses `IMemoryCache` to avoid repeated database/API hits.
- **Console Logging**: Logs validation and network errors to the browser console.
- **CORS Configuration**: Enables secure cross-origin communication between client and server.

---

## 🛠️ Tech Stack

- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Shared Models**: .NET Class Library
- **Caching**: Blazored.SessionStorage (client), IMemoryCache (server)
- **Validation**: System.ComponentModel.DataAnnotations
- **Serialization**: System.Text.Json
- **Logging**: Custom ConsoleLogger

