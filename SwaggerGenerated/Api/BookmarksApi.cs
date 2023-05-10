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
    public interface IBookmarksApi
    {
        /// <summary>
        /// Bookmark Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to bookmark</param>
        /// <returns>Job</returns>
        Job BookJob (long? id);
        /// <summary>
        /// Un-bookmark Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to un-bookmark</param>
        /// <returns>Job</returns>
        Job UnbookJob (long? id);
        /// <summary>
        /// View bookmarked Jobs This can only be done by the logged in user.
        /// </summary>
        /// <returns>JobInfoPage</returns>
        JobInfoPage ViewBookJob ();
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class BookmarksApi : IBookmarksApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public BookmarksApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksApi"/> class.
        /// </summary>
        /// <returns></returns>
        public BookmarksApi(String basePath)
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
        /// Bookmark Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to bookmark</param>
        /// <returns>Job</returns>
        public Job BookJob (long? id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling BookJob");
    
            var path = "/bookmarks/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling BookJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling BookJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Job) ApiClient.Deserialize(response.Content, typeof(Job), response.Headers);
        }
    
        /// <summary>
        /// Un-bookmark Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to un-bookmark</param>
        /// <returns>Job</returns>
        public Job UnbookJob (long? id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UnbookJob");
    
            var path = "/bookmarks/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UnbookJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UnbookJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Job) ApiClient.Deserialize(response.Content, typeof(Job), response.Headers);
        }
    
        /// <summary>
        /// View bookmarked Jobs This can only be done by the logged in user.
        /// </summary>
        /// <returns>JobInfoPage</returns>
        public JobInfoPage ViewBookJob ()
        {
    
            var path = "/bookmarks";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewBookJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewBookJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobInfoPage) ApiClient.Deserialize(response.Content, typeof(JobInfoPage), response.Headers);
        }
    
    }
}
