using homework_8.Models;
namespace homework_8.Models.Interfaces
{
    public interface IResourceRepository
    {
        public Task<Resource> GetByIdAysnc(int id);
        //get all resources for list page
        public Task<List<Resource>> GetAllAsync();
        //Check if name exists (for validation)
        public Task<bool> NameExistsAsync(string name);
        //create new resource
        public Task CreateAsync(Resource resource);
    }
}
