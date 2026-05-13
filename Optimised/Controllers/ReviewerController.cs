using Microsoft.AspNetCore.Mvc;
using Optimised.Models;
using Optimised.Classes;

namespace Optimised.Controllers
{
    [Route("[controller]")]
    public class ReviewerController : Controller
    {
        private readonly string _connectionString;
        private readonly ReviewerManager _reviewerManager;
        public ReviewerController(IConfiguration configuration)
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
        public async Task<IActionResult> SubmitReview([FromForm] long submissionId, [FromForm] double score, [FromForm] long reviewerId)
        {
            var review = Review.Create(submissionId, score, reviewerId);
            await Database.Save(review, _connectionString);
            EvaluationManager.CompleteEvaluation(review);
            return View("Result");

        }

        [HttpGet("reviewer-creation")]
        public IActionResult ReviewerCreation()
        {
            return View("ReviewerCreation");
        }

        [HttpPost("create-reviewer")]
        public async Task<IActionResult> CreateReviewer([FromForm] string name, [FromForm] string surname, [FromForm] string researchInstitution, [FromForm] bool available, [FromForm] string connectionString, [FromForm] string emailAddress)
        {
            var reviewer = Reviewer.Create(null, name, surname, researchInstitution, available, connectionString, emailAddress);
            await Database.Save(reviewer, _connectionString);

            ViewData["ReviewerId"] = reviewer.Id;
            return View("ReviewerCreationResult");

        }





    }
}
