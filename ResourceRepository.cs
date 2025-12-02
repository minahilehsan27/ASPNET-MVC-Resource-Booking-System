using homework_8.Data;
using homework_8.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace homework_8.Models.Repositories
{
    public class ResourceRepository:IResourceRepository
    {
        private readonly ApplicationDbContext _context;//(database connection)we need this to change the database.
        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Resource resource) 
        {
            _context.Resources.Add(resource);//wanna enter resource.
            await _context.SaveChangesAsync();//it is not similar to migartions,as in migration we update the database where as  here there's happening change in table,here it is just executing a SQL query. 
            //Migrations (One-time setup),SaveChangesAsync (Frequent operations).
            //how actuaaly happens in database
        }
        public async Task<Resource> GetByIdAysnc(int id)
        { 
            return await _context.Resources.FirstOrDefaultAsync(r => r.ResourceID == id);
            //FirstOrDefaultAsync() is a database query method,which means "Find the FIRST resource that matches this condition, or return NULL if nothing found."
            //// SQL Generated: 
            // SELECT TOP 1 * FROM Resources WHERE ResourceID = 5
        }
        public async Task<List<Resource>> GetAllAsync()
        {
            return await _context.Resources.OrderByDescending(r=>r.CreatedDateTime).ToListAsync();
            //ToListAsync():"Go to the database, run this query, and return ALL the results as a List."
        }
        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Resources.AnyAsync(r => r.ResourceName.ToLower() == name.ToLower());
            //AnyAsync() is used here because it's the most efficient way to check if something exists in the database.
            //Any():Existence check,if yes return true else false.
        }
        public async Task<Resource> GetByIdAsync(int id)
        {
            return await _context.Resources.FindAsync(id);
        }

    }
}
