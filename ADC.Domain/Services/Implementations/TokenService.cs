using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ADC.Domain.Services.Implementations;

internal class TokenService(Microsoft.Extensions.Configuration.IConfiguration configuration) : ITokenService
{

    private string JwtKey => configuration["Jwt:Key"] ?? string.Empty;


    /// <summary>
    /// Genera un JSON Web Token
    /// </summary>
    /// <param name="user">Modelo de usuario</param>
    public string Generate(string userId, int appID)
    {
        // Configuración
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));

        // Credenciales
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        // Reclamaciones
        var claims = new[]
        {
            new Claim(ClaimTypes.PrimarySid, userId)
        };

        // Expiración del token
        var expiración = DateTime.UtcNow.AddHours(5);

        // Token
        var token = new JwtSecurityToken(null, null, claims, null, expiración, credentials);

        // Genera el token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    /// <summary>
    /// Valida un JSON Web token
    /// </summary>
    /// <param name="token">Token a validar</param>
    public (bool IsAuthenticated, string userId) Validate(string token)
    {
        try
        {
            // Comprobación
            if (string.IsNullOrWhiteSpace(token))
                return new()
                {
                    IsAuthenticated = false
                };

            // Configurar la clave secreta.
            var key = Encoding.ASCII.GetBytes(JwtKey);

            // Validar el token
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true
            };

            try
            {

                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Si el token es válido, puedes acceder a los claims (datos) del usuario
                var user = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                _ = int.TryParse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.PrimarySid)?.Value, out var id);

                // Devuelve una respuesta exitosa
                return new()
                {
                    IsAuthenticated = true,
                    userId = user ?? string.Empty
                };

            }
            catch (SecurityTokenException)
            {
            }
        }
        catch { }

        return new()
        {
            IsAuthenticated = false
        };
    }

}