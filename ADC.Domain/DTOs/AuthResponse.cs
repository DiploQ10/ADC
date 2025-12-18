using ADC.Domain.Responses;

namespace ADC.Domain.DTOs;

public class AuthResponse : ResponseBase
{
    public DateTime ExpiresAt { get; set; }
    
    public AuthResponse() : base() { }
    
    public AuthResponse(Responses.Responses response, string token, DateTime expiresAt, string message = "") : base(response)
    {
        Token = token;
        ExpiresAt = expiresAt;
        Message = message;
    }
}
