using Library.Infrastructure.Data;
using Library.Infrastructure.Models;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Конфігурація БД
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// 🔹 Identity + ролі
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<LibraryDbContext>()
    .AddDefaultTokenProviders();

// 🔹 JWT Аутентифікація
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

// 🔹 Сервіси
builder.Services.AddScoped<IRepository<BookModel>, Repository<BookModel>>();
builder.Services.AddScoped<ICrudServiceAsync<BookModel>, CrudServiceAsync<BookModel>>();

builder.Services.AddScoped<IRepository<LibraryMemberModel>, Repository<LibraryMemberModel>>();
builder.Services.AddScoped<ICrudServiceAsync<LibraryMemberModel>, CrudServiceAsync<LibraryMemberModel>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await InitializeDatabaseAsync(app.Services);

app.Run();

async Task InitializeDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var scopedServices = scope.ServiceProvider;

    var db = scopedServices.GetRequiredService<LibraryDbContext>();
    await db.Database.MigrateAsync();

    var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "User", "Librarian", "Admin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
    string adminEmail = "admin@example.com";
    string adminPass = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Адміністратор"
        };

        var result = await userManager.CreateAsync(adminUser, adminPass);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    if (!db.LibraryMembers.Any())
    {
        var testMember = new LibraryMemberModel
        {
            FullName = "Тестовий Користувач",
            Email = "test@example.com"
        };
        db.LibraryMembers.Add(testMember);
        await db.SaveChangesAsync();

        var testBook = new BookModel
        {
            Title = "Тестова книга",
            Author = "Тест Автор",
            LibraryMemberId = testMember.Id,
            BookTags = new List<BookTagModel>()
        };
        db.Books.Add(testBook);
        await db.SaveChangesAsync();
    }
}


app.Run();
