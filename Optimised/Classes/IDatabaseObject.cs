namespace Optimised.Classes
{
    public interface IDatabaseObject
    {
        public Task Save(string connectionString);
    }
}
