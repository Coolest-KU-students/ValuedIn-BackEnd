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
    public interface IOrganizationJobsApi
    {
        /// <summary>
        /// View Organization Jobs View Organization Jobs
        /// </summary>
        /// <param name="id">ID of organization to return open job positions</param>
        /// <param name="pageToken"></param>
        /// <returns>JobInfoPage</returns>
        JobInfoPage ViewOrgJob (long? id, string pageToken);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class OrganizationJobsApi : IOrganizationJobsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationJobsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public OrganizationJobsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationJobsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OrganizationJobsApi(String basePath)
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
        /// View Organization Jobs View Organization Jobs
        /// </summary>
        /// <param name="id">ID of organization to return open job positions</param>
        /// <param name="pageToken"></param>
        /// <returns>JobInfoPage</returns>
        public JobInfoPage ViewOrgJob (long? id, string pageToken)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ViewOrgJob");
            // verify the required parameter 'pageToken' is set
            if (pageToken == null) throw new ApiException(400, "Missing required parameter 'pageToken' when calling ViewOrgJob");
    
            var path = "/organizationjobs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (pageToken != null) queryParams.Add("pageToken", ApiClient.ParameterToString(pageToken)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewOrgJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewOrgJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobInfoPage) ApiClient.Deserialize(response.Content, typeof(JobInfoPage), response.Headers);
        }
    
    }
}
