using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserTestAPI.Interfaces;
using UserTestAPI.Models;

namespace UserTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IAppDbContext _appDbContext;

        public UserController(IAppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> GetAll()
        {
            try
            {
                return _appDbContext.Users
                .ToArray();
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }

        [HttpGet("GetByName")]
        public IEnumerable<User> GetByName([FromQuery] string name)
        {
            try
            {
                var users = _appDbContext.Users.Where(u => u.Name == name).ToList();

                if (users == null || users.Count == 0)
                {
                    throw new Exception($"not foud users with name: {name}");
                }

                return users;

            }
            catch (Exception ex)
            {
                return new List<User>();

                //return BadRequest(ex.Message);
            }
        }


        [HttpPost(Name = "AddUser")]
        public IActionResult Add([FromQuery] string name, [FromQuery] string email)
        {
            try
            {
                var user = new User
                {
                    Name = name,
                    Email = email,
                    CreationTime = DateTime.Now
                };

                _appDbContext.Users.Add(user);

                _appDbContext.Save();

                return Ok("created");
            }
            catch(Exception ex)
            {
                return BadRequest($"Oops: {ex.Message}");
            }
        }
    }
}