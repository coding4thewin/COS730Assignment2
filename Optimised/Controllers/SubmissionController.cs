using Microsoft.AspNetCore.Mvc;
using Optimised.Classes;

namespace Optimised.Controllers
{
    [Route("")]
    public class SubmissionController : Controller
    {
        private readonly string _connectionString;
        public SubmissionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromForm] string name, [FromForm] string surname, [FromForm] IFormFile file, [FromForm] string researchInstitution, [FromForm] string email)
        {
            if (Validator.ValidateFormat(name, surname, researchInstitution, file, email))
            {
                var submission = Submission.Create(file);
                await Database.Save(submission, _connectionString);

                await EvaluationManager.StartEvaluation(submission.Id, email, researchInstitution, _connectionString);
                

                return View("Result");
            }
            else
                return View("Error");
        }
    }
}
