using InMemoryCachingSample.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace InMemoryCachingSample.Tests;

public class CacheProviderTests
{
    [Fact]
    public void GetFromCache_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var memoryCacheMock = new Mock<IMemoryCache>();
        object? cachedValue = "cached-value";
        
        memoryCacheMock
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(true);
        
        var cacheProvider = new CacheProvider(memoryCacheMock.Object);
        
        // Act
        var result = cacheProvider.GetFromCache<string>("test-key");
        
        // Assert
        Assert.Equal("cached-value", result);
        memoryCacheMock.Verify(m => m.TryGetValue("test-key", out cachedValue), Times.Once);
    }
    
    [Fact]
    public void GetFromCache_WhenKeyDoesNotExist_ReturnsNull()
    {
        // Arrange
        var memoryCacheMock = new Mock<IMemoryCache>();
        object? cachedValue = null;
        
        memoryCacheMock
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(false);
        
        var cacheProvider = new CacheProvider(memoryCacheMock.Object);
        
        // Act
        var result = cacheProvider.GetFromCache<string>("non-existent-key");
        
        // Assert
        Assert.Null(result);
        memoryCacheMock.Verify(m => m.TryGetValue("non-existent-key", out cachedValue), Times.Once);
    }
    
    [Fact]
    public void SetCache_SetsValueInCache()
    {
        // Arrange
        var memoryCacheMock = new Mock<IMemoryCache>();
        var cacheMockSetup = new Mock<ICacheEntry>();
        
        memoryCacheMock
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheMockSetup.Object);
        
        var cacheProvider = new CacheProvider(memoryCacheMock.Object);
        var options = new MemoryCacheEntryOptions();
        
        // Act
        cacheProvider.SetCache("test-key", "test-value", options);
        
        // Assert
        memoryCacheMock.Verify(m => m.CreateEntry("test-key"), Times.Once);
    }
    
    [Fact]
    public void ClearCache_RemovesKeyFromCache()
    {
        // Arrange
        var memoryCacheMock = new Mock<IMemoryCache>();
        var cacheProvider = new CacheProvider(memoryCacheMock.Object);
        
        // Act
        cacheProvider.ClearCache("test-key");
        
        // Assert
        memoryCacheMock.Verify(m => m.Remove("test-key"), Times.Once);
    }
}