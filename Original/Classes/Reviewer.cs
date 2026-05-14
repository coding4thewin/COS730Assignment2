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
        public long? AssignedSUbmissionId { get; set; }
        public Reviewer(long id, string name, string surname, string researchInstitution, bool available, string connectionString)
        {
            Id = id;
            Name = name;
            Surname = surname;
            ResearchInstitution = researchInstitution;
            Available = available;
            _connectionString = connectionString;

        }

        public void AssignReview(long submissionId)
        {
            AssignedSUbmissionId = submissionId;
        }
    }
}
