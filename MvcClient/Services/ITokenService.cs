using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.Services
{
    public interface ITokenService
    {
        Task<string> GetToken(string scope);
    }
}
