using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.Dtos;
using ETicaretAPI.Application.Dtos.Facebook;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;

        public FacebookLoginCommandHandler(ITokenHandler tokenHandler, UserManager<Domain.Entities.Identity.AppUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _tokenHandler = tokenHandler;
            _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id=1378991189510499&client_secret=d4ca3ff82781684126deffb4677c89df&grant_type=client_credentials");

            JsonSerializer.Deserialize<AccessTokenDto>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync(
               $"https://graph.facebook.com/oauth//debug_token?input_token={request.AuthToken}=&access_token={accessTokenResponse}");

            AccessTokenValidationDto accessToken = JsonSerializer.Deserialize<AccessTokenValidationDto>(userAccessTokenValidation);

            if (accessToken.Data.IsValid)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");

                UserInfoDto userInfoDto = JsonSerializer.Deserialize<UserInfoDto>(userInfoResponse);

                var info = new UserLoginInfo("FACEBOOK", accessToken.Data.UserId, "FACEBOOK");

                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                bool result = user != null;

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userInfoDto.Email);

                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfoDto.Email,
                            UserName = userInfoDto.Email,
                            NameSurname = userInfoDto.Name
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
                return new()
                {
                    Token = token
                };
            }
            throw new Exception("Invalid External authentication");
        }
    }
}
