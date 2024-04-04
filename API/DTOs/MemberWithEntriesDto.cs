namespace API.DTOs
{
    public class MemberWithEntriesDto
    {
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public ICollection<EntryDto> Entries { get; set; }
    }
}
