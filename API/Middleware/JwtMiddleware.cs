

using Microsoft.IdentityModel.Tokens;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Middleware;

public class JwtMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, token);

        await next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        int userId = -1;

        try
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var key = System.Text.Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validatedToken;
            userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            context.Items["UserId"] = userId;
        }
        catch
        {
            // Do nothing if JWT validation fails
            // User is not attached to context so request won't have access to secure routes
        }
    }
}