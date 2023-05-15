using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ValuedInBE;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBETests.IntegrationTests
{
    public class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
    {
        protected readonly Uri _clientBaseAdress;
        protected readonly HttpClient _client;
        protected readonly ValuedInContext _valuedInContext;

        public IntegrationTestBase(IntegrationTestWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _clientBaseAdress = factory.ClientOptions.BaseAddress;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.authenticationScheme);
            IServiceScopeFactory scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
            _valuedInContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ValuedInContext>();
        }

        protected void AddLoginHeaderToHttpClient(string user)
        {
            _client.DefaultRequestHeaders.Add(TestAuthHandler.userLoginHeader, user);
        }

        protected void RemoveLoginHeaderFromHttpClient()
        {
            _client.DefaultRequestHeaders.Remove(TestAuthHandler.userLoginHeader);
        }

        protected static StringContent SerializeIntoJsonHttpContent<T>(T target)
        {
            return new(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
        }

    }
}
