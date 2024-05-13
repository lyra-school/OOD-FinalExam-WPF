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
using System.Windows.Shapes;

namespace OOD_FinalExam_WPF
{
    /// <summary>
    /// Interaction logic for CustomerSearchResults.xaml
    /// </summary>
    public partial class CustomerSearchResults : Window
    {
        public CustomerSearchResults()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close window after button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Changes values of textboxes when an item is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbxMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbxMatches.SelectedItem == null)
            {
                return;
            }

            Customer cust = (Customer)lbxMatches.SelectedItem;

            tbxCustName.Text = cust.Name;
            tbxCustPhone.Text = cust.ContactNumber;
        }
    }
}
