using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UserTestAPI.DB;
using UserTestAPI.Interfaces;
using UserTestAPI.Models;

namespace UserTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpDelete(Name = "DeleteUser")]
        public IActionResult DeleteUser([FromQuery][Required] int userId ,[FromQuery] string username = "")
        {
            try
            {
               var user = _appDbContext.Users.FirstOrDefault(u => u.Id == userId || (username != null && u.Name == username));

                _appDbContext.Users.Remove(user);

                _appDbContext.Save();
                return Ok($"user with id: {user.Id} and name: {user.Name} deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByName")]
        public ActionResult<IEnumerable<User>> GetByName([FromQuery] string name)
        {
            try
            {
                var users = _appDbContext.Users.Where(u => u.Name.Contains(name)).ToList();

                if (users == null || users.Count == 0)
                {
                    throw new Exception($"not foud users with name: {name}");
                }

                return users;

            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
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

        [HttpPut(Name = "Update")]
        public IActionResult Update([FromQuery] int id, [FromQuery] string name, [FromQuery] string email)
        {
            try
            {
                var user = _appDbContext.Users.FirstOrDefault(user => user.Id == id);
                user.Name = name;
                user.Email = email;

                _appDbContext.Users.Update(user);

                _appDbContext.Save();

                return Ok("updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"Oops: {ex.Message}");
            }
        }
    }
}