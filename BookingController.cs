using homework_8.Models;
using homework_8.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace homework_8.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            UserManager<IdentityUser> userManager)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ====================================
        // TASK 3: AJAX Request Booking
        // ====================================
        [Authorize(Roles = "Employee")]  // ✅ Only employees can book
        [HttpPost]
        [ValidateAntiForgeryToken]  // ✅ Added anti-forgery validation
        public async Task<IActionResult> RequestBooking(int resourceId)
        {
            // ✅ Get current user
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Json(new { success = false, message = "User not authenticated." });
            }

            // ✅ TASK 3 Requirement 3: Check if user already requested this resource
            if (await _bookingRepository.HasUserBookedResource(resourceId, currentUser.Id))
            {
                return Json(new { success = false, message = "You have already requested this resource." });
            }

            // ✅ Create booking
            var booking = new Booking
            {
                UserId = currentUser.Id,
                ResourceID = resourceId,
                Status = "Pending",
                RequestedTime = DateTime.Now
            };

            var result = await _bookingRepository.CreateBookingAsync(booking);

            if (result)
            {
                return Json(new { success = true, message = "✅ Booking requested successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "❌ Failed to create booking request." });
            }
        }

        // ====================================
        // TASK 4: Admin Approval Dashboard
        // ====================================
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ApprovalDashboard()
        {
            var pendingBookings = await _bookingRepository.GetPendingBookingAsync();
            return View(pendingBookings);
        }

        // ====================================
        // TASK 4: AJAX Approve Booking
        // ====================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveBooking(int bookingId)
        {
            try
            {
                var result = await _bookingRepository.ApproveBookingAsync(bookingId);

                if (result)
                {
                    // ✅ TASK 4 Requirement 3: Return partial view with updated data
                    var pendingBookings = await _bookingRepository.GetPendingBookingAsync();
                    return PartialView("_PendingBookings", pendingBookings);
                }
                else
                {
                    return Json(new { success = false, message = "Booking not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}