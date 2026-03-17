using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salguri.Application.DTOS;
public class TotpVerifyRequestDTO
{
    // The FactorId we got during enrollment
    public string? FactorId { get; set; }

    // The 6 digit code from authenticator app
    public string? Code { get; set; }
}
