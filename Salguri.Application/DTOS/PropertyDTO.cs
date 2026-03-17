using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salguri.Application.DTOS;
// DTOs/PropertyDTO.cs
public class PropertyDTO
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Location { get; set; }
    public string? Price { get; set; }
    public string? PriceLabel { get; set; }
    public int? Beds { get; set; }
    public int? Baths { get; set; }
    public int? Sqft { get; set; }
    public int? YearBuilt { get; set; }
    public double? Rating { get; set; }
    public int? Reviews { get; set; }
    public string? Description { get; set; }
    public string? Images { get; set; }
    public string? Amenities { get; set; }
    public string? Type { get; set; }
    public string? AgentName { get; set; }
    public string? AgentRole { get; set; }
    public string? AgentRating { get; set; }
    public string? AgentDeals { get; set; }
}

public class PropertyResponseDTO
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<PropertyDTO>? Data { get; set; }
}
