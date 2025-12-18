using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ADC.Domain.DTOs;
using ADC.Infraestructure.Interfaces;
using ADC.Persistence.Repositories;
using ADC.Persistence.Models;
using ADC.Persistence.Models.Enums;
using Responses = ADC.Domain.Responses.Responses;

namespace ADC.Infraestructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private const string JwtSecret = "your-secret-key-min-32-characters-long-for-security";
    private const int TokenExpirationHours = 24;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<string> GenerateTokenAsync(string userId, string email, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtSecret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(TokenExpirationHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    public Task<string> HashPasswordAsync(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        var hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        return Task.FromResult(Convert.ToBase64String(hashBytes));
    }

    public Task<bool> VerifyPasswordAsync(string password, string hash)
    {
        try
        {
            var hashBytes = Convert.FromBase64String(hash);
            
            if (hashBytes.Length != 48)
                return Task.FromResult(false);

            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var computedHash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != computedHash[i])
                    return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<AuthResponse> RegisterAsync(UserRequest request)
    {
        // Check if username exists
        var allUsers = await _userRepository.GetAllAsync();
        if (allUsers.Models.Any(u => u.Username == request.Username))
        {
            throw new Exception("Username already exists");
        }

        // Check if email exists
        if (allUsers.Models.Any(u => u.Email == request.Email))
        {
            throw new Exception("Email already exists");
        }

        // Hash password
        var passwordHash = await HashPasswordAsync(request.Password);

        // Create user entity
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Name = request.FirstName,
            IdentityDocument = string.Empty,
            Password = string.Empty,
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            State = EntityState.Active
        };

        // Save user
        var createResponse = await _userRepository.CreateAsync(userEntity);

        if (createResponse.Response != Responses.Success)
        {
            throw new Exception("Failed to create user");
        }

        // Generate token
        var token = await GenerateTokenAsync(userEntity.Id.ToString(), userEntity.Email, userEntity.Role);
        var expiresAt = DateTime.UtcNow.AddHours(TokenExpirationHours);

        return new AuthResponse(Responses.Success, token, expiresAt);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Get all users and find by username
        var allUsers = await _userRepository.GetAllAsync();
        var user = allUsers.Models.FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Verify password
        var isPasswordValid = await VerifyPasswordAsync(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Generate token
        var token = await GenerateTokenAsync(user.Id.ToString(), user.Email, user.Role);
        var expiresAt = DateTime.UtcNow.AddHours(TokenExpirationHours);

        return new AuthResponse(Responses.Success, token, expiresAt);
    }
}
