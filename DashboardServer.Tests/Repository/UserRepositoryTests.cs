using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserRepository _repository;
        private readonly IUserResolverService _userResolverService;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("123");

            _userResolverService = userResolverMock.Object;
            _dbContext = new ApplicationDbContext(options, _userResolverService);
            _repository = new UserRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region SetApiKey Tests

        [Fact]
        public async Task SetApiKey_SetsApiKeyForExistingUser()
        {
            // Arrange
            var userId = "user123";
            var encryptedApiKey = "encrypted_key_12345";

            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.SetApiKey(userId, encryptedApiKey);

            // Assert
            var updatedUser = await _dbContext.Users.FindAsync(userId);
            Assert.NotNull(updatedUser);
            Assert.Equal(encryptedApiKey, updatedUser.CanvasApiKey);
        }

        [Fact]
        public async Task SetApiKey_UpdatesExistingApiKey()
        {
            // Arrange
            var userId = "user123";
            var oldApiKey = "old_encrypted_key";
            var newApiKey = "new_encrypted_key";

            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                CanvasApiKey = oldApiKey
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.SetApiKey(userId, newApiKey);

            // Assert
            var updatedUser = await _dbContext.Users.FindAsync(userId);
            Assert.NotNull(updatedUser);
            Assert.Equal(newApiKey, updatedUser.CanvasApiKey);
            Assert.NotEqual(oldApiKey, updatedUser.CanvasApiKey);
        }

        #endregion

        #region GetEncryptedApiKey Tests

        [Fact]
        public async Task GetEncryptedApiKey_ReturnsApiKey_WhenUserExists()
        {
            // Arrange
            var userId = "user123";
            var encryptedApiKey = "encrypted_key_12345";

            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                CanvasApiKey = encryptedApiKey
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetEncryptedApiKey(userId);

            // Assert
            Assert.Equal(encryptedApiKey, result);
        }

        [Fact]
        public async Task GetEncryptedApiKey_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "nonexistent_user";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _repository.GetEncryptedApiKey(userId));

            Assert.Equal("User not found or API key not set", exception.Message);
        }

        [Fact]
        public async Task GetEncryptedApiKey_ThrowsException_WhenApiKeyIsNull()
        {
            // Arrange
            var userId = "user123";

            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                CanvasApiKey = null
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _repository.GetEncryptedApiKey(userId));

            Assert.Equal("User not found or API key not set", exception.Message);
        }

        [Fact]
        public async Task GetEncryptedApiKey_ThrowsException_WhenApiKeyIsEmpty()
        {
            // Arrange
            var userId = "user123";

            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com",
                CanvasApiKey = ""
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _repository.GetEncryptedApiKey(userId));

            Assert.Equal("User not found or API key not set", exception.Message);
        }

        #endregion
    }
}
