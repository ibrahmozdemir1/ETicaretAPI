using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.Dtos;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;
        public LoginUserCommandHandler(SignInManager<Domain.Entities.Identity.AppUser> signInManager, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser appUser = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (appUser == null)
                appUser = await _userManager.FindByEmailAsync(request.UserNameOrEmail);

            if (appUser == null)
                throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password,false);

            if(result.Succeeded) // Authentication başarılı olmuştur...
            {
                Token token = _tokenHandler.CreateAccessToken(5);
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token,
                };
            }

            throw new AuthenticationErrorException();

        }
    }
}
