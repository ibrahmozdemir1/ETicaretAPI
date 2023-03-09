using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Dtos.User;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, 
            CancellationToken cancellationToken)
        {
            CreateUserResponseDto responseDto = await _userService.CreateAsync(new() 
            { 
                Email = request.Email,
                Password= request.Password,
                PasswordConfirm = request.PasswordConfirm,
                NameSurname= request.NameSurname,
                UserName= request.UserName,
            });

            return new()
            {
                Success = responseDto.Success,
                Message = responseDto.Message
            };
        }
    }
}
