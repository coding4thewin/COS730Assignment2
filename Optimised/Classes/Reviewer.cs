using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Optimised.Classes
{
    public class Reviewer : IDatabaseObject
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ResearchInstitution { get; set; }
        public string EmailAddress { get; set; }
        public bool Available { get; set; }
        private readonly string _connectionString;

        private Reviewer(long? id, string name, string surname, string researchInstitution, bool available, string connectionString, string emailAddress)
        {
            Id = id;
            Name = name;
            Surname = surname;
            ResearchInstitution = researchInstitution;
            Available = available;
            _connectionString = connectionString;
            EmailAddress = emailAddress;
        }

        public static Reviewer Create(long? id, string name, string surname, string researchInstitution, bool available, string connectionString, string emailAddress)
        {
            return new Reviewer(id, name, surname, researchInstitution, available, connectionString, emailAddress);
        }

        public async Task Save(string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Reviewer (Name, Surname, Institution, Available, EmailAddress) VALUES (@Name, @Surname, @Institution, @Available, @EmailAddress)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Institution", ResearchInstitution);
            command.Parameters.AddWithValue("@Available", true);
            command.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            await command.ExecuteNonQueryAsync();

            var idQuery = "SELECT TOP 1 Id FROM Reviewer ORDER BY Id DESC";

            using SqlCommand idCommand = new SqlCommand(idQuery, connection);
            var reader = await idCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            Id = reader.GetInt64(0);
        }

        public async Task AssignReview(long submissionId)
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
