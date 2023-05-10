using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUsersFeedApi
    {
        /// <summary>
        /// List all Feed elements of Users This can only be done by the logged in user.
        /// </summary>
        /// <param name="feedToken"></param>
        /// <returns>UserFeedInfoPage</returns>
        UserFeedInfoPage ListFeedUsers (string feedToken);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class UsersFeedApi : IUsersFeedApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersFeedApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public UsersFeedApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersFeedApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UsersFeedApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        /// List all Feed elements of Users This can only be done by the logged in user.
        /// </summary>
        /// <param name="feedToken"></param>
        /// <returns>UserFeedInfoPage</returns>
        public UserFeedInfoPage ListFeedUsers (string feedToken)
        {
            // verify the required parameter 'feedToken' is set
            if (feedToken == null) throw new ApiException(400, "Missing required parameter 'feedToken' when calling ListFeedUsers");
    
            var path = "/usersfeed";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (feedToken != null) queryParams.Add("feedToken", ApiClient.ParameterToString(feedToken)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ListFeedUsers: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ListFeedUsers: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserFeedInfoPage) ApiClient.Deserialize(response.Content, typeof(UserFeedInfoPage), response.Headers);
        }
    
    }
}
