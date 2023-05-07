using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ValuedInBE.Chats.Models.DTOs.Request;
using ValuedInBE.PersonalValues.Models.DTOs.Requests;
using ValuedInBE.PersonalValues.Models.DTOs.Response;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBETests.IntegrationTests;
using ValuedInBETests.IntegrationTests.Config;
using Xunit;

namespace ValuedInBE.PersonalValues.Controllers
{
    public class PersonalValueAPIIntegrationTest : IntegrationTestBase
    {
        private const string personalValueRoute = "/api/values";
        private const string personalValueGroupRoute = "/api/values/groups/{id}";
        private const string sysAdminUserLogin = "SYS_ADMIN";
        private const string defaultUserLogin = "DEFAULT";
        private readonly List<string> _usersWithRolesThatAreNotSysAdmin = new() { defaultUserLogin, "HR", "ORG_ADMIN" };

        public PersonalValueAPIIntegrationTest(IntegrationTestWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ListAllValues_WhenWithoutSearch_ShouldReturnSomething()
        {
            AddLoginHeaderToHttpClient(defaultUserLogin);
            HttpResponseMessage response = await _client.GetAsync(personalValueRoute);
            string response3 = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            IEnumerable<PersonalValue>? personalValues =
                JsonConvert.DeserializeObject<IEnumerable<PersonalValue>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(personalValues);
            Assert.NotEmpty(personalValues);
        }


        [Fact]
        public async Task ListAllValues_WhenWithSearch_ShouldReturnValuesCorrespondingToTheSearch()
        {
            AddLoginHeaderToHttpClient(defaultUserLogin);
            string search = "hon"; //honest, honorable, etc.
            string personalValueRouteWithSearch = $"{personalValueRoute}?search={search}";
            HttpResponseMessage response = await _client.GetAsync(personalValueRouteWithSearch);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            IEnumerable<PersonalValue>? personalValues =
                JsonConvert.DeserializeObject<IEnumerable<PersonalValue>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(personalValues);
            Assert.NotEmpty(personalValues);
            Assert.True(personalValues!.All(value => value.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public async Task CreateValue_WhenUserNotSysAdmin_ShouldBeForbidden()
        {
            NewValue newValue = new() { GroupId = 1, Name = "test", Modifier = 0 };
            StringContent stringContent = SerializeIntoJsonHttpContent(newValue);
            foreach(string user in _usersWithRolesThatAreNotSysAdmin)
            {
                AddLoginHeaderToHttpClient(user);
                HttpResponseMessage response = await _client.PostAsync(personalValueRoute, stringContent);
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }
        }

        [Fact]
        public async Task CreateValue_WhenValueNotExists_ShouldSaveTheValue()
        {
            long groupId = 3;
            string name = "Humble";
            short modifier = 3;
            NewValue newValue = new() { GroupId = groupId, Name = name, Modifier = modifier };
            StringContent stringContent = SerializeIntoJsonHttpContent(newValue);
            AddLoginHeaderToHttpClient(sysAdminUserLogin);
            HttpResponseMessage response = await _client.PostAsync(personalValueRoute, stringContent);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            PersonalValue? savedValue = await _valuedInContext.Values.
                                            FirstOrDefaultAsync(
                                                value => value.GroupId == groupId 
                                                        && value.Name == name 
                                                        && value.Modifier == modifier);
            Assert.NotNull(savedValue);
        }


        [Fact]
        public async Task UpdateValue_WhenUserNotSysAdmin_ShouldBeForbidden()
        {
            UpdatedValue newValue = new() { ValueId = 1, GroupId = 1, Name = "test", Modifier = 0 };
            StringContent stringContent = SerializeIntoJsonHttpContent(newValue);
            foreach (string user in _usersWithRolesThatAreNotSysAdmin)
            {
                AddLoginHeaderToHttpClient(user);
                HttpResponseMessage response = await _client.PutAsync(personalValueRoute, stringContent);
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
                RemoveLoginHeaderFromHttpClient();
            }
        }

        [Fact]
        public async Task UpdateValue_WhenValueExists_ShouldUpdate()
        {
            long valueId = 3;
            long groupId = 2;
            string name = "Honorary";
            short modifier = 0;
            UpdatedValue updatedValue = new() { ValueId = valueId, GroupId = groupId, Name = name, Modifier = modifier };
            StringContent stringContent = SerializeIntoJsonHttpContent(updatedValue);
            AddLoginHeaderToHttpClient(sysAdminUserLogin);

            HttpResponseMessage response = await _client.PutAsync(personalValueRoute, stringContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            PersonalValue? savedValue = await _valuedInContext.Values.FirstOrDefaultAsync(value => value.Id == valueId);
            Assert.NotNull(savedValue);
            Assert.Equal(groupId, savedValue!.GroupId);
            Assert.Equal(name, savedValue.Name);
            Assert.Equal(modifier, savedValue.Modifier);
        }

        [Fact]
        public async Task UpdateValue_WhenValueNotExists_ShouldReceiveNotFound()
        {
            UpdatedValue updatedValue = new() { ValueId = 99999, GroupId = 0, Name = "name", Modifier = 0 };
            StringContent stringContent = SerializeIntoJsonHttpContent(updatedValue);
            AddLoginHeaderToHttpClient(sysAdminUserLogin);

            HttpResponseMessage response = await _client.PutAsync(personalValueRoute, stringContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact] 
        public async Task GetPersonalValuesFromGroupAsync_WhenGroupExists_ReturnvaluesFromGroup()
        {
            string valueGroupRouteWithId = personalValueGroupRoute.Replace("{id}", "1");
            AddLoginHeaderToHttpClient(defaultUserLogin);

            HttpResponseMessage response = await _client.GetAsync(valueGroupRouteWithId);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            ValuesInGroup? valuesInGroup = JsonConvert.DeserializeObject<ValuesInGroup>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(valuesInGroup);
            Assert.NotEmpty(valuesInGroup!.Values);
        }

        [Fact]
        public async Task GetPersonalValuesFromGroupAsync_WhenGroupNotExists_ReturnNotFound()
        {
            string valueGroupRouteWithId = personalValueGroupRoute.Replace("{id}", "9999");
            AddLoginHeaderToHttpClient(defaultUserLogin);

            HttpResponseMessage response = await _client.GetAsync(valueGroupRouteWithId);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
