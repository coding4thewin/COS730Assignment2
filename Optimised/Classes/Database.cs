using Microsoft.Data.SqlClient;

namespace Optimised.Classes
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
                        string emailAddress = reader.GetString(5);
                        reviewers.Add(Reviewer.Create(id, name, surname, researchInstitution, available, connectionString, emailAddress));
                    }
                }
            }

            return reviewers;

        }

        public static async Task Save(IDatabaseObject databaseObject, string connectionString)
        {
            await databaseObject.Save(connectionString);
        }
    }
}
