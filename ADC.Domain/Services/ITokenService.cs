namespace ADC.Domain.Services;

public interface ITokenService
{
    string Generate(string userId, int appID);
    (bool IsAuthenticated, string userId) Validate(string token);
}