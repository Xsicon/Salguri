using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salguri.Application.DTOS;

public class UserRequestDTO
{
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

