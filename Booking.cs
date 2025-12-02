using System.ComponentModel.DataAnnotations;

namespace homework_8.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        [Required]
        public int ResourceID {  get; set; }
        public Resource Resource { get; set; }
        [Required]
        public string UserId {  get; set; } // Employee who requested
        public DateTime RequestedTime {  get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";// Pending, Approved, Rejected
    }
}
