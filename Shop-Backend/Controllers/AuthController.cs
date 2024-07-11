using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop_Backend.JWT;
using Shop_Backend.Models;
using Shop_Backend.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IUserRepository userRepository, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid request. Username and password are required.");
            }

            var authenticatedUser = _userRepository.GetUser(user.Username, user.Password);

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey ?? throw new InvalidOperationException("JWT key is not configured."));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, authenticatedUser.Username ?? throw new InvalidOperationException("User username is null.")) 
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return _userRepository.GetUsers();
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid request. Username and password are required.");
            }

            if (_userRepository.GetUser(user.Username, user.Password) != null)
            {
                return BadRequest("User already exists.");
            }

            _userRepository.AddUser(user);
            return Ok("User registered successfully.");
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] User user)
        {
            var existingUser = _userRepository.GetUserById(user.Id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            _userRepository.UpdateUser(user);
            return Ok("User updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            _userRepository.DeleteUser(id);
            return Ok("User deleted successfully.");
        }
    }
}

