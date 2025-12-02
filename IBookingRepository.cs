namespace homework_8.Models.Interfaces
{
    public interface IBookingRepository
    {
        //create new booking
        public Task<bool> CreateBookingAsync(Booking booking);
        //checks if user has already booked this resource or not
        public Task<bool> HasUserBookedResource(int resourceId , string userId);
        //Get all bookings for a specific user
        public Task<List<Booking>> GetUserBookingsAsync(string userId);
        public Task<List<Booking>> GetPendingBookingAsync();
        public Task<bool> ApproveBookingAsync(int bookingId);
    }
}
