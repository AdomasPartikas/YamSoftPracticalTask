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
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isValid, errorMessage) = Validator.IsUserDataValid(userDto);
            if (!isValid)
                return BadRequest(new { error = errorMessage });

            var authResponse = await authService.RegisterAsync(userDto);

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isValid, errorMessage) = Validator.IsUserDataValid(userDto);
            if (!isValid)
                return BadRequest(new { error = errorMessage });

            var authResponse = await authService.LoginAsync(userDto);

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
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