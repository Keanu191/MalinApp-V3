using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
// import Admin GUI to access the Admin Window, this is for 6.9's requirements
using Q7AdminGUI;

namespace Q6GUI
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
            displaySortedDictionary();
            // represent parameter string as the searchInput textbox for staff name search
            filterStaffByName(searchInput.Text);
            // represent parameter string as searchID textbox for staff Id search
            filterStaffByID(searchID.Text);
            this.KeyDown += Grid_KeyDown;
            populateTextBox();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            // if Alt + A keys are pressed set GUI Open to true
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.A))
            {
                // call OpenGUI method
                openGUI();
            }

            // call clearstaffName/ID methods
            clearStaffName();
            clearStaffID();
        }

        // 6.1 Create a SortedDictionary data structure with a TKey of type integer and a TValue of type string, name the new data structure “MasterFile”. 
        public static SortedDictionary<int, string> MasterFile = new SortedDictionary<int, string>();

        // Create Observable collection for the requirements of 6.4
        ObservableCollection<KeyValuePair<int, string>> dictionaryDisplay = new ObservableCollection<KeyValuePair<int, string>>();

        /*
         * 6.2
         * Create a method that will read the data from the .csv file into the SortedDictionary data structure when the GUI loads. 
         */

        private void readCSV()
        {
            // create stream reader
            StreamReader reader = null;
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
                        try
                        {
                            MasterFile.Add(int.Parse(s), t);
                        }
                        catch (Exception ex)
                        {
                            statusBarText.Text = ex.Message;
                        }

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
         * 6.3
         * Create a method to display the SortedDictionary data into a non-selectable display only list box (ie read only). 
         */
        private void displaySortedDictionary()
        {
            // clear items in the readonly listbox
            ListBoxReadOnly.ClearValue(ItemsControl.ItemsSourceProperty);


            // Set ListBoxReadOnly Items source to masterfile as nothing will be changed there 
            ListBoxReadOnly.ItemsSource = MasterFile;
        }

        /*
         * 6.4
         * Create a method to filter the Staff Name data from the SortedDictionary into a second filtered and selectable list box.
         * This method must use a text box input and update as each character is entered.
         * The list box must reflect the filtered data in real time. 
         */
        private void filterStaffByName(string staffSearch)
        {
            // set parameter string to searchInput.Text and convert the users textbox input to lowercase to prevent any issues with case sensitity if it arrises  
            staffSearch = searchInput.Text.ToLower();

            // clear the observable collection
            dictionaryDisplay.Clear();

            // loop through the dictionary to find matchign entires
            foreach (var kvp in MasterFile)
            {
                // if the name in the dictionary contains whats in the search textbox then display results
                if (kvp.Value.ToLower().Contains(staffSearch))
                {
                    // show the Key and Value for the search result
                    dictionaryDisplay.Add(new KeyValuePair<int, string>(kvp.Key, kvp.Value));
                    ListBoxSelectable.ScrollIntoView(kvp.Value);
                    statusBarText.Text = $"The following Staff Name was found! : {staffSearch}";
                }
            }

            // If no items matched the search term, update the status bar text.
            if (dictionaryDisplay.Count == 0)
            {
                statusBarText.Text = $"The following Staff Name could not be found! : {staffSearch}";
            }

            // Update the ListBox's ItemsSource to the filtered collection.
            ListBoxSelectable.ItemsSource = dictionaryDisplay;
        }

        /*
         * 6.5
         * Create a method to filter the Staff ID data from the SortedDictionary into the second filtered and selectable list box.
         * This method must use a text box input and update as each number is entered.
         * The list box must reflect the filtered data in real time. 
         */
        private void filterStaffByID(string idSearch)
        {
            // set parameter string to searchID.Text  
            idSearch = searchID.Text;

            // clear the observable collection
            dictionaryDisplay.Clear();

            // Staff ID set as int
            int idInput;

            // Create a bool that Attempts to Parse an integer typed into the textbox to see whether the user is searching for the Staff ID or the Staff name
            bool intEntered = int.TryParse(idSearch, out idInput);


            // loop through the dictionary to find matchign entires
            foreach (var kvp in MasterFile)
            {
                // if the name in the dictionary contains whats in the search textbox then display results
                if (kvp.Key.ToString().Contains(idSearch))
                {
                    // show the Key and Value for the search result
                    dictionaryDisplay.Add(new KeyValuePair<int, string>(kvp.Key, kvp.Value));
                    ListBoxSelectable.ScrollIntoView(kvp.Key);
                    statusBarText.Text = $"The following Staff ID was found! : {idSearch}";
                }
            }

            // If no items matched the search term, update the status bar text.
            if (dictionaryDisplay.Count == 0)
            {
                statusBarText.Text = $"The following Staff ID could not be found! : {idSearch}";
            }

            // Update the ListBox's ItemsSource to the filtered collection.
            ListBoxSelectable.ItemsSource = dictionaryDisplay;
        }

        /*
         * 6.6
         * Create a method for the Staff Name text box which will clear the contents and place the focus into the Staff Name text box.
         * Utilise a keyboard shortcut. 
         * 
         * The keyboard combination to do this is: Delete + Enter
         * (this would be the delete underneath the insert button on the keyboard)
         */
        private void clearStaffName()
        {
            // Check if the Delete key and Enter Key are pressed at the same time
            if (Keyboard.IsKeyDown(Key.Enter) && Keyboard.IsKeyDown(Key.Delete))
            {
                // clear the staff name textboxes contents
                searchInput.Clear();

                // place focus into the staff name textbox
                searchInput.Focus();

                // display confirmation message in status bar that the staff's name textbox has been successfully cleared
                statusBarText.Text = "The staff name textbox has been cleared!";
            }

            return;
        }

        /*
         * 6.7
         * Create a method for the Staff ID text box which will clear the contents and place the focus into the text box.
         * Utilise a keyboard shortcut. 
         * 
         * The keyboard combination to do this is: Delete + Left Shift
         * (this would be the delete underneath the insert button on the keyboard)
         */

        private void clearStaffID()
        {
            // Check if the Delete key and Left Shift Key are pressed at the same time
            if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.Delete))
            {
                // clear the staff name textboxes contents
                searchID.Clear();

                // place focus into the staff name textbox
                searchID.Focus();

                // display confirmation message in status bar that the staff's name textbox has been successfully cleared
                statusBarText.Text = "The staff ID textbox has been cleared!";
            }

            return;
        }

        /*
         * 6.8
         * Create a method for the filtered and selectable list box which will populate the two text boxes when a staff record is selected.
         * Utilise the Tab and keyboard keys. 
         * 
         * keyboard combination is Tab + Left Shift and then select any item in the listbox
         */
        private void populateTextBox()
        {
            if (Keyboard.IsKeyDown(Key.Tab) && Keyboard.IsKeyDown(Key.LeftShift))
            {
                // for ReadOnly Listbox
                // if the selected item in the listbox exists
                if (ListBoxReadOnly.SelectedItem != null)
                {
                    // initalise the selected item as a kvp of type int string like the dictionary then assign
                    var selectedItem = (KeyValuePair<int, string>)ListBoxReadOnly.SelectedItem; // Assign the ID to the first TextBox
                    searchID.Text = selectedItem.Key.ToString(); // Assign the name to the second TextBox
                    searchInput.Text = selectedItem.Value;
                }

                // for Selectable Listbox
                // if the selected item in the listbox exists
                if (ListBoxSelectable.SelectedItem != null)
                {
                    // initalise the selected item as a kvp of type int string like the dictionary then assign
                    var selectedItem = (KeyValuePair<int, string>)ListBoxSelectable.SelectedItem; // Assign the ID to the first TextBox
                    searchID.Text = selectedItem.Key.ToString(); // Assign the name to the second TextBox
                    searchInput.Text = selectedItem.Value;
                }
            }
        }

        /*
         * 6.9
         * Create a method that will open the Admin GUI when the Alt + A keys are pressed. 
         * Ensure the General GUI sends the currently selected Staff ID and Staff Name to the Admin GUI for Update and Delete purposes and is opened as modal.
         * Create modified logic to open the Admin GUI to Create a new user when the Staff ID 77 and the Staff Name is empty.
         * Read the appropriate criteria in the Admin GUI for further information. 
         */

        // get selected Staff ID method
        private int GetSelectedStaffId()
        {
            if (ListBoxReadOnly.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                return selectedItem.Key;
            }
            return 0; // Default value if nothing is selected
        }

        // get selected Staff name method
        private string GetSelectedStaffName()
        {
            if (ListBoxReadOnly.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                return selectedItem.Value;
            }
            return string.Empty; // Default value if nothing is selected
        }

        private void openGUI()
        {
            // create a bool to determine whether an item in the listboxes are selected or not
            bool itemSelected;

            // if else statements for readonly listbox (1st listbox)
            // to get intended results make sure in the future to make sure to do listBox.SelectedItem == null instead of ListBox.SelectedItem is false
            if (ListBoxReadOnly.SelectedItem == null)
            {
                itemSelected = false;
                statusBarText.Text = "Error: Cannot open Admin GUI, an item in the read only listbox must be selected!";
                return;
            }
            else
            {
                itemSelected = true;
                if (itemSelected == true)
                {
                    // Get the currently selected Staff ID and Staff Name
                    var selectedStaffId = GetSelectedStaffId();
                    var selectedStaffName = GetSelectedStaffName();
                    var dictionary = MasterFile;

                    // Determine action based on criteria
                    // create
                    if (selectedStaffId == 77 && string.IsNullOrWhiteSpace(selectedStaffName))
                    {
                        // Open Admin GUI for Create action
                        // initalise the project reference parameters string action, int staffId, string staffName
                        var adminGui = new AdminGUI("Create", selectedStaffId, selectedStaffName, dictionary);
                        adminGui.ShowDialog();
                    }
                    // update delete
                    else
                    {
                        // Open Admin GUI for Update/Delete action, passing Staff ID and Name
                        // initalise the project reference parameters string action, int staffId, string staffName
                        var adminGui = new AdminGUI("UpdateDelete", selectedStaffId, selectedStaffName, dictionary);
                        adminGui.ShowDialog();
                    }
                    statusBarText.Text = "Admin GUI has been successfully opened!";
                }

            }

            // if else statements for selectable listbox (2nd listbox)

            if (ListBoxSelectable.SelectedItem == null)
            {
                itemSelected = false;
                statusBarText.Text = "Error: Cannot open Admin GUI, an item in the selectable listbox must be selected!";
            }
            else
            {
                itemSelected = true;
                if (itemSelected == true)
                {
                    // Get the currently selected Staff ID and Staff Name
                    var selectedStaffId = GetSelectedStaffId();
                    var selectedStaffName = GetSelectedStaffName();
                    var dictionary = MasterFile;

                    // Determine action based on criteria
                    // create
                    if (selectedStaffId == 77 && string.IsNullOrWhiteSpace(selectedStaffName))
                    {
                        // Open Admin GUI for Create action
                        // initalise the project reference parameters string action, int staffId, string staffName
                        var adminGui = new AdminGUI("Create", selectedStaffId, null, dictionary);
                        adminGui.ShowDialog();
                    }
                    // update delete
                    else
                    {
                        // Open Admin GUI for Update/Delete action, passing Staff ID and Name
                        // initalise the project reference parameters string action, int staffId, string staffName
                        var adminGui = new AdminGUI("UpdateDelete", selectedStaffId, selectedStaffName, dictionary);
                        adminGui.ShowDialog();
                    }
                    statusBarText.Text = "Admin GUI has been successfully opened!";
                }
            }
        }

        /*
         * Not really part of assignment just reusing old code here, I want to make sure that for when searching for staff ID only an integer gets typed into the textbox
         * Source for finding this code:
         * https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
         */
        private void searchID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ListBoxReadOnly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            populateTextBox();
        }

        private void ListBoxSelectable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            populateTextBox();
        }

        private void searchID_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterStaffByID(searchID.Text);
        }

        private void searchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
           filterStaffByName(searchInput.Text);
        }
    }
}
