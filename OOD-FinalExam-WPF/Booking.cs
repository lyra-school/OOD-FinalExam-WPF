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

        /// <summary>
        /// Override used for formatting within a ListBox
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            return $"{Customer.Name} ({Customer.ContactNumber} - Party of {NumberOfParticipants})";
        }
    }
}
