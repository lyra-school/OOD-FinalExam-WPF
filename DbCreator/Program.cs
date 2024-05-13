using OOD_FinalExam_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbCreator
{
    internal class Program
    {
        /// <summary>
        /// Program used to populate DB with sample data
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            RestaurantData restaurantData = new RestaurantData("OODExam_LyraKarsaj");
            using(restaurantData)
            {
                // Populate customer data, then respective bookings, then add to DB and and save changes
                Customer c1 = new Customer() { Name = "Tom Jones", ContactNumber = "086-123 4567" };
                Customer c2 = new Customer() { Name = "Mary Smith", ContactNumber = "086 546 3214" };
                Customer c3 = new Customer() { Name = "Jo Doyle", ContactNumber = "087 1221 222" };

                Booking b1 = new Booking() { BookingsDate = DateTime.Now, NumberOfParticipants = 2 };
                Booking b2 = new Booking() { BookingsDate = DateTime.Now, NumberOfParticipants = 3 };
                Booking b3 = new Booking() { BookingsDate = DateTime.Now, NumberOfParticipants = 4 };

                c1.Bookings.Add(b1);
                c2.Bookings.Add(b2);
                c3.Bookings.Add(b3);
                restaurantData.Customers.Add(c1);
                restaurantData.Customers.Add(c2);
                restaurantData.Customers.Add(c3);

                restaurantData.SaveChanges();
            }
        }
    }
}
