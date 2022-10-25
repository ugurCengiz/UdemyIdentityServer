using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyIdentityServer.Client1.Models;

namespace UdemyIdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {

            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(_config["AuthServerUrl"]);

            if (disco.IsError)
            {
                //hata ve loglama alanı
            }

            var password = new PasswordTokenRequest();

            password.Address = disco.TokenEndpoint;
            password.UserName = loginViewModel.Email;
            password.Password = loginViewModel.Password;
            password.ClientId = _config["ClientResourceOwner:ClientId"];
            password.ClientSecret = _config["ClientResourceOwner:ClientSecret"];

            var token = await client.RequestPasswordTokenAsync(password);

            if (token.IsError)
            {

                ModelState.AddModelError("", "Email veya Şifreniz yanlış");
                return View();

                //hata yakalama ve loglama
            }

            var userInfoRequest = new UserInfoRequest();

            userInfoRequest.Token = token.AccessToken;
            userInfoRequest.Address = disco.UserInfoEndpoint;

            var userInfo = await client.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                //hata yakalama ve loglama
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme,"name","role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {                
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value=
                DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)}
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,authenticationProperties);

            return RedirectToAction("Index", "User");
        }
    }
}
