using AuthService.Domain.Entities;

namespace AuthService.Application.Services;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}