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
            readCSV(); // call ReadCSV file when the GUI loads per 4.2 assessment requirements
        }

        // 4.1 Create a Dictionary data structure with a TKey of type integer and a TValue of type string, name the new data structure “MasterFile”. 
        Dictionary<int, string> MasterFile = new Dictionary<int, string>();


        // 4.2 Create a method that will read the data from the .csv file into the Dictionary data structure when the GUI loads.          
        private void readCSV()
        {
            // probably won't work, would need to be more direct with the file location but thats for another day
            using (var reader = new StreamReader(@"C:\\Users\\noawa\\Documents\\GitHub\\MalinAT3\\WPFQ4\\WpfApp1\\WpfApp1\\MalinStaffNamesV3.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // read records from the CSV file and populate the dictionary
                var records = csv.GetRecords<Record>();

                foreach (var record in records)
                {
                    MasterFile[record.TKey] = record.TValue; // add to dictionary
                }
            }
        }
    }
}