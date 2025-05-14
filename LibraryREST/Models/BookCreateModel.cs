namespace LibraryREST.Models
{
    public class BookCreateModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int LibraryMemberId { get; set; }
    }
}
