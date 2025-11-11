using ADC.Api.DTOs;
using ADC.Api.DTOs.Response;
using ADC.Domain.Responses;
using ADC.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost]
    public async Task<ReadOneResponse<AuthenticationResponseModel>> AuthenticateAsync([FromBody] AuthenticationDto model)
    {
        var authResponse = await authenticationService.Authenticate(model.Username, model.Password);

        if (authResponse.Response is not Responses.Success)
            return new(authResponse.Response);

        // Generar token.
        string token = authenticationService.GenerateToken(authResponse.Model.Email, "--ACTUALIZAR--");

        return new(Responses.Success, new AuthenticationResponseModel
        {
            Token = token
        });
    }
}