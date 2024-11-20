using System;
using System.Collections.Generic;
using System.IO;
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

namespace Q7AdminGUI
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
        private SortedDictionary<int, string> masterFile;
        public AdminGUI(string action, int staffId, string staffName, SortedDictionary<int, string> MasterFile)
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
            //createNewID();
        }

        // method for making the staff ID textbox readonly
        private void readOnlyID(bool readOnly)
        {
            /*
            * 7.1
            * Create the Admin GUI with the following settings: GUI is model, all Control Box features are removed/hidden, then add two text boxes.
            * The text box for the Staff ID should be read-only for Add, Update and Delete purposes. 
            */
            if (readOnly == true)
            {
                staffIDText.IsReadOnly = true;
                
            }
            else if (readOnly == false)
            {
                staffIDText.IsReadOnly = false;
            }

        }
        /*
         * 7.2
         * Create a method that will receive the Staff ID from the General GUI and then populate text boxes with the related data.  
         */
        private void retrieveStaffData(string action, int staffId, string staffName)
        {
            // set the staff id's textbox to equal to the staffID parameter, make sure its .ToString() as its set as a int in the parameter
            staffIDText.Text = staffId.ToString();
            staffNameText.Text = staffName;

            // if the action equals to create, set the staffID textbox readonly to false
            if (action == "create")
            {
                readOnlyID(false);
            }
            // if the action equals to updateDelete, set the staffID textbox readonly to false
            else if (action == "UpdateDelete")
            {
                readOnlyID(true);
            }
        }

        /*
         * 7.3
         * Create a method that will create a new Staff ID and input the staff name from the related text box.
         * The Staff ID must be unique starting with 77xxxxxxx while the staff name may be duplicated.
         * The new staff member must be added to the SortedDictionary data structure. 
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
         * 7.4
         * Create a method that will Update the name of the current Staff ID. 
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
         * 7.5
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
         * 7.6
         * Create a method that will save changes to the csv file, this method should be called as the Admin GUI closes. 
         */
        private void saveChanges()
        {
            // Check if the file exists
            if (File.Exists("MalinStaffNamesV3.csv"))
            {
                // Open the file in append mode
                using (StreamWriter writer = new StreamWriter("MalinStaffNamesV3.csv", true))
                {
                    foreach (var entry in masterFile)
                    {
                        writer.WriteLine($"{entry.Key},{entry.Value}");
                        MessageBox.Show("The changes have been saved to the csv file!");
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
         * 7.7
         * Create a method that will close the Admin GUI when the Alt + L keys are pressed.
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
    }
}
