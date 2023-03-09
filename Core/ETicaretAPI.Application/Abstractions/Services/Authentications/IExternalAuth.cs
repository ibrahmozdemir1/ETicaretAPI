using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuth
    {
        Task<Dtos.Token> FacebookLoginAsync(string authToken);
        Task<Dtos.Token> GoogleLoginAsync(string idToken);
    }
}
