using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Original.Classes;

namespace Original.Controllers
{
    [Route("")]
    public class SubmissionController : Controller
    {
        private readonly string _connectionString;
        private readonly ReviewerManager _reviewerManager;
        public SubmissionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _reviewerManager = new ReviewerManager(configuration);

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromForm] string name, [FromForm] string surname, [FromForm] string researchInstitution, [FromForm] IFormFile file, [FromForm] string email)
        {
            if (Validator.ValidateFormat(name, surname, researchInstitution, file, email))
            {
                long submissionId = await Database.SaveSubmission(file, _connectionString);

                var filteredReviewers = await _reviewerManager.GetAvailableReviewers(_connectionString, researchInstitution); 

                foreach (var reviewer in filteredReviewers)
                {
                    reviewer.AssignReview(submissionId);
                }

                EvaluationManager.StartEvaluation(filteredReviewers, submissionId, email, _connectionString);
                

                return View("Result");
            }
            else
                return View("Error");
        }

       
        

       
    }
}
