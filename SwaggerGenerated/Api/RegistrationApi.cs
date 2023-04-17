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
    public interface IRegistrationApi
    {
        /// <summary>
        /// Register User account Register User account
        /// </summary>
        /// <param name="body">Created user object</param>
        /// <returns>NewUser</returns>
        NewUser RegUser (NewUser body);
        /// <summary>
        /// Register User account Register User account
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <returns>NewUser</returns>
        NewUser RegUser (string login, string role, string firstName, string lastName, string password, string email, string telephone);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class RegistrationApi : IRegistrationApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public RegistrationApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationApi"/> class.
        /// </summary>
        /// <returns></returns>
        public RegistrationApi(String basePath)
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
        /// Register User account Register User account
        /// </summary>
        /// <param name="body">Created user object</param>
        /// <returns>NewUser</returns>
        public NewUser RegUser (NewUser body)
        {
            // verify the required parameter 'body' is set
            if (body == null) throw new ApiException(400, "Missing required parameter 'body' when calling RegUser");
    
            var path = "/register";
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
                throw new ApiException ((int)response.StatusCode, "Error calling RegUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling RegUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (NewUser) ApiClient.Deserialize(response.Content, typeof(NewUser), response.Headers);
        }
    
        /// <summary>
        /// Register User account Register User account
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <returns>NewUser</returns>
        public NewUser RegUser (string login, string role, string firstName, string lastName, string password, string email, string telephone)
        {
            // verify the required parameter 'login' is set
            if (login == null) throw new ApiException(400, "Missing required parameter 'login' when calling RegUser");
            // verify the required parameter 'role' is set
            if (role == null) throw new ApiException(400, "Missing required parameter 'role' when calling RegUser");
            // verify the required parameter 'firstName' is set
            if (firstName == null) throw new ApiException(400, "Missing required parameter 'firstName' when calling RegUser");
            // verify the required parameter 'lastName' is set
            if (lastName == null) throw new ApiException(400, "Missing required parameter 'lastName' when calling RegUser");
            // verify the required parameter 'password' is set
            if (password == null) throw new ApiException(400, "Missing required parameter 'password' when calling RegUser");
            // verify the required parameter 'email' is set
            if (email == null) throw new ApiException(400, "Missing required parameter 'email' when calling RegUser");
            // verify the required parameter 'telephone' is set
            if (telephone == null) throw new ApiException(400, "Missing required parameter 'telephone' when calling RegUser");
    
            var path = "/register";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                    if (login != null) formParams.Add("login", ApiClient.ParameterToString(login)); // form parameter
if (role != null) formParams.Add("role", ApiClient.ParameterToString(role)); // form parameter
if (firstName != null) formParams.Add("firstName", ApiClient.ParameterToString(firstName)); // form parameter
if (lastName != null) formParams.Add("lastName", ApiClient.ParameterToString(lastName)); // form parameter
if (password != null) formParams.Add("password", ApiClient.ParameterToString(password)); // form parameter
if (email != null) formParams.Add("email", ApiClient.ParameterToString(email)); // form parameter
if (telephone != null) formParams.Add("telephone", ApiClient.ParameterToString(telephone)); // form parameter

            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling RegUser: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling RegUser: " + response.ErrorMessage, response.ErrorMessage);
    
            return (NewUser) ApiClient.Deserialize(response.Content, typeof(NewUser), response.Headers);
        }
    
    }
}
