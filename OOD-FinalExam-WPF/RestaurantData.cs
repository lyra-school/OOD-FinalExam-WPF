using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_FinalExam_WPF
{
    /// <summary>
    /// Database object used to store booking and customer data
    /// </summary>
    public class RestaurantData : DbContext
    {
        // Define tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Creates a new database with the given name
        /// </summary>
        /// <param name="databaseName">Name for the new database</param>
        public RestaurantData(string databaseName) : base(databaseName) {}

        /// <summary>
        /// Empty constructor used to quickly connect to a DB of an expected name
        /// </summary>
        public RestaurantData() : base("OODExam_LyraKarsaj") { }
    }
}
