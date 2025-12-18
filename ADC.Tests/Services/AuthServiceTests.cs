using Microsoft.EntityFrameworkCore;
using ADC.Infraestructure.Services;
using ADC.Infraestructure.Interfaces;
using ADC.Domain.Entities;
using ADC.Domain.DTOs;
using ADC.Persistence.Repositories;
using ADC.Persistence.Repositories.EF;
using ADC.Domain.Responses;

namespace ADC.Tests.Services
{
    [TestClass]
    public class AuthServiceTests
    {
        public IUserRepository UserRepository { get; set; } = null!;
        public IAuthService AuthService { get; set; } = null!;
        private DbContext _context = null!;

        [TestInitialize]
        public void Initialize()
        {
            // Configurar base de datos en memoria con nombre único
            var options = new DbContextOptionsBuilder<ADC.Persistence.Data.DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ADC.Persistence.Data.DataContext(options);
            UserRepository = new UserRepository(_context as ADC.Persistence.Data.DataContext);
            AuthService = new AuthService(UserRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        #region GenerateTokenAsync Tests

        [TestMethod]
        public async Task GenerateTokenAsync_ValidInput_ReturnsToken()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var email = "test@example.com";
            var role = "User";

            // Act
            var result = await AuthService.GenerateTokenAsync(userId, email, role);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public async Task GenerateTokenAsync_DifferentInputs_ReturnsDifferentTokens()
        {
            // Arrange
            var userId1 = Guid.NewGuid().ToString();
            var userId2 = Guid.NewGuid().ToString();
            var email = "test@example.com";
            var role = "User";

            // Act
            var token1 = await AuthService.GenerateTokenAsync(userId1, email, role);
            var token2 = await AuthService.GenerateTokenAsync(userId2, email, role);

            // Assert
            Assert.AreNotEqual(token1, token2);
        }

        #endregion

        #region HashPasswordAsync Tests

        [TestMethod]
        public async Task HashPasswordAsync_ValidPassword_ReturnsHash()
        {
            // Arrange
            var password = "TestPassword123";

            // Act
            var result = await AuthService.HashPasswordAsync(password);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreNotEqual(password, result);
        }

        [TestMethod]
        public async Task HashPasswordAsync_SamePassword_ReturnsDifferentHashes()
        {
            // Arrange
            var password = "TestPassword123";

            // Act
            var hash1 = await AuthService.HashPasswordAsync(password);
            var hash2 = await AuthService.HashPasswordAsync(password);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        #endregion

        #region VerifyPasswordAsync Tests

        [TestMethod]
        public async Task VerifyPasswordAsync_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "TestPassword123";
            var hash = await AuthService.HashPasswordAsync(password);

            // Act
            var result = await AuthService.VerifyPasswordAsync(password, hash);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task VerifyPasswordAsync_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123";
            var wrongPassword = "WrongPassword123";
            var hash = await AuthService.HashPasswordAsync(password);

            // Act
            var result = await AuthService.VerifyPasswordAsync(wrongPassword, hash);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task VerifyPasswordAsync_InvalidHash_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123";
            var invalidHash = "invalidhash";

            // Act
            var result = await AuthService.VerifyPasswordAsync(password, invalidHash);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region RegisterAsync Tests

        [TestMethod]
        public async Task RegisterAsync_ValidRequest_ReturnsAuthResponse()
        {
            // Arrange
            var request = new UserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "TestPassword123",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var result = await AuthService.RegisterAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Responses.Success, result.Response);
            Assert.IsNotNull(result.Token);
            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
            Assert.IsTrue(result.ExpiresAt > DateTime.UtcNow);
        }

        [TestMethod]
        public async Task RegisterAsync_UsernameAlreadyExists_ThrowsException()
        {
            // Arrange
            var request = new UserRequest
            {
                Username = "existinguser",
                Email = "test@example.com",
                Password = "TestPassword123"
            };

            var existingUserEntity = new ADC.Persistence.Models.UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "other@example.com",
                PasswordHash = "hash",
                Role = "User",
                Name = "Existing",
                LastName = "User",
                IdentityDocument = "12345",
                Password = ""
            };

            // Add existing user to database
            await UserRepository.CreateAsync(existingUserEntity);

            // Act & Assert
            try
            {
                await AuthService.RegisterAsync(request);
                Assert.Fail("Expected Exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Username already exists", ex.Message);
            }
        }

        [TestMethod]
        public async Task RegisterAsync_EmailAlreadyExists_ThrowsException()
        {
            // Arrange
            var request = new UserRequest
            {
                Username = "newuser",
                Email = "existing@example.com",
                Password = "TestPassword123"
            };

            var existingUserEntity = new ADC.Persistence.Models.UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "otheruser",
                Email = "existing@example.com",
                PasswordHash = "hash",
                Role = "User",
                Name = "Other",
                LastName = "User",
                IdentityDocument = "12345",
                Password = ""
            };

            // Add existing user to database
            await UserRepository.CreateAsync(existingUserEntity);

            // Act & Assert
            try
            {
                await AuthService.RegisterAsync(request);
                Assert.Fail("Expected Exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Email already exists", ex.Message);
            }
        }

        [TestMethod]
        public async Task RegisterAsync_PasswordIsHashed_Success()
        {
            // Arrange
            var request = new UserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "PlainTextPassword123",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var result = await AuthService.RegisterAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Responses.Success, result.Response);
            
            // Verify password was hashed (retrieve from database)
            var users = await UserRepository.GetAllAsync();
            var createdUser = users.Models.FirstOrDefault(u => u.Username == request.Username);
            Assert.IsNotNull(createdUser);
            Assert.AreNotEqual(request.Password, createdUser.PasswordHash);
        }

        #endregion

        #region LoginAsync Tests

        [TestMethod]
        public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "TestPassword123"
            };

            var passwordHash = await AuthService.HashPasswordAsync(request.Password);
            var userEntity = new ADC.Persistence.Models.UserEntity
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = "test@example.com",
                PasswordHash = passwordHash,
                Role = "User",
                Name = "Test",
                LastName = "User",
                IdentityDocument = "12345",
                Password = ""
            };

            // Add user to database
            await UserRepository.CreateAsync(userEntity);

            // Act
            var result = await AuthService.LoginAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Responses.Success, result.Response);
            Assert.IsNotNull(result.Token);
            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
            Assert.IsTrue(result.ExpiresAt > DateTime.UtcNow);
        }

        [TestMethod]
        public async Task LoginAsync_InvalidUsername_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "nonexistent",
                Password = "TestPassword123"
            };

            // Database is empty by default

            // Act & Assert
            try
            {
                await AuthService.LoginAsync(request);
                Assert.Fail("Expected UnauthorizedAccessException was not thrown");
            }
            catch (UnauthorizedAccessException ex)
            {
                Assert.AreEqual("Invalid credentials", ex.Message);
            }
        }

        [TestMethod]
        public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "WrongPassword"
            };

            var correctPasswordHash = await AuthService.HashPasswordAsync("CorrectPassword");
            var userEntity = new ADC.Persistence.Models.UserEntity
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = "test@example.com",
                PasswordHash = correctPasswordHash,
                Role = "User",
                Name = "Test",
                LastName = "User",
                IdentityDocument = "12345",
                Password = ""
            };

            // Add user to database
            await UserRepository.CreateAsync(userEntity);

            // Act & Assert
            try
            {
                await AuthService.LoginAsync(request);
                Assert.Fail("Expected UnauthorizedAccessException was not thrown");
            }
            catch (UnauthorizedAccessException ex)
            {
                Assert.AreEqual("Invalid credentials", ex.Message);
            }
        }

        [TestMethod]
        public async Task LoginAsync_TokenGenerated_IsValid()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "testuser",
                Password = "TestPassword123"
            };

            var passwordHash = await AuthService.HashPasswordAsync(request.Password);
            var userEntity = new ADC.Persistence.Models.UserEntity
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = "test@example.com",
                PasswordHash = passwordHash,
                Role = "User",
                Name = "Test",
                LastName = "User",
                IdentityDocument = "12345",
                Password = ""
            };

            await UserRepository.CreateAsync(userEntity);

            // Act
            var result = await AuthService.LoginAsync(request);

            // Assert
            Assert.IsNotNull(result.Token);
            Assert.IsTrue(result.Token.Length > 50); // JWT tokens are typically longer
            Assert.IsTrue(result.Token.Contains(".")); // JWT has 3 parts separated by dots
        }

        #endregion
    }
}
