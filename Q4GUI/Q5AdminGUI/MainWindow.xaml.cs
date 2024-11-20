using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using static System.Collections.Specialized.BitVector32;


namespace Q5AdminGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminGUI : Window
    {
        public AdminGUI()
        {

        }
        private string action;
        private int staffId;
        private string staffName;
        private Dictionary<int, string> masterFile;


        // initalise parameters with the new MainWindow for the 4.9 code
        public AdminGUI(string action, int staffId, string staffName, Dictionary<int, string> MasterFile)
        {
            InitializeComponent();

            if (action == "create")
            {
                // Clear Staff ID/Staff Name
                staffIDText.Clear();
                staffNameText.Clear();
            }
            else if (action == "UpdateDelete")
            {
                // populate with the existing staff id/name for the update and delete functions
                staffIDText.Text = staffId.ToString();
                staffNameText.Text = staffName;
            }  
            
            // Initialize the class properties with the parameters passed to the constructor
            this.action = action;
            this.staffId = staffId;
            this.staffName = staffName;
            this.masterFile = MasterFile;

            // Call createNewID() when the window is initialized
            createNewID();
            // set ID textbox to read only
            readOnlyID();

        }

        // method for making the staff ID textbox readonly
        private void readOnlyID()
        {
            /*
            * 5.1
            * Create the Admin GUI with the following settings: GUI is model, all Control Box features are removed/hidden, then add two text boxes.
            * The text box for the Staff ID should be read-only for Add, Update and Delete purposes. 
            */
            
            staffIDText.IsReadOnly = true;

        }


        private void retrieveStaffData(string action, int staffId, string staffName)
        {
            /*
            * 5.2
            * Create a method that will receive the Staff ID from the General GUI and then populate text boxes with the related data. 
            */

            // set the staff id's textbox to equal to the staffID parameter, make sure its .ToString() as its set as a int in the parameter
            staffIDText.Text = staffId.ToString();
            staffNameText.Text = staffName;

            // set staffIDText to readOnly
            readOnlyID();
        }

           /*
            * 5.3
            * Create a method that will create a new Staff ID and input the staff name from the related text box. 
            * The Staff ID must be unique starting with 77xxxxxxx while the staff name may be duplicated.
            * The new staff member must be added to the Dictionary data structure. 
            */

           private void createNewID()
           {
            
                string staffIdInput = staffIDText.Text;
                string staffNameInput = staffNameText.Text;

                int parsedId;
                if (int.TryParse(staffIdInput, out parsedId))
                {
                      // Check if the staffId already exists in the dictionary
                      if (masterFile.ContainsKey(parsedId))
                      {
                         //MessageBox.Show("ERROR: Staff ID already exists.");
                      }
                      else
                      {

                        if (staffIdInput.Contains("77"))
                        {
                        // add new staff to the dictionary data structure
                        masterFile.Add(parsedId, staffNameInput);
                        // Log parsed staffId for debugging
                        Console.WriteLine($"Created staffId: {parsedId}");
                        MessageBox.Show("Staff member successfully added!");
                        }
                        else if (!staffIdInput.Contains("77"))
                         {
                        MessageBox.Show("Staff ID is invalid, must start with 77!");
                         }
                                
                      }
                                  
                }
            }


        /*
         * 5.4 Create a method that will Update the name of the current Staff ID. 
         */
        private void updateName(int staffId, string staffName)
        {
            // call retrieveStaffData method and initalise the parameters, ensure that the action is set to UpdateDelete as this is an update method
            retrieveStaffData("UpdateDelete", staffId, staffName);


            if (masterFile.ContainsKey(staffId))
            {
                masterFile[staffId] = staffName; // Update existing value
                MessageBox.Show("Staff successfully updated!");
                Console.WriteLine($"Updated Staff Name/ID: {staffId + staffName}");
            }
            else
            {
                Console.WriteLine($"Updated Staff Name: {staffName}");
                MessageBox.Show("Error: cannot update");
            }

            // set the staffIDText to the staffID as a string, this cannot be edited
            staffIDText.Text = staffId.ToString();
            // set the staffName textbox to the staff name variable in the parameter
            staffNameText.Text = staffName;
        }

        /*
         * 5.5 
         * Create a method that will Remove the current Staff ID and clear the text boxes. 
         */

        private void removeID(int staffID, string staffName)
        {
            // call retrieveStaffData method and initalise the parameters, ensure that the action is set to UpdateDelete as this is an delete method
            retrieveStaffData("UpdateDelete", staffID, staffName);

            if (masterFile.ContainsKey(staffID))
            {
                // remove the staffID from the dictionary
                masterFile.Remove(staffID);

                // clear staff name/id textboxes
                staffNameText.Clear();
                staffIDText.Clear();

                // refocus
                staffIDText.Focus();
            }
            else
            {
                MessageBox.Show("Error cannot delete staff!");
            }
        }

        /*
         * 5.6
         * Create a method that will save changes to the csv file, this method should be called as the Admin GUI closes.
         */

        private void saveChanges()
        {
            // start the stopwatch before calling the ReadCSV method
            Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
            stopwatch.Start(); // start stopwatch

            // Check if the file exists
            if (File.Exists("MalinStaffNamesV3.csv"))
            {
                // Open the file in append mode
                using (StreamWriter writer = new StreamWriter("MalinStaffNamesV3.csv", true))
                {
                    foreach (var entry in masterFile)
                    {
                        writer.WriteLine($"{entry.Key},{entry.Value}");

                        stopwatch.Stop(); // stop stopwatch
                        TimeSpan timeTaken = stopwatch.Elapsed;
                        // display time taken in milliseconds to load CSV
                        Console.WriteLine($"{timeTaken.Milliseconds} MS for Write");
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("FATAL ERROR: THE CSV FILE IS NOT IN THE ADMIN GUI SOLUTION EXPLORER!!");
            }
        }

        /*
         * 5.7
         * Create a method that will close the Admin GUI when the Alt + L keys are pressed
         */

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // if Alt + L key is pressed
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.L))
            {
                // display messagebox with yes or no options
                if (MessageBox.Show("Are you sure you want to exit the Admin GUI?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // call saveChanges method before closing
                    saveChanges();
                    // close GUI
                    Close();
                }
                // call e.handled to prevent any further processing of any key strokes
                e.Handled = true;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
             // call create ID method when Create button is clicked
             createNewID();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // call update name method
            updateName(staffId, staffName);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // call remove staff id method
            removeID(staffId, staffName);
        }

        private void btnCreateMode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Create mode enabled!");

            // clear staff name/id textboxes
            staffNameText.Clear();
            staffIDText.Clear();

            // make staff ID text editable
            staffIDText.IsReadOnly = false;
        }

        /*
         * Not really part of assignment just reusing old code here, I want to make sure that for when searching for staff ID only an integer gets typed into the textbox
         * Source for finding this code:
         * https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
         */
        private void staffIDText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        
    }
}

