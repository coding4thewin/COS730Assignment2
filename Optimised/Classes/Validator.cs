namespace Optimised.Classes
{
    public class Validator
    {
        public static bool ValidateFormat(string name, string surname, string researchInstitution, IFormFile file, string email)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if (String.IsNullOrEmpty(surname))
                return false;
            if (String.IsNullOrEmpty(researchInstitution))
                return false;
            if (String.IsNullOrEmpty(email))
                return false;
            if (file == null)
                return false;

            return true;
        }
    }
}
