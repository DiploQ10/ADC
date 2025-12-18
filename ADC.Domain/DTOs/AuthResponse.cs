using ADC.Domain.Responses;

namespace ADC.Domain.DTOs;

public class AuthResponse : ResponseBase
{
    public DateTime ExpiresAt { get; set; }

    public AuthResponse()
    {
        Response = Responses.Responses.Undefined;
    }

    public AuthResponse(Responses.Responses response, string token, DateTime expiresAt)
    {
        Response = response;
        Token = token;
        ExpiresAt = expiresAt;
    }
}
