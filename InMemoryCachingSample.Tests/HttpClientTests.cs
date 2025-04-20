using System.Net;
using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using Moq;
using Moq.Protected;
using Xunit;

namespace InMemoryCachingSample.Tests;

public class HttpClientTests
{
    [Fact]
    public async Task Get_ReturnsUsersFromApi()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"{""data"":[{""id"":1,""email"":""george.bluth@reqres.in""},{""id"":2,""email"":""janet.weaver@reqres.in""}]}")
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new System.Net.Http.HttpClient(handlerMock.Object);
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var client = new InMemoryCachingSample.Infrastructure.HttpClient(factory.Object);

        // Act
        var users = await client.Get();

        // Assert
        var userList = users.ToList();
        Assert.Equal(2, userList.Count);
        Assert.Equal(1, userList[0].id);
        Assert.Equal("george.bluth@reqres.in", userList[0].email);
        Assert.Equal(2, userList[1].id);
        Assert.Equal("janet.weaver@reqres.in", userList[1].email);
    }

    [Fact]
    public async Task Get_WhenApiReturnsError_ThrowsHttpRequestException()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new System.Net.Http.HttpClient(handlerMock.Object);
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var client = new InMemoryCachingSample.Infrastructure.HttpClient(factory.Object);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => client.Get());
    }
}