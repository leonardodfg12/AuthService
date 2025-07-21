namespace AuthService.Application.DTOs;

public class RegisterUserCommand
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string CPF { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "Cliente";
}