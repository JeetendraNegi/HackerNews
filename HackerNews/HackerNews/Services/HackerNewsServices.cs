using HackerNews.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HackerNews.Services
{
    /// <summary>
    /// HackerNewsServices.
    /// </summary>
    public class HackerNewsServices : IHackerNewsServices
    {
        private readonly HttpClient client;
        private const string APIUrl = "https://hacker-news.firebaseio.com/";

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="client">the HttpClient.</param>
        public HackerNewsServices(HttpClient client)
        {
            this.client = client;
            client.BaseAddress = new Uri(APIUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Get Top Stories Data From API.
        /// </summary>
        /// <param name="path">the path of the endpoint.</param>
        /// <returns>the list of stories.</returns>
        public async Task<int[]> GetTopStoriesDataFromAPIAsync(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int[]>(data);
            }
            return null;
        }

        /// <summary>
        /// Get Item Data From API.
        /// </summary>
        /// <param name="path">the path of the endpoint.</param>
        /// <returns>return the Item.</returns>
        public async Task<Item> GetItemDataFromAPIAsync(string path)
        {
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Item>(data);
            }
            return new Item();
        }
    }
}
