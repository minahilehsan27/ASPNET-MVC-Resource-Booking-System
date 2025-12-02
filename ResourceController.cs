using homework_8.Models;
using homework_8.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace homework_8.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ResourceController(
            IResourceRepository resourceRepository,
            UserManager<IdentityUser> userManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _resourceRepository = resourceRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Only Admin can access Resource Creation Page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Form with validation + saving Admin's ID
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(Resource resource)
        {
            //Removing validation errors for server-managed fields
            ModelState.Remove("CreatedByUserId");
            ModelState.Remove("CreatedDateTime");
            ModelState.Remove("IsAvailable");

            // Check if resource name already exists
            if (await _resourceRepository.NameExistsAsync(resource.ResourceName))
            {
                ModelState.AddModelError("ResourceName", "A resource with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get current user ID
                    var user = await _userManager.GetUserAsync(User);

                    // Set server-managed properties
                    resource.CreatedByUserId = user.Id;
                    resource.CreatedDateTime = DateTime.Now;
                    resource.IsAvailable = true;

                    // Save to database
                    await _resourceRepository.CreateAsync(resource);

                    TempData["SuccessMessage"] = $"Resource '{resource.ResourceName}' created successfully!";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $" Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", $"Error: {ex.InnerException.Message}");
                    }
                }
            }

            return View(resource);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<IActionResult> List()
        { 
            var resources = await _resourceRepository.GetAllAsync();
            return View(resources);
        }
    }
}