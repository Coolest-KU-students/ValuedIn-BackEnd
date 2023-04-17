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
    public interface ICVApi
    {
        /// <summary>
        /// Upload CV for User This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of user to update</param>
        /// <param name="body"></param>
        /// <param name="additionalMetadata">Additional Metadata</param>
        /// <returns>ModelApiResponse</returns>
        ModelApiResponse UploadCV (long? id, Object body, string additionalMetadata);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CVApi : ICVApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CVApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CVApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CVApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CVApi(String basePath)
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
        /// Upload CV for User This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of user to update</param>
        /// <param name="body"></param>
        /// <param name="additionalMetadata">Additional Metadata</param>
        /// <returns>ModelApiResponse</returns>
        public ModelApiResponse UploadCV (long? id, Object body, string additionalMetadata)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UploadCV");
    
            var path = "/cv/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (additionalMetadata != null) queryParams.Add("additionalMetadata", ApiClient.ParameterToString(additionalMetadata)); // query parameter
                        postBody = ApiClient.Serialize(body); // http body (model) parameter

            // authentication setting, if any
            String[] authSettings = new String[] { "valuedIn_auth" };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UploadCV: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UploadCV: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ModelApiResponse) ApiClient.Deserialize(response.Content, typeof(ModelApiResponse), response.Headers);
        }
    
    }
}
