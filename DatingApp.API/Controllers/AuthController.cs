using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;

        }

    public async Task<IActionResult> Register(string username,string password){
                username=username.ToLower();
                if(await _repo.UserExist(username))
                return BadRequest("Username already axists");
                var userToCreate=new User{Username=username};
                var createdUser=_repo.Resister(userToCreate,password);
                return StatusCode(201);
                
                        
    }
    }
}