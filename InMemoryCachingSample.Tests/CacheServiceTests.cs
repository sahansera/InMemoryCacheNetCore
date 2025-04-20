using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Services;
using InMemoryCachingSample.Utils;
using Moq;
using Xunit;

namespace InMemoryCachingSample.Tests;

public class CacheServiceTests
{
    private readonly Mock<ICacheProvider> _cacheProviderMock;
    private readonly CacheService _cacheService;
    private readonly List<User> _sampleUsers;

    public CacheServiceTests()
    {
        _cacheProviderMock = new Mock<ICacheProvider>();
        _cacheService = new CacheService(_cacheProviderMock.Object);
        
        _sampleUsers = new List<User>
        {
            new User { id = 1, email = "user1@example.com" },
            new User { id = 2, email = "user2@example.com" }
        };
    }

    [Fact]
    public void GetCachedUser_ReturnsUsersFromCache()
    {
        // Arrange
        _cacheProviderMock
            .Setup(c => c.GetFromCache<IEnumerable<User>>(CacheKeys.Users))
            .Returns(_sampleUsers);

        // Act
        var result = _cacheService.GetCachedUser();

        // Assert
        Assert.Equal(_sampleUsers, result);
        _cacheProviderMock.Verify(c => c.GetFromCache<IEnumerable<User>>(CacheKeys.Users), Times.Once);
    }

    [Fact]
    public void ClearCache_RemovesUserCacheKey()
    {
        // Act
        _cacheService.ClearCache();

        // Assert
        _cacheProviderMock.Verify(c => c.ClearCache(CacheKeys.Users), Times.Once);
    }
}