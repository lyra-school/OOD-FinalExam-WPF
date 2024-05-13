using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Clear tbx on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbxNoOfCustomers_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxNoOfCustomers.Text = "";
        }

        /// <summary>
        /// Clear tbx on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbxName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxName.Text = "";
        }

        /// <summary>
        /// Clear tbx on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbxContact_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxContact.Text = "";
        }

        /// <summary>
        /// Adds a booking using another booking window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // do nothing if one of the textboxes is blank/invalid
            int number;
            bool parsed = int.TryParse(tbxNoOfCustomers.Text, out number);
            if(tbxName.Text == "" || tbxContact.Text == "" || tbxNoOfCustomers.Text == "" || parsed == false || dpSearch.SelectedDate == null)
            {
                return;
            }

            DateTime dt = (DateTime)dpSearch.SelectedDate;
            CustomerSearchResults selectionWin = new CustomerSearchResults();

            // update child window display with entered data
            selectionWin.tblkBookingDate.Text = $"Date of booking - {dt.Day}/{dt.Month}/{dt.Year}";
            selectionWin.tblkCustomerCount.Text = $"Numbers of customers - {number}";
            selectionWin.tbxCustName.Text = tbxName.Text;
            selectionWin.tbxCustPhone.Text = tbxContact.Text;

            // get all customers and match them against the entered name; match found = part of child window display
            Regex reg = new Regex(tbxName.Text, RegexOptions.IgnoreCase);

            var query = from c in _db.Customers
                        select c;

            List<Customer> cust = query.ToList();
            List<Customer> matchingCust = new List<Customer> ();

            foreach(Customer c in cust)
            {
                MatchCollection match = reg.Matches(c.Name);

                if(match.Count > 0)
                {
                    matchingCust.Add(c);
                }
            }
            selectionWin.lbxMatches.ItemsSource = matchingCust;

            // open window; method waits until it's closed
            selectionWin.ShowDialog();

            // get current data from child window
            string custName = selectionWin.tbxCustName.Text;
            string custPhone = selectionWin.tbxCustPhone.Text;

            // try to find customer with exact customer name and phone
            var query2 = from c in _db.Customers
                         where (c.Name == custName) &&
                         (c.ContactNumber == custPhone)
                         select c;
            List<Customer> cust2 = query2.ToList();

            // get all bookings for given date
            var query3 = from b in _db.Bookings
                        where b.BookingsDate == dt.Date
                        select b;

            List<Booking> bookings = query3.ToList();
            int used = 0;

            // Search all bookings for total number of participants
            foreach (Booking booking in bookings)
            {
                used += booking.NumberOfParticipants;
            }

            // if no customers are found, create new one + booking; otherwise just create booking. add to tables as appropriate
            // do nothing if capacity would be exceeded; makes confirmation misleading but I'm out of time
            if (cust2.Count == 0 && used + number <= 40)
            {
                Customer c = new Customer() { Name = custName, ContactNumber = custPhone };
                Booking b = new Booking() { BookingsDate = dt.Date, NumberOfParticipants = number};

                c.Bookings.Add(b);
                _db.Customers.Add(c);
                _db.SaveChanges();
            } else if(used + number <= 40)
            {
                // there SHOULDN'T be more than 1 customer in the list, but if there is, default to the first one
                Booking b = new Booking() { BookingsDate = dt.Date, NumberOfParticipants = number };
                cust2[0].Bookings.Add(b);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes selected booking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // if nothing selected, do nothing
            if(lbxBooking.SelectedItem == null)
            {
                return;
            }

            // get target booking, terminate selection and generate new list excluding it
            Booking targetBooking = (Booking)lbxBooking.SelectedItem;
            lbxBooking.SelectedItem = null;
            DateTime dt = (DateTime)dpBooking.SelectedDate;
            var query = from b in _db.Bookings
                        where (b.BookingsDate == dt.Date) &&
                        (b.BookingId != targetBooking.BookingId)
                        select b;
            List<Booking> bookings = query.ToList();

            // update window before deleting the booking
            lbxBooking.ItemsSource = bookings;
            CapacityUpdater(bookings);

            // delete booking and save
            _db.Bookings.Remove(targetBooking);
            _db.SaveChanges();
        }
    }
}
