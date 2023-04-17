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
    public interface IOrganizationsFeedApi
    {
        /// <summary>
        /// List all Feed elements of Organizations List all Feed elements of Organizations
        /// </summary>
        /// <param name="feedToken"></param>
        /// <returns>OrgFeedInfoPage</returns>
        OrgFeedInfoPage ListFeedOrg (string feedToken);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class OrganizationsFeedApi : IOrganizationsFeedApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationsFeedApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public OrganizationsFeedApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationsFeedApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OrganizationsFeedApi(String basePath)
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
        /// List all Feed elements of Organizations List all Feed elements of Organizations
        /// </summary>
        /// <param name="feedToken"></param>
        /// <returns>OrgFeedInfoPage</returns>
        public OrgFeedInfoPage ListFeedOrg (string feedToken)
        {
            // verify the required parameter 'feedToken' is set
            if (feedToken == null) throw new ApiException(400, "Missing required parameter 'feedToken' when calling ListFeedOrg");
    
            var path = "/feed/organizationsfeed";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ListFeedOrg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ListFeedOrg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (OrgFeedInfoPage) ApiClient.Deserialize(response.Content, typeof(OrgFeedInfoPage), response.Headers);
        }
    
    }
}
