using BusinessModel.Auth;
using BusinessModel.Common; 
using DataAccess.Auth;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Auth
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly IAuthDA _authRepository;
        private readonly IConfiguration _iconfiguration;
        public AuthBusiness(IAuthDA authRepositoryInstance, IConfiguration iconfiguration) => (_authRepository, _iconfiguration) = (authRepositoryInstance, iconfiguration);
      
        public Tokens GenerateToken(string userName) { return GenerateJWTTokens(userName); }
        public Tokens GenerateRefreshToken(string username) { return GenerateJWTTokens(username); }
        public Tokens GenerateJWTTokens(string userName)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _iconfiguration["JWT:Key"];
                var tokenKey = Encoding.UTF8.GetBytes(key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                 new Claim(ClaimTypes.Name, userName)
                  }),
                    Expires = DateTime.Now.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();

                StatusResponse Tokenupdate = _authRepository.TokenUpdate(userName, tokenHandler.WriteToken(token).ToString(), refreshToken.ToString());

                if(Tokenupdate.StatusCode!= ResponseCode.Success || Tokenupdate== null)
                {
                    return new Tokens { Access_Token = Tokenupdate.StatusDesc, Refresh_Token = Tokenupdate.StatusDesc};
                }

                return new Tokens { Access_Token = tokenHandler.WriteToken(token), Refresh_Token = refreshToken };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
            return principal;
        }
        public Tokens GetSavedRefreshTokens(String UserName,String RefreshToken)
        {
            Tokens result = new Tokens();
            var Token = _authRepository.GetRefreshtoken(UserName, RefreshToken);
            if(Token==null)                       
            return null;           
            result.Refresh_Token = Token.Refresh_Token;
            return result;
        }

    }
}
