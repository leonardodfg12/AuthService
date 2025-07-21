using AuthService.Application.DTOs;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Handlers;

public class AuthHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtGenerator jwtGenerator)
{
    public async Task RegisterAsync(RegisterUserCommand command)
    {
        var existing = await userRepository.GetByEmailAsync(command.Email);
        if (existing is not null)
            throw new InvalidOperationException("Usuário já registrado.");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = command.Name,
            Email = command.Email,
            CPF = command.CPF,
            PasswordHash = passwordHasher.Hash(command.Password),
            Role = Enum.Parse<UserRole>(command.Role, ignoreCase: true)
        };

        await userRepository.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null || !passwordHasher.Verify(user.PasswordHash, request.Password))
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        return jwtGenerator.GenerateToken(user);
    }
    
    public async Task<IEnumerable<User>> ListarUsuariosAsync()
    {
        return await userRepository.GetAllAsync();
    }
}