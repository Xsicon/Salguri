using Supabase.Postgrest.Attributes;
namespace Salguri.Domain.Models;

[Table("profiles")]
public class Profiles : Supabase.Postgrest.Models.BaseModel
{
    [PrimaryKey("id", true)]
    public Guid? Id { get; set; }
    [Column("full_name")]
    public string? FullName { get; set; }
    [Column("phone")]
    public string? Phone { get; set; }
    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }
    [Column("role")]
    public string? Role { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}
