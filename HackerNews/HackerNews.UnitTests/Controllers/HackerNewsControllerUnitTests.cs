using HackerNews.Controllers;
using HackerNews.Models;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;

namespace HackerNews.UnitTests.Controllers
{
    [TestClass]
    public class HackerNewsControllerUnitTests
    {
        private HackerNewsController hackerNewsController;
        private Mock<IHackerNewsServices> mockHackerNewsServices;
        private Mock<ICacheService> mockMemoryCache;

        [TestInitialize]
        public void Initialize()
        {
            this.mockHackerNewsServices = new Mock<IHackerNewsServices>();
            this.mockMemoryCache = new Mock<ICacheService>();
            this.hackerNewsController = new HackerNewsController(this.mockHackerNewsServices.Object, this.mockMemoryCache.Object);
        }

        [TestMethod]
        public async Task GetTopStories_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            var fakeData = new List<Item> { new Item { Id = 1, Title = "Test" } };
            var fakeResponse = new List<int> { 1, 2, 3, 4, 5 };

            mockHackerNewsServices.Setup(s => s.GetTopStoriesDataFromAPIAsync(It.IsAny<string>())).ReturnsAsync(fakeResponse.ToArray());
            mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out fakeData)).Returns(false);

            // Act
            var result = await hackerNewsController.GetTopStories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult?.Value, typeof(List<Item>));
            var model = okResult?.Value as List<Item>;
            Assert.AreEqual(5, model?.Count);
        }

        [TestMethod]
        public async Task GetTopStories_ReturnsOkResult_WhenGetTopStoriesDataFromAPIAsyncReturnsNull()
        {
            // Arrange
            var fakeData = new List<Item> { new Item { Id = 1, Title = "Test" } };
            var fakeResponse = new List<int>();

            mockHackerNewsServices.Setup(s => s.GetTopStoriesDataFromAPIAsync(It.IsAny<string>())).ReturnsAsync(fakeResponse.ToArray());
            mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out fakeData)).Returns(false);

            // Act
            var result = await hackerNewsController.GetTopStories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult?.Value, typeof(List<Item>));
            var model = okResult?.Value as List<Item>;
            Assert.AreEqual(0, model?.Count);
        }

        [TestMethod]
        public async Task GetTopStories_ReturnsOkResult_WhenResponseIsNull()
        {
            // Arrange
            var fakeResponse = new List<int> {  };

            mockHackerNewsServices.Setup(x => x.GetTopStoriesDataFromAPIAsync(It.IsAny<string>()))
                .ReturnsAsync(fakeResponse.ToArray());

            // Act
            var result = await hackerNewsController.GetTopStories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetTopStories_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            mockHackerNewsServices.Setup(x => x.GetTopStoriesDataFromAPIAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await hackerNewsController.GetTopStories();

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, statusCodeResult.StatusCode);
        }
    }
}