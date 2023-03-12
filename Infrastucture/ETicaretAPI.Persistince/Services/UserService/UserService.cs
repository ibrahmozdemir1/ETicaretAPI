using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Dtos.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistince.Services.UserService
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserDto createUserDto)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                NameSurname = createUserDto.NameSurname,
            }, createUserDto.Password);

            CreateUserResponseDto response = new() { Success = result.Succeeded };

            if (result.Succeeded)
            {
                response.Message = "Kullanıcı Başarıyla Oluşturuldu.";
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }
            }

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, string id, DateTime accessTokenDate)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddMinutes(15);

                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new NotFoundUserException();
            }
        }
    }
}
