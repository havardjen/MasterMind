using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterMindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterMindController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Welcome to this super incredible amazing relatively ok MasterMind service");
        }
    }
}
