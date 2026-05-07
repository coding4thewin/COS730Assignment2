using System.Runtime.CompilerServices;

namespace Unoptimized.Classes
{
    public class EvaluationManager
    {
        private static Random _random = new Random();

        public void StartEvaluation(IEnumerable<Reviewer> reviewers, long submissionId, string connectionString)
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

        }

        private double CalculateAverage(IEnumerable<double> scores)
        {
            return scores.Sum() / scores.Count();
        }

        private bool CheckConsensus(IEnumerable<double> scores)
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

        public double SubmitScore(Reviewer reviewer)
        {
            return _random.NextDouble() * 100;
        }
    }
}
