using AspNetCoreGeneratedDocument;

namespace Optimised.Classes
{
    public class ReviewerManager
    {
        private readonly string _connectionString;

        public ReviewerManager(string connectionString)
        {
            _connectionString = connectionString;
        }

       public async Task<IEnumerable<Reviewer>> GetAvailableReviewers(string connectionString, string researchInstitution)
        {
            var reviewers = await Database.FetchReviewers(connectionString);
            reviewers = CheckConflictsAndWorkload(reviewers, researchInstitution);

            return reviewers;

        }

        public IEnumerable<Reviewer> CheckConflictsAndWorkload(IEnumerable<Reviewer> reviewers, string researchInstitution)
        {
            var filtered = new List<Reviewer>();

            foreach (var reviewer in reviewers)
            {
                if (reviewer.ResearchInstitution != researchInstitution && reviewer.Available)
                    filtered.Add(reviewer);
            }

            return filtered;
        }
    }
}