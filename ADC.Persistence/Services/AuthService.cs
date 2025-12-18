using ADC.Domain.DTOs;
using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ADC.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> GenerateTokenAsync(string userId, string email, string role)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = _configuration["JwtSettings:Issuer"] ?? "ADC";
        var audience = _configuration["JwtSettings:Audience"] ?? "ADCUsers";
        var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("sub", userId),
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }

    public Task<string> HashPasswordAsync(string password)
    {
        // BCrypt is CPU-intensive, so we offload to a background thread
        return Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12)));
    }

    public Task<bool> VerifyPasswordAsync(string password, string hash)
    {
        // BCrypt is CPU-intensive, so we offload to a background thread
        return Task.Run(() => BCrypt.Net.BCrypt.Verify(password, hash));
    }

    public async Task<AuthResponse> RegisterAsync(UserRequest request)
    {
        // Check if username already exists
        var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUsername.Response == Responses.Success)
        {
            return new AuthResponse(Responses.InvalidParameters, string.Empty, DateTime.MinValue, "Username already exists");
        }

        // Check if email already exists
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail.Response == Responses.Success)
        {
            return new AuthResponse(Responses.InvalidParameters, string.Empty, DateTime.MinValue, "Email already exists");
        }

        // Hash password
        var passwordHash = await HashPasswordAsync(request.Password);

        // Create user entity
        var userEntity = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            Name = request.FirstName,
            LastName = request.LastName,
            Password = passwordHash, // For backward compatibility with existing code
            PasswordHash = passwordHash, // Primary field for authentication
            Role = "User",
            IsActive = true,
            IdentityDocument = string.Empty // Required field, set to empty
        };

        // Save user
        var createResponse = await _userRepository.CreateAsync(userEntity);
        
        if (createResponse.Response != Responses.Success)
        {
            return new AuthResponse(Responses.InvalidParameters, string.Empty, DateTime.MinValue, "Failed to create user");
        }

        // Generate token
        var token = await GenerateTokenAsync(createResponse.Id.ToString(), request.Email, "User");
        var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"));

        return new AuthResponse(Responses.Success, token, expiresAt, "User registered successfully");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Find user by username
        var userResponse = await _userRepository.GetByUsernameAsync(request.Username);
        
        if (userResponse.Response != Responses.Success || userResponse.Model == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var user = userResponse.Model;

        // Verify password
        var isPasswordValid = await VerifyPasswordAsync(request.Password, user.PasswordHash);
        
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

        // Generate token
        var token = await GenerateTokenAsync(user.Id.ToString(), user.Email, user.Role);
        var expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"));

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.Update(user);

        return new AuthResponse(Responses.Success, token, expiresAt, "Login successful");
    }
}
