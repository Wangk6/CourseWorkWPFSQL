//******************************************************
// File: MainWindow.xaml.cs
//
// Purpose: Frontend GUI for the students coursework.
//          The user picks a JSON file to read from.
//          Uploads to SQL Server and reads from SQL.
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
using System.Collections.ObjectModel;

namespace CourseWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CourseWorkClass courseWork;
        ObservableCollection<Submission> Submissions;

        static string databaseName = "CourseWork";
        static string submissionTableName = "tblSubmissions";

        public MainWindow()
        {
            InitializeComponent();
         
        }

        //Connection string that is placed in config file, better alternative than displaying in code
        public static string ConnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetSubmission();
        }

        //****************************************************************
        //Name: GetSubmission()
        //Purpose: Get data from SQL, clear fields,
        //         fill listBox Assignment
        //Input Type: None
        //Output Type: None
        //****************************************************************

        private void GetSubmission()
        {
            string sql = $"SELECT * FROM " + submissionTableName;
            using (SqlConnection connection = new SqlConnection(ConnString(databaseName)))
            {
                try
                {
                    Submissions = new ObservableCollection<Submission>(connection.Query<Submission>(sql).ToList());
                    //clearFields();
                    listBoxGrade.ItemsSource = Submissions;
                }
                catch (Exception ex)
                {

                }
            }
        }

        //****************************************************************
        //Name: InsertSubmission()
        //Purpose: After user inputs JSON file, insert data to SQL, then 
        //         fill listBox
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void InsertSubmission()
        {
            using (SqlConnection connection = new SqlConnection(ConnString(databaseName)))
            {
                string query = @"INSERT INTO " + submissionTableName + "(AssignmentName, CategoryName, Grade) " +
                    "VALUES (@AssignmentName,@CategoryName,@Grade)";
                //Establish open connection
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@AssignmentName", DbType.String ));
                        command.Parameters.Add(new SqlParameter("@CategoryName", DbType.String));
                        command.Parameters.Add(new SqlParameter("@Grade", DbType.Decimal));

                    //Get information from JSON
                    foreach (Submission submission in courseWork.Submissions)
                    {
                        //Insert values for each parameter
                        command.Parameters[0].Value = submission.AssignmentName;
                        command.Parameters[1].Value = submission.CategoryName;
                        command.Parameters[2].Value = submission.Grade;
                        //Execute 
                        command.ExecuteNonQuery();
                    }
                        connection.Close();
                        GetSubmission();
                    }   
            }
        }

        //****************************************************************
        //Name: ClearDatabase()
        //Purpose: Clears entire SQL database records then calls to InsertSubmission
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void ClearDatabase()
        {
            using (SqlConnection connection = new SqlConnection(ConnString(databaseName)))
            {
                string query = @"DELETE FROM "+ submissionTableName;
                //Establish open connection
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    
                    InsertSubmission();
                }
            }
        }



        //****************************************************************
        //Name: ImportJSON_Click()
        //Purpose: When user clicks on button 'Open Course Work JSON File',
        //         opens a file dialog that allows user to select a file.
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
        //Purpose: Search for a JSON file then call function to upload to DB
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
                ClearDatabase();
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
            string messageBoxText = "Course Work GUI \n Version 2.0 \n Kevin Wang";
            string caption = "About Course Work GUI";
            MessageBoxButton button = MessageBoxButton.OK;

            MessageBox.Show(messageBoxText, caption, button);
        }
    }
}
