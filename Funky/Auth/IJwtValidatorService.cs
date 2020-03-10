using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Funky.Auth
{
    public interface IJwtValidatorService
    {
        Task ValidateJwt();
    }
}