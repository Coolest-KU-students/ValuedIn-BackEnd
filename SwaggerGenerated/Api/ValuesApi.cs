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
    public interface IValuesApi
    {
        /// <summary>
        /// Archive Value This can only be done by the System Admin.
        /// </summary>
        /// <param name="name">Name to filter by</param>
        /// <returns></returns>
        void ArchValue (string name);
        /// <summary>
        /// List all Values List all existing values
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>ValueInfoPage</returns>
        ValueInfoPage ListValues (string pageToken);
        /// <summary>
        /// Create new Value for Sys Admin This can only be done by system admin.
        /// </summary>
        /// <param name="body">Create a new value in the social network</param>
        /// <returns>Value</returns>
        Value SysCreateValue (Value body);
        /// <summary>
        /// Update Value Update an existent value in social network
        /// </summary>
        /// <param name="body">Update an existent value in social network</param>
        /// <param name="name">Name to filter by</param>
        /// <returns>Value</returns>
        Value UpdateValues (Value body, string name);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ValuesApi : IValuesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ValuesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ValuesApi(String basePath)
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
        /// Archive Value This can only be done by the System Admin.
        /// </summary>
        /// <param name="name">Name to filter by</param>
        /// <returns></returns>
        public void ArchValue (string name)
        {
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling ArchValue");
    
            var path = "/values";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (name != null) queryParams.Add("name", ApiClient.ParameterToString(name)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ArchValue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArchValue: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// List all Values List all existing values
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>ValueInfoPage</returns>
        public ValueInfoPage ListValues (string pageToken)
        {
            // verify the required parameter 'pageToken' is set
            if (pageToken == null) throw new ApiException(400, "Missing required parameter 'pageToken' when calling ListValues");
    
            var path = "/values";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ListValues: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ListValues: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ValueInfoPage) ApiClient.Deserialize(response.Content, typeof(ValueInfoPage), response.Headers);
        }
    
        /// <summary>
        /// Create new Value for Sys Admin This can only be done by system admin.
        /// </summary>
        /// <param name="body">Create a new value in the social network</param>
        /// <returns>Value</returns>
        public Value SysCreateValue (Value body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling SysCreateValue");
    
            var path = "/values";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling SysCreateValue: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SysCreateValue: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Value) ApiClient.Deserialize(response.Content, typeof(Value), response.Headers);
        }
    
        /// <summary>
        /// Update Value Update an existent value in social network
        /// </summary>
        /// <param name="body">Update an existent value in social network</param>
        /// <param name="name">Name to filter by</param>
        /// <returns>Value</returns>
        public Value UpdateValues (Value body, string name)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling UpdateValues");
    
            var path = "/values";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (name != null) queryParams.Add("name", ApiClient.ParameterToString(name)); // query parameter
                        postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateValues: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateValues: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Value) ApiClient.Deserialize(response.Content, typeof(Value), response.Headers);
        }
    
    }
}
