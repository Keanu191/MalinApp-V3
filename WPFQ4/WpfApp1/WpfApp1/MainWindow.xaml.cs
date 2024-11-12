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
            // load dictionary into the listboxes on start up per 4.3 assessment requirements
            displayDictionary();
            // represent parameter string as the searchInput textbox
            filterStaff(searchInput.Text);
        }

        // 4.1 Create a Dictionary data structure with a TKey of type integer and a TValue of type string, name the new data structure “MasterFile”. 
        Dictionary<int, string> MasterFile = new Dictionary<int, string>();

        /* 12/11/2024
         * Create an observable collection to replace the dictionary itself when trying to clear items in a listbox as I get this exception relating to the ItemsSource:
         * System.InvalidOperationException: 'Operation is not valid while ItemsSource is in use. Access and modify elements with ItemsControl.ItemsSource instead.'
        */

        ObservableCollection<KeyValuePair<int, string>> dictionaryDisplay = new ObservableCollection<KeyValuePair<int, string>>();

         
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
                        MasterFile.Add(int.Parse(s), t);;

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
            // clear items in the listbox
            ListBoxReadOnly.ClearValue(ItemsControl.ItemsSourceProperty);
            ListBoxSelectable.ClearValue(ItemsControl.ItemsSourceProperty);


            // initalise the contents of the dictionary, to the observed collection (dictionary display)
            for (int i = 0; i < MasterFile.Values.Count; i++)
                dictionaryDisplay.Add(MasterFile.ElementAt(i));

            // Set ListBoxReadOnly/ListBoxSelectable ItemsSource to the dictionary
            ListBoxReadOnly.ItemsSource = dictionaryDisplay;
            ListBoxSelectable.ItemsSource = dictionaryDisplay;

            /*
             * Debugging notes:
             * When I set a breakpoint to ListBoxReadOnly.ItemsSource the Items.Count is 9926 which is about right 
             * but for the second listbox (the selectable listbox), the Items.Count is 0. On top of that there is cells but
             * they are empty!
             */
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

            /* Issue, the 2nd selectable listbox in my WPF application has empty cells
             * although the code remains the same for the other listbox which successfully loads the csv file
             * I set the ItemsSource for this listbox to the dictionary (MasterFile) 
             * When I set a breakpoint on the ListBoxSelectable.Items.Add(kvp.Key + ": " + kvp.Value);
             * the csv data successfully shows in the locals tab (kvp variable) 
             */

            //ListBoxSelectable.ClearValue(ItemsControl.ItemsSourceProperty);

            //ListBoxSelectable.ItemsSource = dictionaryDisplay;

            // set parameter string to searchInput.Text and convert the users textbox input to lowercase to prevent any issues with case sensitity if it arrises  
            staffSearch = searchInput.Text.ToLower();

            // clear listbox items
            ListBoxSelectable.ClearValue(ItemsControl.ItemsSourceProperty);

            // Staff ID set as int
            int idInput;

            // Create a bool that Attempts to Parse an integer typed into the textbox to see whether the user is searching for the Staff ID or the Staff name
            bool intEntered = int.TryParse(staffSearch, out idInput);
            
            // ensure KeyValuePair Key/Value matches with the variables of the dictionary with the Key being int and string being the value
            foreach (var kvp in dictionaryDisplay)
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

        private void ListBoxReadOnly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBoxSelectable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}