using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Event.Api.Models;

namespace Event.Api.Tests
{
    public class EventControllerTests : IClassFixture<TestFixture<Startup>>
    {       
        private readonly HttpClient _httpClient;

        public EventControllerTests(TestFixture<Startup> fixture)
        {
            _httpClient = fixture.Client;
        }

        [Fact]
        public async Task CreateNewEvent_PassValidAdmin_ReturnsEvent()
        {
            // Arrange
            Models.Event eventInfo = null;
            string json = @"{    
                'eventTitle': 'Community - Be a Teacher @ Kamarajar Illam',
                'eventDescription': 'Community - Be a Teacher @ Kamarajar Illam for the school students',
                'eventDate': '2020-02-15T19:24:57.602Z',
                'eventStartTime': '2020-02-15T19:24:57.602Z',
                'eventEndTime': '2020-02-15T19:24:57.602Z',
                'eventLocation': 'TNagar',
                'isTransportation': true,
                'poc': 'ram, suresh',
                'eventRegisterUsers': ['volunteer, test']
              }";
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = "/api/v1/events/CreateEvent";

            // Act
            var response = await _httpClient.PostAsync(request, httpContent);

            // Assert
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                eventInfo = JsonConvert.DeserializeObject<Event.Api.Models.Event>(responseContent);
            }

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK && eventInfo.Id != null);
        }
         
        [Fact]
        public async Task UpdateEvent_PassValidVolunteer_ReturnsEvent()
        {
            // Arrange
            Task<Models.Event> eventInfo = null;
            List<Models.Event> volunteerEvents = GetEventsVolunteer().Result;           

            string json = JsonConvert.SerializeObject(volunteerEvents[0]);

            //string json = @"{    
            //   'id': volunteerEvents[0].Id,
            //    'eventTitle': 'Community - Be a Teacher @ Govt Primary School',
            //    'eventDescription': 'Community - Be a Teacher @ Kamarajar Illam for the school students',
            //    'eventDate': '2020-02-15T19:24:57.602Z',
            //    'eventStartTime': '2020-02-15T19:24:57.602Z',
            //    'eventEndTime': '2020-02-15T19:24:57.602Z',
            //    'eventLocation': 'TNagar',
            //    'isTransportation': true,
            //    'poc': 'ram, suresh',
            //    'eventRegisterUsers': ['volunteer, test, test1']
            //  }";
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = "/api/v1/events/UpdateEvent";

            // Act
            var response = await _httpClient.PutAsync(request, httpContent);

            // Assert
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                eventInfo = JsonConvert.DeserializeObject<Task<Models.Event>>(responseContent);
            }

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK );
        }

        [Fact]
        public async Task GetEvent_All_Admin_ReturnsAllEvent()
        {
            // Arrange
            List<Models.Event> events = null;

            string token = GetJWTToken("admin", "Admin");
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("accept","application/json");
            var request = "/api/v1/events/GetAllEvents";

            // Act
            var response = await _httpClient.GetAsync(request);

            // Assert
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                events = JsonConvert.DeserializeObject<List<Event.Api.Models.Event>>(responseContent);
            }

            Assert.True(events != null && events.Count > 0);
        }

        [Fact]
        public async Task GetEvent_All_Volunteeer_ReturnsAllEvent()
        {
            // Arrange
            List<Models.Event> events = null;
           
            string token = GetJWTToken("volunteer", "Volunteer");

            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var request = "/api/v1/events/GetEventsByVolunteer?name=user";

            // Act
            var response = await _httpClient.GetAsync(request);

            // Assert
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                events = JsonConvert.DeserializeObject<List<Event.Api.Models.Event>>(responseContent);
            }

            Assert.True(events != null && events.Count > 0);
        } 
        
        private async Task<List<Models.Event>> GetEventsVolunteer()
        {
            List<Models.Event> events = null;          

            string token = GetJWTToken("volunteer", "Volunteer");

            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            var request = "/api/v1/events/GetEventsByVolunteer?name=user";

            var response =  await _httpClient.GetAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                events = JsonConvert.DeserializeObject<List<Event.Api.Models.Event>>(responseContent);
                return events;
            }
            return events;
        }

        private string GetJWTToken(string username, string Role)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenInfo = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(tokenInfo);            
            return token.ToString();
        }
    }
}
