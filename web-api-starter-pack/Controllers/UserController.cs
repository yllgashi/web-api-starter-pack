using Microsoft.AspNetCore.Mvc;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api_starter_pack.Controllers
{
    public class UserController : Controller
    {
        private UserService userService;
        public IActionResult Index()
        {
            return View();
        }

        [Route("users")]
        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            userService = new UserService();
            ResponseModel res = userService.Create(user);
            return Ok(res);
        }
    }
}
