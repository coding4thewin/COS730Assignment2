using Microsoft.Data.SqlClient;

namespace Original.Classes
{
    public class ReviewerManager
    {
        private readonly string _connectionString;

        public ReviewerManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

       public async Task<IEnumerable<Reviewer>> GetAvailableReviewers(string connectionString, string researchInstitution)
        {
            var reviewers = await Database.FetchReviewers(connectionString);
            reviewers = FilterConflicts(reviewers, researchInstitution);
            reviewers = CheckWorkload(reviewers);

            return reviewers;

        }

        public IEnumerable<Reviewer> FilterConflicts(IEnumerable<Reviewer> reviewers, string researchInstitution)
        {
            var filtered = new List<Reviewer>();

            foreach (var reviewer in reviewers)
            {
                if (reviewer.ResearchInstitution != researchInstitution)
                    filtered.Add(reviewer);
            }

            return filtered;
        }

        public IEnumerable<Reviewer> CheckWorkload(IEnumerable<Reviewer> reviewers)
        {
            var filtered = new List<Reviewer>();

            foreach (var reviewer in reviewers)
            {
                if (reviewer.Available)
                    filtered.Add(reviewer);
            }

            return filtered;
        }
    }
}