using Supabase.Postgrest.Attributes;
namespace Salguri.Domain.Models;

[Table("properties")]
public class Property : Supabase.Postgrest.Models.BaseModel
{
    [PrimaryKey("id", true)]
    public Guid? Id { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("title")]
    public string? Title { get; set; }
    [Column("location")]
    public string? Location { get; set; }
    [Column("price")]
    public string? price { get; set; }
    [Column("price_label")]
    public string? PriceLabel { get; set; }
    [Column("beds")]
    public int? Beds { get; set; }
    [Column("baths")]
    public int? Baths { get; set; }
    [Column("sqft")]
    public int? Sqft { get; set; }
    [Column("year_built")]
    public int? YearBuilt { get; set; }
    [Column("rating")]
    public double? rating { get; set; }
    [Column("reviews")]
    public int? Reviews { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("images")]
    public string? Images { get; set; }
    [Column("Amenities")]
    public string? Amenities { get; set; }
    [Column("type")]
    public string? Type { get; set; }
    [Column("agent_name")]
    public string? AgentName { get; set; }
    [Column("agent_role")]
    public string? AgentRole { get; set; }
    [Column("agent_rating")]
    public string? AgentRating { get; set; }
    [Column("agent_deals")]
    public string? AgentDeals { get; set; }
}
