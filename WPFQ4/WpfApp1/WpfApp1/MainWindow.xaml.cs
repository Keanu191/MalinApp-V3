using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CsvHelper;
using System.Linq;
using System.Data;
using System.Collections.ObjectModel;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // call ReadCSV file when the GUI loads per 4.2 assessment requirements, set missingFieldFound to null as what the exception suggested
            readCSV();
            // load dictionary into the read only listbox on start up per 4.3 assessment requirements
            displayDictionary();
            // represent parameter string as the searchInput textbox
            filterStaff(searchInput.Text);
        }

        // 4.1 Create a Dictionary data structure with a TKey of type integer and a TValue of type string, name the new data structure “MasterFile”. 
        Dictionary<int, string> MasterFile = new Dictionary<int, string>();

        

        // 4.2 Create a method that will read the data from the .csv file into the Dictionary data structure when the GUI loads.          
        private void readCSV()
        { 
            // create stream reader
            StreamReader? reader = null;
            if (File.Exists("MalinStaffNamesV3.csv"))
            {
                // read provided csv file
                reader = new StreamReader(File.OpenRead("MalinStaffNamesV3.csv"));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    // if there isn't a comma then add values
                    if (line != ",")
                    {
                        string s = values[0];
                        string t = values[1];
                        // due to the dictionary being formatted of int, string, we will have to parse S to represent the integer
                        MasterFile.Add(int.Parse(s), t);

                        // display success message
                        statusBarText.Text = "The CSV file has been loaded successfully!";
                    }
                }
            }
            else
            {
                // if CSV file wasn't found then display error message
                statusBarText.Text = "ERROR: The CSV file could not be found!";
            }
        }

        

        /*
         * 4.3 Create a method to display the Dictionary data into a non-selectable display only list box (ie read only). 
         */
        private void displayDictionary()
        {
            // clear items in the listboxes
            ListBoxReadOnly.Items.Clear();
            ListBoxSelectable.Items.Clear();

            // Set ItemsSource to the dictionary
            ListBoxReadOnly.ItemsSource = MasterFile;
            ListBoxSelectable.ItemsSource = MasterFile;
        }

        /*
         * 4.4
         * Create a method to filter the Staff Name data from the Dictionary into a second filtered and selectable list box. 
         * This method must use a text box input and update as each character is entered.
         * The list box must reflect the filtered data in real time. 
         * 
         *  4.5
         * Create a method to filter the Staff ID data from the Dictionary into the second filtered and selectable list box.
         * This method must use a text box input and update as each number is entered.
         * The list box must reflect the filtered data in real time. 
        */

    
        private void filterStaff(string staffSearch)
        {
            // call displayDictionary method
            displayDictionary();

            // set parameter string to searchInput.Text and convert the users textbox input to lowercase to prevent any issues with case sensitity if it arrises  
            staffSearch = searchInput.Text.ToLower();

            // clear listbox items
            ListBoxSelectable.Items.Clear();

            // Staff ID set as int
            int idInput;

            // Create a bool that Attempts to Parse an integer typed into the textbox to see whether the user is searching for the Staff ID or the Staff name
            bool intEntered = int.TryParse(staffSearch, out idInput);
            
            // ensure KeyValuePair Key/Value matches with the variables of the dictionary with the Key being int and string being the value
            foreach (KeyValuePair<int, string> kvp in MasterFile)
            { 
                if (intEntered == true && kvp.Key == int.Parse(staffSearch))
                {
                    // if the textbox input is an int, see if the key/value matches the search result
                    if (kvp.Key == idInput)
                    {
                        // show the Key and Value for the search result
                        ListBoxSelectable.Items.Add(kvp.Key + ": " + kvp.Value);
                        statusBarText.Text = "The following Staff ID was found! : " + staffSearch;
                    }
                    else
                    {
                        statusBarText.Text = "The following Staff ID could not be found! : " + staffSearch;
                    }

                }
                else
                {
                    // now dealing with the actual Staff Names

                    // if the name in the dictionary contains whats in the search textbox then display results
                    if (kvp.Value.ToLower().Contains(staffSearch))
                    {
                        // show the Key and Value for the search result
                        ListBoxSelectable.Items.Add(kvp.Key + ": " + kvp.Value);
                        statusBarText.Text = "The following Staff Name was found! : " + staffSearch;
                    }
                    else
                    {
                        statusBarText.Text = "The following Staff Name could not be found! : " + staffSearch;
                    }
                }
            }
        }

        private void searchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterStaff(searchInput.Text);
        }
    }
}