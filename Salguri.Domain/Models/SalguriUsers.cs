using Postgrest.Attributes;
using Postgrest.Models;
namespace Salguri.Domain.Models;

[Table("users")]
public class SalguriUsers : BaseModel
{
    [PrimaryKey("id")]
    public string? Id { get; set; }
    [Column("role_id")]
    public long? RoleId { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime DeletedAt { get; set; }
    [Column("is_active")]
    public bool? IsActive { get; set; }
    [Column("fullname")]
    public string? FullName { get; set; }
    [Column("email")]
    public string? Email { get; set; }

}
