using HackerNews.Models;

namespace HackerNews.Services
{
    public interface IHackerNewsServices
    {
        Task<int[]> GetTopStoriesDataFromAPIAsync(string path);
        Task<Item> GetItemDataFromAPIAsync(string path);
    }
}
