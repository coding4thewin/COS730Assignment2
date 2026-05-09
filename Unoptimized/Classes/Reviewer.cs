using Microsoft.Data.SqlClient;

namespace Original.Classes
{
    public class Reviewer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ResearchInstitution { get; set; }
        public bool Available { get; set; }
        private readonly string _connectionString;

        public Reviewer(long id, string name, string surname, string researchInstitution, bool available, string connectionString)
        {
            Id = id;
            Name = name;
            Surname = surname;
            ResearchInstitution = researchInstitution;
            Available = available;
            _connectionString = connectionString;

        }

        public async void AssignReview(long submissionId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Review (SubmissionId, ReviewerId, Complete) VALUES (@SubmissionId, @ReviewerId, @Complete)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@SubmissionId", submissionId);
            command.Parameters.AddWithValue("@ReviewerId", Id);
            command.Parameters.AddWithValue("@Complete", false);

            await command.ExecuteNonQueryAsync();
        }
    }
}
