using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public IConfiguration _config { get; set; }
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
             if(!ModelState.IsValid)
                return BadRequest();
            userForRegisterDto.username = userForRegisterDto.username.ToLower();
            if (await _repo.UserExist(userForRegisterDto.username))
                return BadRequest("Username already axists");
            var userToCreate = new User { Username = userForRegisterDto.username };
            var createdUser = _repo.Resister(userToCreate, userForRegisterDto.password);
            return StatusCode(201);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try{

           
            var userFromRepo = await _repo.Login(userForLoginDto.username, userForLoginDto.password);
             if(userFromRepo==null)
               return Unauthorized();
            var claims = new []{
            new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
            new Claim(ClaimTypes.Name,userFromRepo.Username)
            };
            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDecreptor= new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds
            };
            var tokenHandler=new JwtSecurityTokenHandler();
            var token=tokenHandler.CreateToken(tokenDecreptor);
            return Ok(new {
                token=tokenHandler.WriteToken(token)
            });
             }
            catch(Exception e){
                var a=e;
                return StatusCode(500);
            }
        }
    }
}