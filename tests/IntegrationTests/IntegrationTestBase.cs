using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ValuedInBE;
using ValuedInBE.Contexts;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBETests.IntegrationTests
{
    public class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;
        protected readonly ValuedInContext _valuedInContext;

        public IntegrationTestBase(IntegrationTestWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
            IServiceScopeFactory scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
            _valuedInContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ValuedInContext>();
        }

        protected void AddUserIdToClient(string user)
        {
            _client.DefaultRequestHeaders.Add(TestAuthHandler.UserId, user);
        }

        protected void RemoveUserIdFromClient()
        {
            _client.DefaultRequestHeaders.Remove(TestAuthHandler.UserId);
        }

        protected static StringContent SerializeIntoJsonHttpContent<T>(T target)
        {
            return new(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
        }
    }
}
