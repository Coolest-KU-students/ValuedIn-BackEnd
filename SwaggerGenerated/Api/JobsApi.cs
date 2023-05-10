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
    public interface IJobsApi
    {
        /// <summary>
        /// Archive Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to archive</param>
        /// <returns></returns>
        void ArchJob (long? id);
        /// <summary>
        /// Create new Job This can only be done by HR role.
        /// </summary>
        /// <param name="body">Create a new job in the social network</param>
        /// <param name="id">ID of organization where job will be created</param>
        /// <returns>Job</returns>
        Job CreateJob (Job body, long? id);
        /// <summary>
        /// List all Jobs List all existing jobs
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>JobSystemInfoPage</returns>
        JobSystemInfoPage ListJobs (string pageToken);
        /// <summary>
        /// Update Job This can only be done by HR role.
        /// </summary>
        /// <param name="body">Update an existent job in social network</param>
        /// <param name="id">ID of job to return</param>
        /// <returns>Job</returns>
        Job UpdateJob (Job body, long? id);
        /// <summary>
        /// View Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to return</param>
        /// <returns>Job</returns>
        Job ViewJob (long? id);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class JobsApi : IJobsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public JobsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public JobsApi(String basePath)
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
        /// Archive Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to archive</param>
        /// <returns></returns>
        public void ArchJob (long? id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ArchJob");
    
            var path = "/jobs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ArchJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArchJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create new Job This can only be done by HR role.
        /// </summary>
        /// <param name="body">Create a new job in the social network</param>
        /// <param name="id">ID of organization where job will be created</param>
        /// <returns>Job</returns>
        public Job CreateJob (Job body, long? id)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling CreateJob");
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CreateJob");
    
            var path = "/jobs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Job) ApiClient.Deserialize(response.Content, typeof(Job), response.Headers);
        }
    
        /// <summary>
        /// List all Jobs List all existing jobs
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>JobSystemInfoPage</returns>
        public JobSystemInfoPage ListJobs (string pageToken)
        {
            // verify the required parameter 'pageToken' is set
            if (pageToken == null) throw new ApiException(400, "Missing required parameter 'pageToken' when calling ListJobs");
    
            var path = "/jobs";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling ListJobs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ListJobs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (JobSystemInfoPage) ApiClient.Deserialize(response.Content, typeof(JobSystemInfoPage), response.Headers);
        }
    
        /// <summary>
        /// Update Job This can only be done by HR role.
        /// </summary>
        /// <param name="body">Update an existent job in social network</param>
        /// <param name="id">ID of job to return</param>
        /// <returns>Job</returns>
        public Job UpdateJob (Job body, long? id)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling UpdateJob");
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UpdateJob");
    
            var path = "/jobs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Job) ApiClient.Deserialize(response.Content, typeof(Job), response.Headers);
        }
    
        /// <summary>
        /// View Job This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of job to return</param>
        /// <returns>Job</returns>
        public Job ViewJob (long? id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ViewJob");
    
            var path = "/jobs/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    
            // authentication setting, if any
            String[] authSettings = new String[] { "api_key", "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewJob: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewJob: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Job) ApiClient.Deserialize(response.Content, typeof(Job), response.Headers);
        }
    
    }
}
