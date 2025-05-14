using LibraryWeb.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Підключення Razor компонентів
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Додати HttpClient (для виклику API)
builder.Services.AddHttpClient();

// Підключення контексту БД, якщо потрібно
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Запускаємо застосунок
var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
