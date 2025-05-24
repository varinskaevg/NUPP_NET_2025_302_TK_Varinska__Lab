using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Infrastructure.Models
{
    public class LibraryMemberModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public DateTime RegisteredAt { get; set; }  // <-- додай це

        public List<BookModel> BorrowedBooks { get; set; } = new List<BookModel>();
    }
}
