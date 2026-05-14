using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Optimised.Classes
{
    public class Reviewer
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
    }
}
