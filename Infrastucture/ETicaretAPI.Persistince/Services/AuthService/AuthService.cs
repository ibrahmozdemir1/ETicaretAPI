using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.Dtos;
using ETicaretAPI.Application.Dtos.Facebook;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Services.AuthService
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly IUserService _userService;
        readonly ITokenHandler _tokenHandler;

        public AuthService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            UserManager<Domain.Entities.Identity.AppUser> userManager,
            ITokenHandler tokenHandler,
            SignInManager<AppUser> signInManager,
            IUserService userService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
        }

        async Task<Token> CreateUserExternalAsync(AppUser user,string email,string name, UserLoginInfo info)
        {
            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                }

                var createUser = await _userManager.CreateAsync(user);

                result = createUser.Succeeded;
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                throw new Exception("Invalid External authentication");

            Token token = _tokenHandler.CreateAccessToken(5);

            await _userService.UpdateRefreshToken(token.RefreshToken, user.Id, token.Expiration);

            return token;
        }

        public async Task<Token> FacebookLoginAsync(string authToken)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:clientId"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:clientSecret"]}&grant_type=client_credentials");

            AccessTokenDto? accessTokenDto = JsonSerializer.Deserialize<AccessTokenDto>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync(
               $"https://graph.facebook.com/oauth//debug_token?input_token={authToken}=&access_token={accessTokenDto?.AccessToken}");

            AccessTokenValidationDto? accessToken = JsonSerializer.Deserialize<AccessTokenValidationDto>(userAccessTokenValidation);

            if (accessToken?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                UserInfoDto? userInfoDto = JsonSerializer.Deserialize<UserInfoDto>(userInfoResponse);

                var info = new UserLoginInfo("FACEBOOK", accessToken.Data.UserId, "FACEBOOK");

                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateUserExternalAsync(user, userInfoDto.Email, userInfoDto.Name, info);

            }
            throw new Exception("Invalid External authentication");
        }

        public async Task<Token> GoogleLoginAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { $"{_configuration["ExternalLoginSettings:Google:Audince"]}" }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
 
            return await CreateUserExternalAsync(user,payload.Email, payload.Name, info);
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password)
        {
            Domain.Entities.Identity.AppUser appUser = await _userManager.FindByNameAsync(usernameOrEmail);
            if (appUser == null)
                appUser = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (appUser == null)
                throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, password, false);

            if (result.Succeeded) // Authentication başarılı olmuştur...
            {
                Token token = _tokenHandler.CreateAccessToken(5);
                await _userService.UpdateRefreshToken(token.RefreshToken, appUser.Id, token.Expiration);
                return token;
            }

            throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if(user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15);

                await _userService.UpdateRefreshToken(token.RefreshToken,user.Id, token.Expiration);

                return token;
            }

            throw new NotFoundUserException();
        }
    }
}
