using HackerNews.Models;
using HackerNews.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.UnitTests.Services
{
    [TestClass]
    public class HackerNewsServicesUnitTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _client;
        private HackerNewsServices _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _client = new HttpClient(_mockHttpMessageHandler.Object);
            _service = new HackerNewsServices(_client);
        }

        [TestMethod]
        public async Task GetTopStoriesDataFromAPIAsync_ReturnsData_WhenResponseIsSuccess()
        {
            // Arrange
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[1,2,3,4,5]")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(fakeResponse);

            // Act
            var result = await _service.GetTopStoriesDataFromAPIAsync("testPath");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Length);
        }

        [TestMethod]
        public async Task GetItemDataFromAPIAsync_ReturnsData_WhenResponseIsSuccess()
        {
            // Arrange
            var fakeItem = new Item { Id = 1, Url = "testUrl" };
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(fakeItem))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(fakeResponse);

            // Act
            var result = await _service.GetItemDataFromAPIAsync("testPath");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fakeItem.Id, result.Id);
            Assert.AreEqual(fakeItem.Url, result.Url);
        }
    }
}
