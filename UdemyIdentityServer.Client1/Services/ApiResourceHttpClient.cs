using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UdemyIdentityServer.Client1.Models;

namespace UdemyIdentityServer.Client1.Services
{
    public class ApiResourceHttpClient : IApiResourceHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiResourceHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient();
            _configuration = configuration;
        }

        public async Task<HttpClient> GetHttpClient()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            _httpClient.SetBearerToken(accessToken);


            return _httpClient;
        }

        public async Task<List<string>> SaveUserViewModel(UserSaveViewModel userSaveViewModel)
        {

            var disco = await _httpClient.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);

            if (disco.IsError)
            {
                //loglama yap
            }
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();

            clientCredentialsTokenRequest.ClientId = _configuration["ClientResourceOwner:ClientId"];
            clientCredentialsTokenRequest.ClientSecret = _configuration["ClientResourceOwner:ClientSecret"];
            clientCredentialsTokenRequest.Address = disco.TokenEndpoint;

            var token = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (token.IsError)
            {
                //loglama yap
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(userSaveViewModel), Encoding.UTF8, "application/json");

            _httpClient.SetBearerToken(token.AccessToken);

            var response = await _httpClient.PostAsync("https://localhost:5001/api/user/signup", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorList = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());

                return errorList;
            }

            return null;
        }
    }
}
