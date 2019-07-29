using OwinAuthenticationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OwinAuthenticationDemo.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("api/TestMethod")]
        public string TestMethod()
        {
            return "Authorized";
        }

        [OwinAuthorize]
        [HttpGet]
        [ActionName("GetSuccess")]
        public IHttpActionResult GetSuccess()
        {
            return Ok("Request Successful");
        }

        //Add the user credentials to access the WepApi
        [HttpPost]
        [ActionName("AddApiUserAsync")]
        public async Task<IHttpActionResult> AddApiUserAsync()
        {
            var user = new User()
            {
                UserName = "Robert",
                Password = "password",
                Email = "robert@gmail.com",
                EmailConfirmed = true
            };

            using (var repo = new OwinApiAuthRepository())
            {
                await repo.CreateUserAsync(user);
            }

            return Ok();
        }

        [HttpPost]
        [ActionName("AddApiUserAsync2")]
        public async Task<IHttpActionResult> AddApiUserAsync(User userAdd)
        {
            var user = new User()
            {
                UserName = userAdd.UserName,
                Password = userAdd.Password,
                Email = userAdd.Email,
                EmailConfirmed = userAdd.EmailConfirmed
            };

            using (var repo = new OwinApiAuthRepository())
            {
                await repo.CreateUserAsync(user);
            }

            return Ok();
        }
    }
}