using HackerNews.Models;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace HackerNews.Controllers
{
    /// <summary>
    /// HackerNewsController.
    /// </summary>
    [Route("api/HackerNews")]
    [ApiController]    
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsServices hackerNewsServices;
        private readonly ICacheService memoryCache;
        private const string topStoriesPath = "v0/topstories.json";

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="hackerNewsServices">the hacker News Services.</param>
        /// <param name="memoryCache">the memoryCache service.</param>
        public HackerNewsController(IHackerNewsServices hackerNewsServices, ICacheService memoryCache)
        {
            this.hackerNewsServices = hackerNewsServices;
            this.memoryCache = memoryCache;
        }


        /// <summary>
        /// GetTopStories
        /// </summary>
        /// <returns>A list of Sotry Items.</returns>
        [HttpGet("topStories")]
        public async Task<ActionResult> GetTopStories()
        {
            try
            {
                var cacheKey = "TopStories";
                if (!memoryCache.TryGetValue(cacheKey, out List<Item> newStories))
                {
                    var response = await hackerNewsServices.GetTopStoriesDataFromAPIAsync(topStoriesPath);
                    newStories = new List<Item>();
                    if (response != null && response.Any())
                    {
                        var tasks = response.Select(async x =>
                        {
                            var storyData = await GetItemById(x);
                            newStories.Add(storyData);
                        });
                        await Task.WhenAll(tasks);
                    }

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                    // Save data in cache.
                    if (newStories.Any())
                    {
                        memoryCache.Set(cacheKey, newStories, cacheEntryOptions);
                    }
                }

                return this.Ok(newStories);
            }
            catch
            {
                // log the exception
                return this.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get Item By ID.
        /// </summary>
        /// <param name="id">Id of the Item.</param>
        /// <returns>return the Item.</returns>
        private async Task<Item> GetItemById(int id)
        {
           return await hackerNewsServices.GetItemDataFromAPIAsync($"v0/item/{id}.json");
        }
    }
}
