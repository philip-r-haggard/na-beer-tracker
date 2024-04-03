using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Entries")]
public class Entry
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
