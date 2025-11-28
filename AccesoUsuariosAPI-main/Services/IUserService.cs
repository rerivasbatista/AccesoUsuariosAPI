using AccesoUsuariosAPI.Dtos;
namespace AccesoUsuariosAPI.Services;

public interface IUserService
{
    Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
