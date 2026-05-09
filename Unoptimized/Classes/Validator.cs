using Original.Models;

namespace Original.Classes
{
    public class Validator
    {
        public static bool ValidateFormat(SubmissionModel submission)
        {
            if (submission.Name.Length == 0)
                return false;
            if (submission.Surname.Length == 0)
                return false;
            if (submission.ResearchInstitution.Length == 0)
                return false;
            if (submission.File == null)
                return false;

            return true;
        }
    }
}
