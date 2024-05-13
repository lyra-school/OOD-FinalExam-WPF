using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_FinalExam_WPF
{
    /// <summary>
    /// Data class for customer information
    /// </summary>
    public class Customer
    {
        // Attributes
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string ContactNumber {  get; set; }

        // Each customer has a list of bookings
        public virtual List<Booking> Bookings { get; set; }

        /// <summary>
        /// Instantiates a new list of bookings upon creation
        /// </summary>
        public Customer() { 
            Bookings = new List<Booking>();
        }
    }
}
