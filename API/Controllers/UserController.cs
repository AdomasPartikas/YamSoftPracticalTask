using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using YamSoft.API.Dtos;

namespace YamSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController() : ControllerBase
{
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (isValid, errorMessage) = Validator.IsUserDataValid(userDto);
        if (!isValid)
            return BadRequest(errorMessage);

        return Ok(userDto);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (isValid, errorMessage) = Validator.IsUserDataValid(userDto);
        if (!isValid)
            return BadRequest(errorMessage);

        return Ok(userDto);
    }
}

public partial class Validator
{
    public static (bool, string) IsUserDataValid(UserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.Username) ||
            string.IsNullOrWhiteSpace(userDto.Password))
        {
            return (false, "All fields are required.");
        }

        if (userDto.Password.Length < 6)
        {
            return (false, "Password must be at least 6 characters long.");
        }

        return (true, string.Empty);
    }
}