using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Q5GUI;
//using WpfApp1;
//using static WpfApp1.MainWindow.SharedDictionary;
//using static WpfApp1.MainWindow;

namespace Q5GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminGUI : Window

    {
        // bool to determine if a button in the Admin GUI is clicked or not
        bool isButtonClicked = false;
        public AdminGUI(string action, int staffId, string staffName)
        {
            InitializeComponent();
            retrieveStaffData();
            createNewID();

            // method for making the staff ID textbox readonly
            void readOnlyID(bool readOnly)
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
            

            void retrieveStaffData()
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
            void createNewID()
            {
                if (isButtonClicked == true)
                {
                    // set action to create
                    action = "create";

                    if (action == "create")
                    {
                        // set the textbox to the default starting of a staff ID of 77
                        staffIDText.Text = "77";

                        // set the staffName textbox to whatever the staffName currently is as the staff name can be duplicated per assesment requirements
                        staffNameText.Text = staffName;

                        // Save the created staff member into the MasterFile dictionary
                        // staffID is int, staffName is string. It follows the same structure as the dictionary of course (int, string)
                        //SharedDictionary.MasterFile.Add(staffId, staffName);
                        
                    }
                }
            }
        }

        public void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}