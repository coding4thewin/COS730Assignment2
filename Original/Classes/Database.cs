using Microsoft.Data.SqlClient;
namespace Original.Classes
{
    public class Database
    {
        public static async Task<IEnumerable<Reviewer>> FetchReviewers(string connectionString)
        {
            var reviewers = new List<Reviewer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Reviewer";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Id"));
                        string name = reader.GetString(1);
                        string surname = reader.GetString(2);
                        string researchInstitution = reader.GetString(3);
                        bool available = reader.GetBoolean(4);
                        reviewers.Add(new Reviewer(id, name, surname, researchInstitution, available, connectionString));
                    }
                }
            }

            return reviewers;

        }

        public static async void SaveScore(Reviewer reviewer, double score, long submissionId, string connectionString)
        {

            using SqlConnection connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            var saveQuery = "Update Review SET Score = @Score, Complete = 1 WHERE ReviewerId = @ReviewerId AND SubmissionId = @SubmissionId";
            using SqlCommand saveCommand = new SqlCommand(saveQuery, connection);

            saveCommand.Parameters.AddWithValue("@Score", score);
            saveCommand.Parameters.AddWithValue("@ReviewerId", reviewer.Id);
            saveCommand.Parameters.AddWithValue("@SubmissionId", submissionId);
            await saveCommand.ExecuteNonQueryAsync();
        }
        public static async Task<long> SaveSubmission(IFormFile file, string connectionString)
        {


            byte[] fileBytes;
            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();


            using SqlConnection connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            var saveQuery = "INSERT INTO Submission (ResearchFile) VALUES (@FileBytes)";
            using SqlCommand saveCommand = new SqlCommand(saveQuery, connection);

            saveCommand.Parameters.AddWithValue("@FileBytes", fileBytes);
            await saveCommand.ExecuteNonQueryAsync();


            var idQuery = "SELECT TOP 1 Id FROM SUBMISSION ORDER BY Id DESC";

            using SqlCommand idCommand = new SqlCommand(idQuery, connection);
            var reader = await idCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            long Id = reader.GetInt64(0);
            return Id;

        }
    }
}
