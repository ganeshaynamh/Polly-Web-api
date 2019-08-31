using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientDemo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientDemoController : ControllerBase
    {
        [HttpGet("getUser")]
        public ActionResult<IEnumerable<User>> Get()
        {
            User user = GetDummyData();
            return Ok(user);
        }

        private User GetDummyData()
        {
            User user = new User
            {
                Id = 1,
                Name = "jagdish parmar",
                Email = "jagdishparmarjd34@gmail.com"
            };
            return user;
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}