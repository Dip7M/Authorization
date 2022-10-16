using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Provider
{
    public class TokenProvider : ITokenProvider
    {
        private IConfiguration _config;
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(TokenProvider));

        public LoggerManager logger = new LoggerManager();
        public TokenProvider(IConfiguration config)
        {
            this._config = config;
        }
        public string GenerateJwtTokenUser(Parents user)
        {
            JwtSecurityToken token = null;
            try
            {
                logger.Info("Token Provider - User : UserDTO token requested!");
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>();
                
                 claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                 claims.Add(new Claim("RegId", user.RegId.ToString()));
                 claims.Add(new Claim("Email", user.Email));
                
                
                token = new JwtSecurityToken(
                            issuer: _config["Jwt:Issuer"],
                            audience: _config["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: credentials);
            }
            catch (Exception e)
            {
                logger.Error("Token Provider - User : Exception occured while generating tokens as " + e.Message);
                return "Exception occured while generating token as " + e.Message;
            }
            logger.Info("Token Provider - User : Token Created!");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateJwtTokenEmployee(Staff staff)
        {
            JwtSecurityToken token = null;
            try
            {
                logger.Info("Token Provider - Employee : EmployeeDTO token requested!");
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>();
               
                claims.Add(new Claim(ClaimTypes.Role, "Employee"));
                claims.Add(new Claim("Username", staff.Username));
                
                token = new JwtSecurityToken(
                            issuer: _config["Jwt:Issuer"],
                            audience: _config["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: credentials);
            }
            catch (Exception e)
            {
                logger.Error("Token Provider - Employee : Exception occured while generating tokens  as " + e.Message);
                return "Exception occured while generating token as " + e.Message;
            }
            logger.Info("Token Provider - Employee : Token Created!");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
