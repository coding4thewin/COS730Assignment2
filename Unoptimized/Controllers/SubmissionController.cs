using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Unoptimized.Models;
using Unoptimized.Classes;

namespace Unoptimized.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public string Index()
        {
            return "Hello, World!";
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromForm] SubmissionModel submission)
        {
            if (Validator.ValidateFormat(submission))
            {
                long submissionId = await Database.SaveSubmission(submission, _connectionString);

                var filteredReviewers = await _reviewerManager.GetAvailableReviewers(_connectionString, submission.ResearchInstitution); 

                foreach (var reviewer in filteredReviewers)
                {
                    reviewer.AssignReview(submissionId);
                }

                return View("Result");
            }
            else
                return View("Error");
        }

       
        

       
    }
}
