using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Entities;

[Table("Entries")]
public class Entry
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }

    public int AppUserId { get; set; }
    [JsonIgnore]
    public AppUser AppUser { get; set; }
}
