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
    public interface IOrganizationsApi
    {
        /// <summary>
        /// Archive Organization Archive an existent organization in social
        /// </summary>
        /// <param name="id">ID of organization to archive</param>
        /// <returns></returns>
        void ArchiveOrg (long? id);
        /// <summary>
        /// Add a new organization to the social network Add a new organization to the social network
        /// </summary>
        /// <param name="body">Create a new organization in the social network</param>
        /// <returns>NewOrganization</returns>
        NewOrganization CreateOrg (NewOrganization body);
        /// <summary>
        /// List all Organizations List all Organizations
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>OrgSystemInfoPage</returns>
        OrgSystemInfoPage ListOrg (string pageToken);
        /// <summary>
        /// Update Organization Update an existing organization by Id
        /// </summary>
        /// <param name="body">Update an existent organization in social network</param>
        /// <param name="id">ID of organization to return</param>
        /// <returns>OrgSystemInfo</returns>
        OrgSystemInfo UpdateOrg (OrgSystemInfo body, long? id);
        /// <summary>
        /// Find organization by ID Returns a single organization
        /// </summary>
        /// <param name="id">ID of organization to return</param>
        /// <returns>OrgSystemInfo</returns>
        OrgSystemInfo ViewOrg (long? id);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class OrganizationsApi : IOrganizationsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public OrganizationsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OrganizationsApi(String basePath)
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
        /// Archive Organization Archive an existent organization in social
        /// </summary>
        /// <param name="id">ID of organization to archive</param>
        /// <returns></returns>
        public void ArchiveOrg (long? id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ArchiveOrg");
    
            var path = "/organizations/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArchiveOrg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArchiveOrg: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Add a new organization to the social network Add a new organization to the social network
        /// </summary>
        /// <param name="body">Create a new organization in the social network</param>
        /// <returns>NewOrganization</returns>
        public NewOrganization CreateOrg (NewOrganization body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling CreateOrg");
    
            var path = "/organizations";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateOrg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateOrg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (NewOrganization) ApiClient.Deserialize(response.Content, typeof(NewOrganization), response.Headers);
        }
    
        /// <summary>
        /// List all Organizations List all Organizations
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>OrgSystemInfoPage</returns>
        public OrgSystemInfoPage ListOrg (string pageToken)
        {
            // verify the required parameter 'pageToken' is set
            if (pageToken == null) throw new ApiException(400, "Missing required parameter 'pageToken' when calling ListOrg");
    
            var path = "/organizations";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ListOrg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ListOrg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (OrgSystemInfoPage) ApiClient.Deserialize(response.Content, typeof(OrgSystemInfoPage), response.Headers);
        }
    
        /// <summary>
        /// Update Organization Update an existing organization by Id
        /// </summary>
        /// <param name="body">Update an existent organization in social network</param>
        /// <param name="id">ID of organization to return</param>
        /// <returns>OrgSystemInfo</returns>
        public OrgSystemInfo UpdateOrg (OrgSystemInfo body, long? id)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling UpdateOrg");
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UpdateOrg");
    
            var path = "/organizations/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "api_key", "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateOrg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateOrg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (OrgSystemInfo) ApiClient.Deserialize(response.Content, typeof(OrgSystemInfo), response.Headers);
        }
    
        /// <summary>
        /// Find organization by ID Returns a single organization
        /// </summary>
        /// <param name="id">ID of organization to return</param>
        /// <returns>OrgSystemInfo</returns>
        public OrgSystemInfo ViewOrg (long? id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ViewOrg");
    
            var path = "/organizations/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ViewOrg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewOrg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (OrgSystemInfo) ApiClient.Deserialize(response.Content, typeof(OrgSystemInfo), response.Headers);
        }
    
    }
}
