using api.DTOs.User;
using api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserDto request)
    {
        var reponse = await _auth.Login(request.UserName, request.Password);
        if (!reponse.Success)
            return BadRequest(reponse);
        return Ok(reponse);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserDto request)
    {
        var reponse = await _auth.Register(
            new User { UserName = request.UserName }, request.Password
        );
        if (!reponse.Success)
            return BadRequest(reponse);
        return Ok(reponse);
    }
}