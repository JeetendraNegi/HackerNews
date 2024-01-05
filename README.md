**#HackerNews Web API**

**#Controller**

**HackerNewsController :**  This api controller has a endpoin to get the Data of the HackerNews by using HackerNews service.

**#Services**
**HackerNewsService :** This service is use the HttpClient to call the HackerNews API.

**CacheService:** This service is use for Caching.

**#FlowDigram**
GetTopStories(API Endpoint) --> Get the List of TopStories --> For every Story get the Item detail ---> Send the Response.

**#Approach**
For getting the each story data by storyID we make the async call, it is more faster then normal Foreach.
