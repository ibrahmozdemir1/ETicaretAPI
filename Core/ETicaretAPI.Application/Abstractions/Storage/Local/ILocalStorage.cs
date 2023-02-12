using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Storage.Local
{
    public interface ILocalStorage : IStorage
    {
        Task<bool> CopyFileAsync(string path, IFormFile file);
    }
}
