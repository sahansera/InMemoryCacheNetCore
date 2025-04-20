using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Services;
using InMemoryCachingSample.Utils;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace InMemoryCachingSample.Tests;

public class CachedUserServiceTests
{
    private readonly Mock<IUsersService> _usersServiceMock;
    private readonly Mock<ICacheProvider> _cacheProviderMock;
    private readonly CachedUserService _cachedUserService;
    private readonly List<User> _sampleUsers;

    public CachedUserServiceTests()
    {
        _usersServiceMock = new Mock<IUsersService>();
        _cacheProviderMock = new Mock<ICacheProvider>();
        _cachedUserService = new CachedUserService(_usersServiceMock.Object, _cacheProviderMock.Object);
        
        _sampleUsers = new List<User>
        {
            new User { id = 1, email = "user1@example.com" },
            new User { id = 2, email = "user2@example.com" }
        };
    }

    [Fact]
    public async Task GetUsersAsync_WhenCacheExists_ReturnsCachedData()
    {
        // Arrange
        _cacheProviderMock
            .Setup(c => c.GetFromCache<IEnumerable<User>>(CacheKeys.Users))
            .Returns(_sampleUsers);

        // Act
        var result = await _cachedUserService.GetUsersAsync();

        // Assert
        Assert.Equal(_sampleUsers, result);
        _usersServiceMock.Verify(s => s.GetUsersAsync(), Times.Never);
    }

    [Fact]
    public async Task GetUsersAsync_WhenCacheDoesNotExist_FetchesAndCachesData()
    {
        // Arrange
        _cacheProviderMock
            .Setup(c => c.GetFromCache<IEnumerable<User>>(CacheKeys.Users))
            .Returns((IEnumerable<User>?)null);

        _usersServiceMock
            .Setup(s => s.GetUsersAsync())
            .ReturnsAsync(_sampleUsers);

        // Act
        var result = await _cachedUserService.GetUsersAsync();

        // Assert
        Assert.Equal(_sampleUsers, result);
        _usersServiceMock.Verify(s => s.GetUsersAsync(), Times.Once);
        _cacheProviderMock.Verify(
            c => c.SetCache(
                CacheKeys.Users, 
                It.IsAny<IEnumerable<User>>(), 
                It.IsAny<MemoryCacheEntryOptions>()
            ), 
            Times.Once
        );
    }
}