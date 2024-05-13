using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_FinalExam_WPF
{
    /// <summary>
    /// Data class for booking information
    /// </summary>
    public class Booking
    {
        // Attributes
        public int BookingId { get; set; }
        public DateTime BookingsDate { get; set; }
        public int NumberOfParticipants { get; set; }

        // Customer FK & connection
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
