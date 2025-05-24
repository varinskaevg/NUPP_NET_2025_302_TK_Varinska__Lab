namespace LibraryREST.Models
{
    public class LibraryMemberApiModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
