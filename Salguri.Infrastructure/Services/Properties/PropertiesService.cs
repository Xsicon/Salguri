using Salguri.Application.DTOS;
using Salguri.Application.Interfaces;
using Salguri.Domain.Models;
using Supabase.Gotrue;

using Client = Supabase.Client;

namespace Salguri.Infrastructure.Services.Properties;

public class PropertiesService : IPropertyService
{
    private readonly Client _client;

    public PropertiesService(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<PropertyResponseDTO> GetAllPropertiesAsync()
    {
        try
        {
            var result = await _client.From<Property>().Get();

            var data = result.Models.Select(p => new PropertyDTO
            {
                Id = p.Id,
                Title = p.Title,
                Location = p.Location,
                Price = p.price,
                PriceLabel = p.PriceLabel,
                Beds = p.Beds,
                Baths = p.Baths,
                Sqft = p.Sqft,
                YearBuilt = p.YearBuilt,
                Rating = p.rating,
                Reviews = p.Reviews,
                Description = p.Description,
                Images = p.Images,
                Amenities = p.Amenities,
                Type = p.Type,
                AgentName = p.AgentName,
                AgentRole = p.AgentRole,
                AgentRating = p.AgentRating,
                AgentDeals = p.AgentDeals
            }).ToList();

            return new PropertyResponseDTO
            {
                Success = true,
                Message = "Properties fetched successfully.",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new PropertyResponseDTO
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<PropertyResponseDTO> GetPropertyByIdAsync(Guid id)
    {
        try
        {
            var result = await _client
                .From<Property>()
                .Where(p => p.Id == id)
                .Get();

            var property = result.Models.FirstOrDefault();
            if (property == null)
                return new PropertyResponseDTO { Success = false, Message = "Property not found." };

            return new PropertyResponseDTO
            {
                Success = true,
                Data = new List<PropertyDTO>
                {
                    new PropertyDTO
                    {
                        Id          = property.Id,
                        Title       = property.Title,
                        Location    = property.Location,
                        Price       = property.price,
                        PriceLabel  = property.PriceLabel,
                        Beds        = property.Beds,
                        Baths       = property.Baths,
                        Sqft        = property.Sqft,
                        YearBuilt   = property.YearBuilt,
                        Rating      = property.rating,
                        Reviews     = property.Reviews,
                        Description = property.Description,
                        Images      = property.Images,
                        Amenities   = property.Amenities,
                        Type        = property.Type,
                        AgentName   = property.AgentName,
                        AgentRole   = property.AgentRole,
                        AgentRating = property.AgentRating,
                        AgentDeals  = property.AgentDeals
                    }
                }
            };
        }
        catch (Exception ex)
        {
            return new PropertyResponseDTO { Success = false, Message = ex.Message };
        }
    }
}
