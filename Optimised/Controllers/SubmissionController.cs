using Microsoft.AspNetCore.Mvc;
using Optimised.Models;
using Optimised.Classes;

namespace Optimised.Controllers
{
    [Route("")]
    public class SubmissionController : Controller
    {
        private readonly string _connectionString;
        private readonly ReviewerManager _reviewerManager;
        private static NotificationService _notificationService = new NotificationService();
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
        public async Task<IActionResult> Submit([FromForm] string name, [FromForm] string surname, [FromForm] IFormFile file, [FromForm] string researchInstitution, [FromForm] string email)
        {
            if (Validator.ValidateFormat(name, surname, researchInstitution, file))
            {
                var submissionObject = Submission.Create(file);
                await Database.Save(submissionObject, _connectionString);

                var filteredReviewers = await _reviewerManager.GetAvailableReviewers(_connectionString, researchInstitution); 

                foreach (var reviewer in filteredReviewers)
                {
                    await reviewer.AssignReview(submissionObject.Id);
                    _notificationService.NotifyReviewer(reviewer.EmailAddress, $"Dear Reviewer with ID: {reviewer.Id},\n\nSubmission {submissionObject.Id} awaits your review.");
                }

                EvaluationManager.StartEvaluation(filteredReviewers, email);
                

                return View("Result");
            }
            else
                return View("Error");
        }




    }
}
