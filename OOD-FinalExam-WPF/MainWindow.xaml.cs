using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOD_FinalExam_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Connect to DB
        RestaurantData _db = new RestaurantData();
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes displayed ListBox based on the date selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpBooking_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // No date selected = return
            if(dpBooking.SelectedDate == null)
            {
                return;
            }

            DateTime selectedDate = (DateTime)dpBooking.SelectedDate;

            // Search for bookings based on only the Date part of the selected date
            // All inserted bookings should have their time set to midnight
            var query = from b in _db.Bookings
                        where b.BookingsDate == selectedDate.Date
                        select b;

            // Bind the resulting list to the listbox and update capacity information
            List<Booking> bookings = query.ToList();
            lbxBooking.ItemsSource = bookings;
            CapacityUpdater(bookings);
        }

        /// <summary>
        /// Updates capacity info textblocks based on data in bookings
        /// </summary>
        /// <param name="bookings">List of bookings to evaluate</param>
        private void CapacityUpdater(List<Booking> bookings)
        {
            int maxCapacity = 40;
            int used = 0;

            // Search all bookings for total number of participants
            foreach (Booking booking in bookings)
            {
                used += booking.NumberOfParticipants;
            }

            // Evaluate taken space and free space
            tblkBookings.Text = $"Bookings {used}";
            tblkAvailable.Text = $"Availability {maxCapacity - used}";
        }
    }
}
