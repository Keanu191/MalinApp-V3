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
            // clear items in the listbox
            ListBox1.Items.Clear();

            ListBox1.Items.Add();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayDictionary(); // call displayDictionary method
        }
    }
}