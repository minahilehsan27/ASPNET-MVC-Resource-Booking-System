using homework_8.Data;
using homework_8.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace homework_8.Models.Repositories
{
    public class BookingRepository:IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        //Creating booking with validation
        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            if (await HasUserBookedResource(booking.ResourceID, booking.UserId))
                return false;
            var newbooking = new Booking
            {
                ResourceID = booking.ResourceID,
                UserId = booking.UserId,
                RequestedTime = DateTime.UtcNow,
                Status = "Pending"
            };
            _context.Bookings.Add(newbooking);
            await _context.SaveChangesAsync();
            return true;

        }
        //Check if user already requested same resource
        public async Task<bool> HasUserBookedResource(int resourceId, string userId)
        {
            return await _context.Bookings
                .AnyAsync(b => b.ResourceID == resourceId && b.UserId == userId);
        }
        public async Task<List<Booking>> GetUserBookingsAsync(string userId)
        {
            return _context.Bookings.Where(b => b.UserId == userId).Include(b => b.Resource).ToList();
            //.Include(b => b.Resource)->Includes resource details
        }
        //Getting pending bookings for Admin access only
        public async Task<List<Booking>> GetPendingBookingAsync()
        {
            return _context.Bookings.Where(b => b.Status == "Pending").Include(b=>b.Resource).ToList();
        }
        public async Task<bool> ApproveBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if(booking != null)
            {
                booking.Status = "Approved";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
