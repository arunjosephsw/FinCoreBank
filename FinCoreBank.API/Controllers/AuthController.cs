using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Interfaces.Services;
using FinCoreBank.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinCoreBank.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController
    : ControllerBase
{
    private readonly IJwtService _jwtService;

    public AuthController(
        IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(
        LoginRequest request)
    {
        if(request.UserName=="admin"
           && request.Password=="admin123")
        {
            var user =
                new User
                {
                    UserName="admin",
                    Role="Admin"
                };

            var token =
                _jwtService
                    .GenerateToken(user);

            return Ok(
                new LoginResponse
                {
                    Token=token
                });
        }

        return Unauthorized();
    }
}