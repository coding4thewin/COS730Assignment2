namespace Original.Models
{
    public class SubmissionModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ResearchInstitution { get; set; }
        public IFormFile File { get; set; }
        public string Email { get; set; }
    }
}
