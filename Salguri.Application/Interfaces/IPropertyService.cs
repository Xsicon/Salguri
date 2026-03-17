using Salguri.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salguri.Application.Interfaces;

public interface IPropertyService
{
    Task<PropertyResponseDTO> GetAllPropertiesAsync();
    Task<PropertyResponseDTO> GetPropertyByIdAsync(Guid id);
}
