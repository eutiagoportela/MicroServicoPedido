using MicroServicoPedido.API.Controllers;
using MicroServicoPedido.Application.UseCases.Login.DoLogin;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly Mock<IDoLoginUseCase> _loginUseCaseMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;

    public AuthControllerTests()
    {
        _loginUseCaseMock = new Mock<IDoLoginUseCase>();
        _loggerMock = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_loginUseCaseMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        var validUser = new RequestLoginJson { UsuarioNome = "validUser", Senha = "senhaValida" };
        var response = new ResponseLoginJson { Token = "jwt-token" };

        _loginUseCaseMock.Setup(x => x.Execute(It.Is<RequestLoginJson>(r => r.UsuarioNome == validUser.UsuarioNome && r.Senha == validUser.Senha)))
                         .ReturnsAsync(response);

        var result = await _controller.LoginAsync(validUser);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ResponseLoginJson>(okResult.Value);
        Assert.Equal("jwt-token", returnValue.Token);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("usuario", null)]
    [InlineData(null, "senha123")]
    public async Task Login_ReturnsBadRequest_WhenFieldsAreMissing(string usuarionome, string senha)
    {
        var loginRequest = new RequestLoginJson { UsuarioNome = usuarionome, Senha = senha };

        var result = await _controller.LoginAsync(loginRequest);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
