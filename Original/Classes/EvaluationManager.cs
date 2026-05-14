using System.Runtime.CompilerServices;

namespace Original.Classes
{
    public class EvaluationManager
    {
        private static Random _random = new Random();
        private static NotificationService _notificationService = new NotificationService();

        public static async Task StartEvaluation(IEnumerable<Reviewer> reviewers, long submissionId, string researcherEmail, string connectionString, bool isUnitTest = false)
        {
            var scores = new List<double>();
            foreach (var reviewer in reviewers)
            {
                double score = SubmitScore(reviewer, isUnitTest);
                scores.Add(score);
                Database.SaveScore(reviewer, score, submissionId, connectionString);
            }

            double average = CalculateAverage(scores);
            bool hasConsensus = CheckConsensus(scores);
            string submissionStatus = ApplyRules(average, hasConsensus);

            if (submissionStatus == "Needs revision")
            {
                await _notificationService.NotifyRevision(researcherEmail, "Your submission needs revision");
            }
            else if (submissionStatus == "Rejected")
            {
                await _notificationService.NotifyRejection(researcherEmail, "Your submission was rejected");
            }
            else if (submissionStatus == "Accepted")
            {
                await _notificationService.NotifyAcceptance(researcherEmail, "Your submission was accepted");
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

        public static double SubmitScore(Reviewer reviewer, bool isUnitTest=false)
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
