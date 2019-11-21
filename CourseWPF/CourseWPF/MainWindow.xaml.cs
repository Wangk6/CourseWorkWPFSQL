//******************************************************
// File: MainWindow.xaml.cs
//
// Purpose: Frontend GUI for the students coursework.
//          The user picks a JSON file to read from.
//
// Written By: Kevin Wang
//
// Compiler: Visual Studio 2017
//
//******************************************************

using CourseWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace CourseWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CourseWorkClass courseWork;

        static string databaseName = "CourseWork";
        static string submissionTableName = "tblSubmissions";

        public MainWindow()
        {
            InitializeComponent();
        }

        public static string ConnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InsertSubmission();
        }

        private List<Submission> GetSubmission()
        {
            using (IDbConnection connection = new SqlConnection(ConnString(databaseName)))
            {
                try
                {
                    return connection.Query<Submission>($"SELECT * FROM "+ submissionTableName).ToList();
                }
                catch (Exception ex)
                {

                }
                return null;
            }
        }

        private void InsertSubmission()
        {
            using (SqlConnection connection = new SqlConnection(ConnString(databaseName)))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(submissionTableName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@ParkingLotID", parkingLotID));
                        command.Parameters.Add(new SqlParameter("@MaxCapacity", maxCapacity));
                        command.Parameters.Add(new SqlParameter("@PermitType", permitType));
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }


        //****************************************************************
        //Name: ImportJSON_Click()
        //Purpose: When user clicks on button 'Open Course Work JSON File',
        //         opens a file dialog that allows user to select a file.
        //         The path is then displayed to textBoxFileName.
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void ImportJSON_Click(object sender, RoutedEventArgs e)
        {
            //Current working directory
            string exeDir = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //Title of file dialog
            dlg.Title = "Open Course Work From JSON";

            if (Directory.Exists(exeDir))
            {
                dlg.InitialDirectory = exeDir;
            }
            else
            {
                dlg.InitialDirectory = @"C:\";
            }
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                //Clear previous boxes
                readCourseWorkJSON(dlg.FileName);
            }
        }

        //****************************************************************
        //Name: readCourseWorkJSON()
        //Purpose: Asks the user to input a file to read and will search for
        //         a JSON file to set to the object
        //Input Type: None - setFileName for file name
        //Output Type: None
        //****************************************************************
        private void readCourseWorkJSON(string fileName)
        {
            //Open a reader using the file name given by user to read the JSON/XML
            try
            {
                FileStream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CourseWorkClass));
                courseWork = (CourseWorkClass)serializer.ReadObject(reader);
                reader.Close();
                Console.WriteLine("Reading Completed");
                showCourseDetails();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found");
            }
            catch (SerializationException)
            {
                Console.WriteLine("Wrong File Selected");
            }
        }

        //****************************************************************
        //Name: showCourseDetails()
        //Purpose: After user inputs JSON file, fill listBox Assignment
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void showCourseDetails()
        {
            //Clear ListViews then set items
            clearFields();

            //Assignments
            foreach(Assignment asn in courseWork.Assignments)
            {
                listBoxGrade.Items.Add(asn);
            }

        }

        //****************************************************************
        //Name: clearFields()
        //Purpose: Clears all fields including submissions
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void clearFields()
        {
            listBoxGrade.Items.Clear();
            clearSubmissionFields();
        }

        //****************************************************************
        //Name: clearSubmissionFields()
        //Purpose: Clears all submissions fields besides target textBox
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void clearSubmissionFields()
        {
            textBoxAssignmentName.Clear();
            textBoxCategoryName.Clear();
            textBoxGrade.Clear();
        }

        //****************************************************************
        //Name: Exit_Click()
        //Purpose: Close application
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //****************************************************************
        //Name: About_Click()
        //Purpose: About the developer
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void About_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
