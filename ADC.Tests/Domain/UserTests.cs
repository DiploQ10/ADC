using ADC.Domain.Entities;

namespace ADC.Tests.Domain
{
    [TestClass]
    public class UserTests
    {
        #region Constructor and Initialization Tests

        [TestMethod]
        public void Constructor_InitializesWithDefaultValues_PropertiesSetCorrectly()
        {
            // Act
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };

            // Assert
            Assert.IsNotNull(user.Id);
            Assert.IsFalse(string.IsNullOrEmpty(user.Id));
            Assert.AreEqual("testuser", user.Username);
            Assert.AreEqual("test@example.com", user.Email);
            Assert.AreEqual("hashedpassword", user.PasswordHash);
            Assert.AreEqual(string.Empty, user.FirstName);
            Assert.AreEqual(string.Empty, user.LastName);
            Assert.IsTrue((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 1);
            Assert.IsNull(user.LastLoginAt);
            Assert.IsTrue(user.IsActive);
            Assert.AreEqual("User", user.Role);
        }

        [TestMethod]
        public void Constructor_GeneratesUniqueIds_EachInstanceHasDifferentId()
        {
            // Act
            var user1 = new User { Username = "user1", Email = "user1@test.com", PasswordHash = "hash1" };
            var user2 = new User { Username = "user2", Email = "user2@test.com", PasswordHash = "hash2" };

            // Assert
            Assert.AreNotEqual(user1.Id, user2.Id);
        }

        [TestMethod]
        public void Constructor_SetsCreatedAtTimestamp_WithinReasonableTime()
        {
            // Act
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };

            // Assert
            var timeDifference = DateTime.UtcNow - user.CreatedAt;
            Assert.IsTrue(timeDifference.TotalSeconds >= 0 && timeDifference.TotalSeconds < 1);
        }

        [TestMethod]
        public void Constructor_SetsDefaultRole_ToUser()
        {
            // Act
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };

            // Assert
            Assert.AreEqual("User", user.Role);
        }

        [TestMethod]
        public void Constructor_SetsIsActive_ToTrue()
        {
            // Act
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };

            // Assert
            Assert.IsTrue(user.IsActive);
        }

        [TestMethod]
        public void Constructor_InitializesLastLoginAt_AsNull()
        {
            // Act
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };

            // Assert
            Assert.IsNull(user.LastLoginAt);
        }

        #endregion

        #region Property Assignment Tests

        [TestMethod]
        public void Username_SetValidValue_UpdatesCorrectly()
        {
            // Arrange
            var user = new User { Username = "initial", Email = "test@test.com", PasswordHash = "hash" };

            // Act
            user.Username = "newusername";

            // Assert
            Assert.AreEqual("newusername", user.Username);
        }

        [TestMethod]
        public void Email_SetValidValue_UpdatesCorrectly()
        {
            // Arrange
            var user = new User { Username = "test", Email = "old@test.com", PasswordHash = "hash" };

            // Act
            user.Email = "new@test.com";

            // Assert
            Assert.AreEqual("new@test.com", user.Email);
        }

        [TestMethod]
        public void Role_SetValidValue_UpdatesCorrectly()
        {
            // Arrange
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };

            // Act
            user.Role = "Admin";

            // Assert
            Assert.AreEqual("Admin", user.Role);
        }

        #endregion

        #region Business Methods Tests

        [TestMethod]
        public void Deactivate_WhenCalled_SetsIsActiveToFalse()
        {
            // Arrange
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };
            Assert.IsTrue(user.IsActive); // Verify initial state

            // Act
            user.Deactivate();

            // Assert
            Assert.IsFalse(user.IsActive);
        }

        [TestMethod]
        public void Activate_WhenCalled_SetsIsActiveToTrue()
        {
            // Arrange
            var user = new User { Username = "test", Email = "test@test.com", PasswordHash = "hash" };
            user.Deactivate(); // First deactivate
            Assert.IsFalse(user.IsActive); // Verify deactivated state

            // Act
            user.Activate();

            // Assert
            Assert.IsTrue(user.IsActive);
        }

        #endregion
    }
}
