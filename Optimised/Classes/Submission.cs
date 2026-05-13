using Microsoft.Data.SqlClient;

namespace Optimised.Classes
{
    public class Submission : IDatabaseObject
    {
        public long Id { get; set; }
        public IFormFile File { get; set; }

        private Submission(IFormFile file)
        {
            File = file;
        }

        public static Submission Create(IFormFile file)
        {
            return new Submission(file);
        }

        public async Task Save(string connectionString)
        {
            byte[] fileBytes;
            using var memoryStream = new MemoryStream();

            await File.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();


            using SqlConnection connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            var saveQuery = "INSERT INTO Submission (ResearchFile) VALUES (@FileBytes)";
            using SqlCommand saveCommand = new SqlCommand(saveQuery, connection);

            saveCommand.Parameters.AddWithValue("@FileBytes", fileBytes);
            await saveCommand.ExecuteNonQueryAsync();


            var idQuery = "SELECT TOP 1 Id FROM Submission ORDER BY Id DESC";

            using SqlCommand idCommand = new SqlCommand(idQuery, connection);
            var reader = await idCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            Id = reader.GetInt64(0);
        }
    }
}
