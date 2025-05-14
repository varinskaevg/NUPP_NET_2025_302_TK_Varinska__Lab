using Library.Infrastructure.Data;
using Library.Infrastructure.Models;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

builder.Services.AddScoped<IRepository<BookModel>, Repository<BookModel>>();
builder.Services.AddScoped<ICrudServiceAsync<BookModel>, CrudServiceAsync<BookModel>>();

builder.Services.AddScoped<IRepository<LibraryMemberModel>, Repository<LibraryMemberModel>>();
builder.Services.AddScoped<ICrudServiceAsync<LibraryMemberModel>, CrudServiceAsync<LibraryMemberModel>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

    db.Database.Migrate();

    if (!db.LibraryMembers.Any())
    {
        var testMember = new LibraryMemberModel
        {
            FullName = "Тестовий Користувач",
            Email = "test@example.com"
        };
        db.LibraryMembers.Add(testMember);
        db.SaveChanges();

        var testBook = new BookModel
        {
            Title = "Тестова книга",
            Author = "Тест Автор",
            LibraryMemberId = testMember.Id,
            BookTags = new List<BookTagModel>()
        };
        db.Books.Add(testBook);
        db.SaveChanges();
    }
}

app.Run();
