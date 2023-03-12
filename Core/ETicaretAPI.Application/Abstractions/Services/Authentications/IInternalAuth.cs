using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuth
    {
        Task<Dtos.Token> LoginAsync(string usernameOrEmail,string password);
        Task<Dtos.Token> RefreshTokenLoginAsync(string refreshToken);
    }
}
