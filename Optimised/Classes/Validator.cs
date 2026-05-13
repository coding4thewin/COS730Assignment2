using Optimised.Models;

namespace Optimised.Classes
{
    public class Validator
    {
        public static bool ValidateFormat(string name, string surname, string researchInstitution, IFormFile file)
        {
            if (name.Length == 0)
                return false;
            if (surname.Length == 0)
                return false;
            if (researchInstitution.Length == 0)
                return false;
            if (file == null)
                return false;

            return true;
        }
    }
}
