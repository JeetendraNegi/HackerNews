<img width="349" alt="image" src="https://github.com/JeetendraNegi/HackerNews/assets/40231981/f0e418d1-a8f9-457e-9a05-bd532faf7a32">**#HackerNews Web API**

**#Controller**

**HackerNewsController :**  This api controller has a endpoin to get the Data of the HackerNews by using HackerNews service.

**#Services**
**HackerNewsService :** This service is use the HttpClient to call the HackerNews API.

**CacheService:** This service is use for Caching.

**#FlowDigram**
GetTopStories(API Endpoint) --> Get the List of TopStories --> For every Story get the Item detail ---> Send the Response.

**#Approach**
For getting the each story data by storyID we make the async call, it is more faster then normal Foreach.
