using MicroServicoPedido.Application.UseCases.User.RegisterUser;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using MicroServicoPedido.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class UsuariosControllerTests
{
    private readonly UsuariosController _controller;
    private readonly Mock<IRegisterUsuarioUseCase> _registerUseCaseMock;
    private readonly Mock<ILogger<UsuariosController>> _loggerMock;

    public UsuariosControllerTests()
    {
        _registerUseCaseMock = new Mock<IRegisterUsuarioUseCase>();
        _loggerMock = new Mock<ILogger<UsuariosController>>();
        _controller = new UsuariosController(_loggerMock.Object);
    }

    [Fact]
    public async Task Register_ThrowsException_ReturnsInternalServerError()
    {

        var request = new RequestRegisterUsuarioJson { NomeUsuario = "usuario", Senha = "senha123" };

        _registerUseCaseMock.Setup(x => x.Execute(request)).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await _controller.Register(_registerUseCaseMock.Object, request);

        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);

        var returnValue = Assert.IsType<ResponseErrorJson>(internalServerErrorResult.Value);
        Assert.Contains("Erro inesperado", returnValue.ErrorMessages[0]);
    }

    [Theory]
    [InlineData("novoUsuario", "senha123", "Usuário registrado com sucesso", typeof(OkObjectResult))]
    [InlineData("usuarioExistente", "senha123", "Usuário já existe", typeof(BadRequestObjectResult))]
    [InlineData("", "", "Usuário ou senha inválidos.", typeof(BadRequestObjectResult))]
    [InlineData("usuario", "senha123", "Erro inesperado", typeof(ObjectResult))]
    public async Task Register_InlineDataTestCases(string nomeUsuario, string senha, string expectedMessage, Type expectedResponseType)
    {
        var request = new RequestRegisterUsuarioJson { NomeUsuario = nomeUsuario, Senha = senha };

        if (expectedMessage == "Usuário registrado com sucesso")
        {
            var response = new ResponseRegisterUsuarioJson { Message = expectedMessage };
            _registerUseCaseMock.Setup(x => x.Execute(request)).ReturnsAsync(response);
        }
        else if (expectedMessage == "Usuário já existe")
        {
            var response = new ResponseRegisterUsuarioJson { Message = expectedMessage };
            _registerUseCaseMock.Setup(x => x.Execute(request)).ReturnsAsync(response);

            _registerUseCaseMock.Setup(x => x.Execute(It.IsAny<RequestRegisterUsuarioJson>()))
                .ReturnsAsync(response);
        }
        else if (expectedMessage == "Usuário ou senha inválidos.")
        {
            _registerUseCaseMock.Setup(x => x.Execute(request)).ReturnsAsync((ResponseRegisterUsuarioJson)null);
        }
        else if (expectedMessage == "Erro inesperado")
        {
            _registerUseCaseMock.Setup(x => x.Execute(request)).ThrowsAsync(new Exception(expectedMessage));
        }

        var result = await _controller.Register(_registerUseCaseMock.Object, request);

        Assert.IsType(expectedResponseType, result);

        if (expectedResponseType == typeof(OkObjectResult))
        {
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseRegisterUsuarioJson>(okResult.Value);
            Assert.Equal(expectedMessage, returnValue.Message);
        }
        else if (expectedResponseType == typeof(BadRequestObjectResult))
        {
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<ResponseErrorJson>(badRequestResult.Value);
            Assert.Contains(expectedMessage, returnValue.ErrorMessages);
        }
        else if (expectedResponseType == typeof(ObjectResult))
        {
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            var returnValue = Assert.IsType<ResponseErrorJson>(internalServerErrorResult.Value);
            Assert.Contains(expectedMessage, returnValue.ErrorMessages[0]);
        }
    }
}
