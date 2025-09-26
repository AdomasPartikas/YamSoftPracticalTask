using Microsoft.AspNetCore.Mvc;
using YamSoft.API.Dtos;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] UserRegisterDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (isValid, errorMessage) = Validator.IsUserDataValid(userDto);
        if (!isValid)
            return BadRequest(errorMessage);

        var authResponse = authService.Register(userDto);

        return Ok(authResponse);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] UserLoginDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (isValid, errorMessage) = Validator.IsUserDataValid(userDto);
        if (!isValid)
            return BadRequest(errorMessage);

        var authResponse = authService.Login(userDto);

        return Ok(authResponse);
    }
}

public partial class Validator
{
    public static (bool, string) IsUserDataValid(object userDto)
    {
        if (userDto is UserLoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) ||
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return (false, "All fields are required.");
            }
        }
        else if (userDto is UserRegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Username) ||
                string.IsNullOrWhiteSpace(registerDto.Password))
            {
                return (false, "All fields are required.");
            }

            if (registerDto.Password.Length < 6)
            {
                return (false, "Password must be at least 6 characters long.");
            }
        }

        return (true, string.Empty);
    }
}