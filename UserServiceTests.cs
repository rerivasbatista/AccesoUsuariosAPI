using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoUsuariosAPI.Data;
using AccesoUsuariosAPI.Dtos;
using AccesoUsuariosAPI.Repositories;
using AccesoUsuariosAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

public class UserServiceTests
{
    private UserService CreateService(out AppDbContext context)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new AppDbContext(options);

        var repository = new UserRepository(context);

        var config = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:Key", "CLAVE_DE_PRUEBA_SUPER_SECRETA_123456789999999" }, 
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:ExpiresInMinutes", "60" }
            })
            .Build();

        return new UserService(repository, config);
    }

    [Fact]
    public async Task RegisterAsync_DebeCrearUsuarioYRetornarToken()
    {
        var service = CreateService(out var context);

        var request = new RegisterUserRequest
        {
            Name = "ramon",
            Email = "ramon@test.com",
            Password = "123456"
        };

        var result = await service.RegisterAsync(request);

        Assert.NotNull(result.Token);
        Assert.Equal("ramon@test.com", result.Email);

        var user = await context.Users.FirstOrDefaultAsync();
        Assert.True(BCrypt.Net.BCrypt.Verify("123456", user!.PasswordHash));
    }

    [Fact]
    public async Task LoginAsync_CredencialesCorrectas_DebeRetornarToken()
    {
        var service = CreateService(out var context);

        await service.RegisterAsync(new RegisterUserRequest
        {
            Name = "yobanka",
            Email = "yobanka@test.com",
            Password = "123456"
        });

        var login = new LoginRequest
        {
            Email = "yobanka@test.com",
            Password = "123456"
        };

        var result = await service.LoginAsync(login);

        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task LoginAsync_PasswordIncorrecto_DebeLanzarError()
    {
        var service = CreateService(out var context);

        await service.RegisterAsync(new RegisterUserRequest
        {
            Name = "Pedro",
            Email = "pedro@test.com",
            Password = "123456"
        });

        var login = new LoginRequest
        {
            Email = "pedro@test.com",
            Password = "incorrecta"
        };

        await Assert.ThrowsAsync<Exception>(() => service.LoginAsync(login));
    }
}
