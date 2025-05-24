using Library.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Models;

namespace Library.Infrastructure.Data
{
    public class LibraryDbContext : IdentityDbContext<AppUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options) { }

        public DbSet<BookModel> Books { get; set; }
        public DbSet<AudioBookModel> AudioBooks { get; set; }
        public DbSet<EBookModel> EBooks { get; set; }
        public DbSet<LibraryMemberModel> LibraryMembers { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<BookTagModel> BookTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Наслідування (TPH або TPT)
            modelBuilder.Entity<BookModel>().ToTable("Books");
            modelBuilder.Entity<AudioBookModel>().ToTable("AudioBooks");
            modelBuilder.Entity<EBookModel>().ToTable("EBooks");

            modelBuilder.Entity<BookModel>()
                .HasOne(b => b.LibraryMember)
                .WithMany(l => l.BorrowedBooks)
                .HasForeignKey(b => b.LibraryMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookTagModel>()
                .HasKey(bt => new { bt.BookId, bt.TagId });

            modelBuilder.Entity<BookTagModel>()
                .HasOne(bt => bt.Book)
                .WithMany(b => b.BookTags)
                .HasForeignKey(bt => bt.BookId);

            modelBuilder.Entity<BookTagModel>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BookTags)
                .HasForeignKey(bt => bt.TagId);
        }
    }
}
