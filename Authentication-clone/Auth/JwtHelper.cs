using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication_clone.Auth
{
    public static class JwtHelper
    {
        public static IEnumerable<Claim> GetClaims(int userId, Role role)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),
            };

            return claims;
        }

        public static string GetToken(int userId, Role role, JwtSettings jwtSettings)
        {
            var expires = DateTime.UtcNow.AddDays(1);
            var bytesKey = System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret);
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: GetClaims(userId, role),
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(bytesKey), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string? GetJWTTokenClaim(string token, string claimName)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
                return claimValue;
            }
            catch (Exception)
            {
                //TODO: Logger.Error
                return null;
            }
        }
    }
}
