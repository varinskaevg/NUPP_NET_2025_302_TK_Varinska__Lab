using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Infrastructure.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        // Один-до-багатьох: Книга належить одному учаснику
        public int LibraryMemberId { get; set; }

        // Багато-до-багатьох: Книга має багато тегів
        public List<BookTagModel> BookTags { get; set; }

        [JsonIgnore] // Це призведе до того, що властивість не буде серіалізована в JSON
        public LibraryMemberModel LibraryMember { get; set; }
    }
}
