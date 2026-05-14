using System.Runtime.CompilerServices;

namespace Optimised.Classes
{
    public class EvaluationManager
    {
        private static Random _random = new Random();
        private static NotificationService _notificationService = new NotificationService();
        private static Dictionary<string, string> submissionStatusMessage = new Dictionary<string, string> { ["Accepted"] = "Your submission was accepted", ["Rejected"] = "Your submission was rejected", ["Needs revision"] = "Your submission needs revision" };
        private static ReviewerManager _reviewerManager;

        public static async Task StartEvaluation(long submissionId, string researcherEmail, string researchInstitution, string connectionString, bool isUnitTest = false)
        {
            _reviewerManager = new ReviewerManager(connectionString);

            var filteredReviewers = await _reviewerManager.GetAvailableReviewers(connectionString, researchInstitution);

            var scores = new List<double>();
            foreach (var reviewer in filteredReviewers)
            {
                double score = SubmitScore(reviewer, isUnitTest);
                scores.Add(score);
                var review = Review.Create(submissionId, score, reviewer.Id.Value);
                await Database.Save(review, connectionString);
            }

            double average = CalculateAverage(scores);
            bool hasConsensus = CheckConsensus(scores);
            string submissionStatus = ApplyRules(average, hasConsensus);

            _notificationService.NotifySubmissionStatus(submissionStatus, researcherEmail, submissionStatusMessage[submissionStatus]);
       
        }

        private static string ApplyRules(double average, bool hasConsensus)
        {
            if (!hasConsensus)
            {
                return "Needs revision";
            }
            if (average < 50)
                return "Rejected";

            return "Accepted";
        }
        private static double CalculateAverage(IEnumerable<double> scores)
        {
            return scores.Sum() / scores.Count();
        }

        private static bool CheckConsensus(IEnumerable<double> scores)
        {
            double passMark = 50.0;
            if (scores.ElementAt(0) >= passMark)
            {
                foreach (var score in scores)
                {
                    if (score < passMark)
                        return false;
                }
            }
            else
            {
                foreach (var score in scores)
                {
                    if (score >= passMark)
                        return false;
                }

            }

            return true;
        }

        public static double SubmitScore(Reviewer reviewer, bool isUnitTest = false)
        {
            if (isUnitTest)
                return _random.NextDouble() * 100;
            Console.WriteLine($"{reviewer.Name} {reviewer.Surname} (ID: {reviewer.Id})");
            while (true)
            {
                Console.Write("Enter a score between 0 and 100: ");
                if (double.TryParse(Console.ReadLine(), out double score) && 0 <= score && score <= 100)
                {
                    return score;
                }
                Console.WriteLine("Invalid score. Please enter a value between 0 and 100.");
            }
        }
    }
}
