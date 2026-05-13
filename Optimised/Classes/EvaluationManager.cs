using System.Runtime.CompilerServices;

namespace Optimised.Classes
{
    public class EvaluationManager
    {
        private static Random _random = new Random();
        private static NotificationService _notificationService = new NotificationService();
        public static HashSet<long> AsignedAndFilteredReviewerIds { get; set; }
        private static List<Review> _reviews;
        private static string _researcherEmail;

        public static void StartEvaluation(IEnumerable<Reviewer> reviewers, string researcherEmail)
        {
            AsignedAndFilteredReviewerIds = new HashSet<long>(reviewers.Select(r => r.Id.Value));
            _researcherEmail = researcherEmail;
            _reviews = new List<Review>();
        }

        public static void CompleteEvaluation(Review review)
        {
            var submittedReviewerIds = _reviews.Select(r => r.ReviewerId);
            if (submittedReviewerIds.Contains(review.ReviewerId))
                return;
            
            if (_reviews.Count() < AsignedAndFilteredReviewerIds.Count() - 1)
            {
                _reviews.Add(review);
            }
            else
            {
                var scores = _reviews.Select(r => r.Score);

                double average = CalculateAverage(scores);
                bool hasConsensus = CheckConsensus(scores);
                string submissionStatus = ApplyRules(average, hasConsensus);

                if (submissionStatus == "Needs revision")
                {
                    _notificationService.NotifyRevision(_researcherEmail, "Your submission needs revision");
                }
                else if (submissionStatus == "Rejected")
                {
                    _notificationService.NotifyRejection(_researcherEmail, "Your submission was rejected");
                }
                else if (submissionStatus.Length > 0)
                {
                    _notificationService.NotifyAcceptance(_researcherEmail, "Your submission was accepted");
                }

                //empty evaluation-related variables for next evaluation
                AsignedAndFilteredReviewerIds = new HashSet<long>();
                _reviews = new List<Review>();
                _researcherEmail = String.Empty;

            }

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
    }
}
