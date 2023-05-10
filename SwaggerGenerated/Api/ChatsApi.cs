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
    public interface IChatsApi
    {
        /// <summary>
        /// Create Chat This can only be done by the logged in user.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Chat</returns>
        Chat CreateChat (NewChatRequest body);
        /// <summary>
        /// Create a Message in Chat This can only be done by the logged in user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body">Provide message content</param>
        /// <returns>ChatMessage</returns>
        ChatMessage CreateMsg (long? id, NewMessage body);
        /// <summary>
        /// List all Chats This can only be done by the logged in user.
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>ChatInfoPage</returns>
        ChatInfoPage ListChats (string pageToken);
        /// <summary>
        /// View Chat This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of chat to return</param>
        /// <param name="pageToken"></param>
        /// <returns>MessageDTODateTimeOffsetPage</returns>
        MessageDTODateTimeOffsetPage ViewChat (long? id, string pageToken);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ChatsApi : IChatsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ChatsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ChatsApi(String basePath)
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
        /// Create Chat This can only be done by the logged in user.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Chat</returns>
        public Chat CreateChat (NewChatRequest body)
        {
    
            var path = "/chats";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CreateChat: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateChat: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Chat) ApiClient.Deserialize(response.Content, typeof(Chat), response.Headers);
        }
    
        /// <summary>
        /// Create a Message in Chat This can only be done by the logged in user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body">Provide message content</param>
        /// <returns>ChatMessage</returns>
        public ChatMessage CreateMsg (long? id, NewMessage body)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CreateMsg");
    
            var path = "/chats/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling CreateMsg: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateMsg: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ChatMessage) ApiClient.Deserialize(response.Content, typeof(ChatMessage), response.Headers);
        }
    
        /// <summary>
        /// List all Chats This can only be done by the logged in user.
        /// </summary>
        /// <param name="pageToken"></param>
        /// <returns>ChatInfoPage</returns>
        public ChatInfoPage ListChats (string pageToken)
        {
            // verify the required parameter 'pageToken' is set
            if (pageToken == null) throw new ApiException(400, "Missing required parameter 'pageToken' when calling ListChats");
    
            var path = "/chats";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ListChats: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ListChats: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ChatInfoPage) ApiClient.Deserialize(response.Content, typeof(ChatInfoPage), response.Headers);
        }
    
        /// <summary>
        /// View Chat This can only be done by the logged in user.
        /// </summary>
        /// <param name="id">ID of chat to return</param>
        /// <param name="pageToken"></param>
        /// <returns>MessageDTODateTimeOffsetPage</returns>
        public MessageDTODateTimeOffsetPage ViewChat (long? id, string pageToken)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ViewChat");
            // verify the required parameter 'pageToken' is set
            if (pageToken == null) throw new ApiException(400, "Missing required parameter 'pageToken' when calling ViewChat");
    
            var path = "/chats/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (pageToken != null) queryParams.Add("pageToken", ApiClient.ParameterToString(pageToken)); // query parameter
                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewChat: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ViewChat: " + response.ErrorMessage, response.ErrorMessage);
    
            return (MessageDTODateTimeOffsetPage) ApiClient.Deserialize(response.Content, typeof(MessageDTODateTimeOffsetPage), response.Headers);
        }
    
    }
}
