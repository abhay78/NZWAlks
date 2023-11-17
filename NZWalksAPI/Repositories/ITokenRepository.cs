using Microsoft.AspNetCore.Identity;

namespace NZWalksAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWtToken(IdentityUser user,List<string> roles);
    }
}
