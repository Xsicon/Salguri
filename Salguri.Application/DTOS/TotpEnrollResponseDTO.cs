using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salguri.Application.DTOS;
public class TotpEnrollResponseDTO
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    // This is the QR code image as base64 string
    // Frontend will convert this to an actual QR code image
    public string? QrCode { get; set; }

    // This is the secret key shown as text
    // For users who cant scan QR code they can enter this manually
    public string? SecretKey { get; set; }

    // Supabase gives us a FactorId to identify this TOTP enrollment
    // We need this later when verifying the code
    public string? FactorId { get; set; }
}
