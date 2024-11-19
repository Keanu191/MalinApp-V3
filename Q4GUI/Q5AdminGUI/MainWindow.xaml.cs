using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        }

        // method for making the staff ID textbox readonly
        private void readOnlyID(bool readOnly)
        {
            /*
            * 5.1
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


        private void retrieveStaffData(string action, int staffId, string staffName)
        {
            /*
            * 5.2
            * Create a method that will receive the Staff ID from the General GUI and then populate text boxes with the related data. 
            */

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
                         MessageBox.Show("ERROR: Staff ID already exists.");
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

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // call create ID method when Create button is clicked
            createNewID();
        }
    }
    }

