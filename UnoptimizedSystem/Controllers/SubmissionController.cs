using Microsoft.AspNetCore.Mvc;
using UnoptimizedSystem.Classes;

namespace UnoptimizedSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionController : Controller
    {
        [HttpGet]
        public string Index() 
        {
            return "Hello, World!";
        }

        [HttpPost("submit")]
        public string Submit([FromForm] SubmissionModel submission)
        {
            Console.WriteLine("submission:");
            Console.WriteLine(submission);
            return "Submission received";
        }
    }
}
