using LibraryWeb.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ϳ��������� Razor ����������
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ������ HttpClient (��� ������� API)
builder.Services.AddHttpClient();

// ϳ��������� ��������� ��, ���� �������
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� ����������
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
