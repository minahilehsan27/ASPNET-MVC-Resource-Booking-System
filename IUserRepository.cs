namespace homework_8.Models.Interfaces
{
    public interface IUserRepository
    {

        public Task<string> GetCurrentUserIdAsync();
    }
}
