
using calendar_api.Models;
using calendar_api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace calendar_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthController(DataContext context, UserManager<User> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegistrationDTO req)
        {
            var userDB = await _userManager.FindByNameAsync(req.Username);
            if (userDB != null)
                return BadRequest("User already exists");

            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new()
            {
                UserName = req.Username,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            };

            var result = await _userManager.CreateAsync(user, req.Password);
            if (result.Succeeded)
            {
                string token = CreateJwtToken(user);
                return Ok(token);
            }

            return BadRequest("Server error");
        }

        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Login(UserRegistrationDTO req)
        {
            var userDB = await _userManager.FindByNameAsync(req.Username);

            if (userDB == null)
                return BadRequest("User not found");

            var isCorrectPassword = await _userManager.CheckPasswordAsync(userDB, req.Password);
            if (!isCorrectPassword)
                return BadRequest("Wrong password");

            string token = CreateJwtToken(userDB);
            return Ok(new { username = userDB.UserName, token });

            //if (!VerifyPasswordHash(req.Password, Encoding.UTF8.GetBytes(userDB.PasswordHash), Encoding.UTF8.GetBytes(userDB.PasswordSalt)))
            //    return BadRequest("Wrong password");

        }

        private string CreateJwtToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtConfig:Token").Value!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        //private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        //{
        //    using (var hmac = new HMACSHA512(passwordSalt))
        //    {
        //        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return computedHash.SequenceEqual(passwordHash);
        //    }
        //}
    }
}