using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Optimised.Classes
{
    public class Review : IDatabaseObject
    {
        public long SubmissionId { get; set; }
        public double Score { get; set; }
        public long ReviewerId { get; set; }

        private Review(long submissionId, double score, long reviewerId)
        {
            SubmissionId = submissionId;
            Score = score;
            ReviewerId = reviewerId;
        }

        public static Review Create(long submissionId, double score, long reviewerId)
        {
            return new Review(submissionId, score, reviewerId);
        }

        public async Task Save(string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Review (SubmissionId, ReviewerId, Complete) VALUES (@SubmissionId, @ReviewerId, @Complete)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@SubmissionId", SubmissionId);
            command.Parameters.AddWithValue("@ReviewerId", ReviewerId);
            command.Parameters.AddWithValue("@Complete", false);

            await command.ExecuteNonQueryAsync();
        }
    }
}
