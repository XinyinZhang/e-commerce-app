using Core.Entities.Identity;

namespace Core.Interfaces
{
    // goal: create and return JWT tokens
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}