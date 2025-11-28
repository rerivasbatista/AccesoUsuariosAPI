using AccesoUsuariosAPI.Dtos;
using AccesoUsuariosAPI.Helpers;
using AccesoUsuariosAPI.Models;
using AccesoUsuariosAPI.Repositories;

namespace AccesoUsuariosAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;

    public UserService(IUserRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
    {
        var email = request.Email.ToLower();
        if (await _repository.EmailExistsAsync(email))
            throw new Exception("Correo ya registrado");

        var user = new User
        {
            Name = request.Name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _repository.AddAsync(user);

        var token = JwtHelper.Generate(user, _config);

        return new RegisterUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetByEmailAsync(request.Email.ToLower());
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Credenciales inválidas");

        var token = JwtHelper.Generate(user, _config);
        return new LoginResponse { Token = token };
    }
}
