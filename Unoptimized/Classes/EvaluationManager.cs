using System.Runtime.CompilerServices;

namespace Original.Classes
{
    public class EvaluationManager
    {
        private static Random _random = new Random();
        private static NotificationService _notificationService = new NotificationService();

        public static void StartEvaluation(IEnumerable<Reviewer> reviewers, long submissionId, string researcherEmail, string connectionString)
        {
            var scores = new List<double>();
            foreach (var reviewer in reviewers)
            {
                double score = SubmitScore(reviewer);
                scores.Add(score);
                Database.SaveScore(reviewer, score, submissionId, connectionString);
            }

            double average = CalculateAverage(scores);
            bool hasConsensus = CheckConsensus(scores);
            string submissionStatus = ApplyRules(average, hasConsensus);

            if (submissionStatus == "Needs revision")
            {
                _notificationService.NotifyRevision(researcherEmail, "Your submission needs revision");
            }
            else if (submissionStatus == "Rejected")
            {
                _notificationService.NotifyRejection(researcherEmail, "Your submission was rejected");
            }
            else if (submissionStatus.Length > 0)
            {
                _notificationService.NotifyAcceptance(researcherEmail, "Your submission was accepted");
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

        public static double SubmitScore(Reviewer reviewer)
        {
            return _random.NextDouble() * 100;
        }
    }
}
