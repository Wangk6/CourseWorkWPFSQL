//******************************************************
// File: CourseMenu.cs
//
// Purpose: Utilizing the DLL's of CourseWork. Required
//          files are Submission.cs, Category.cs and 
//          Assignment.cs. It will allow a user to read,
//          write and display JSON, XML data from a file.
//
// Written By: Kevin Wang
//
// Compiler: Visual Studio 2017
//
//******************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using CourseWork;
using System.IO;

namespace ConsoleMenu
{
    class CourseMenu
    {
        CourseWorkClass courseWork = new CourseWorkClass();
        //User defines the file name
        private string fileName;

        static void Main(string[] args)
        {
            int choice = 0;
            CourseMenu cm = new CourseMenu();

            //Call MainMenu and display information for user on availble options
            do
            {
                choice = cm.MainMenu();
            }
            while (choice != 7);
        }

        #region Main Menu
        //****************************************************************
        //Name: MainMenu()
        //Purpose: Displays text on screen that lets the user know the
        //         available method options to be used. Returns a number
        //         for user to end the program.
        //Input Type: None
        //Output Type: int
        //****************************************************************
        private int MainMenu()
        {
            string choice;
            bool res;
            int a = 0;
            bool valid = false;

            while (valid == false)
            {
                Console.Write("CourseWork Testing Menu" + Environment.NewLine +
                                  "-----------------------" + Environment.NewLine +
                                "1 - Read course work from JSON file" + Environment.NewLine +
                                "2 - Read course work from XML file" + Environment.NewLine +
                                "3 - Write course work to JSON file" + Environment.NewLine +
                                "4 - Write course work to XML file" + Environment.NewLine +
                                "5 - Display all course work on screen" + Environment.NewLine +
                                "6 - Find submission" + Environment.NewLine +
                                "7 - Exit" + Environment.NewLine +
                                "Enter Choice: ");
                choice = Console.ReadLine();
                Console.WriteLine(Environment.NewLine);
                //Convert user choice to an int, if not an int set to 0 and have user choose again
                if (res = int.TryParse(choice, out a) == false)
                {
                    a = 0;
                }
                else
                {
                    switch (a)
                    {
                        case 1:
                            valid = true;
                            readCourseWorkJSON();
                            Console.WriteLine(Environment.NewLine);
                            break;
                        case 2:
                            valid = true;
                            readCourseWorkXML();
                            Console.WriteLine(Environment.NewLine);
                            break;
                        case 3:
                            valid = true;
                            writeCourseWorkJSON();
                            Console.WriteLine(Environment.NewLine);
                            break;
                        case 4:
                            valid = true;
                            writeCourseWorkXML();
                            Console.WriteLine(Environment.NewLine);
                            break;
                        case 5:
                            valid = true;
                            displayCourseWork();
                            Console.WriteLine(Environment.NewLine);
                            break;
                        case 6:
                            valid = true;
                            findSubmission();
                            Console.WriteLine(Environment.NewLine);
                            break;
                        case 7: //Exit case
                            valid = true;
                            break;
                        default:
                            Console.WriteLine("Invalid Input");
                            break;
                    }
                }
            }
            return a;
        }
        #endregion

        #region Read/Write/Display CourseWork
        //****************************************************************
        //Name: readCourseWorkJSON()
        //Purpose: Asks the user to input a file to read and will search for
        //         a JSON file to set to the object
        //Input Type: None - setFileName for file name
        //Output Type: None
        //****************************************************************
        private void readCourseWorkJSON()
        {
            //Call setFileName for user to set global fileName variable
            setFileName();
            //Open a reader using the file name given by user to read the JSON/XML
            try
            {
                FileStream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CourseWorkClass));
                courseWork = (CourseWorkClass)serializer.ReadObject(reader);
                reader.Close();
                Console.WriteLine("Reading Completed");
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
        //Name: readCourseWorkXML()
        //Purpose: Asks the user to input a file to read and will search for
        //         a XML file to set to the object
        //Input Type: None - setFileName for file name
        //Output Type: None
        //****************************************************************
        private void readCourseWorkXML()
        {
            //Call setFileName for user to set global fileName variable
            setFileName();
            //Open a reader using the file name given by user to read the JSON/XML
            try
            {
                FileStream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                DataContractSerializer serializer = new DataContractSerializer(typeof(CourseWorkClass));
                courseWork = (CourseWorkClass)serializer.ReadObject(reader);
                reader.Close();
                Console.WriteLine("Reading Completed");
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
        //Name: writeCourseWorkJSON()
        //Purpose: Asks the user to input a file to write and will write based
        //         on the objects members
        //Input Type: None - setFileName for file name
        //Output Type: None - creates a file
        //****************************************************************
        private void writeCourseWorkJSON()
        {
            //Call setFileName for user to set global fileName variable
            setFileName();
            //Open a writer using the file name given by user to write the JSON/XML to
            FileStream writer = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            //Currently it sets whatever is given as default in constructor, could change to user input also
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CourseWorkClass));
            serializer.WriteObject(writer, courseWork);
            writer.Close();
            Console.WriteLine("Writing Completed");
        }

        //****************************************************************
        //Name: writeCourseWorkXML()
        //Purpose: Asks the user to input a file to write and will write based
        //         on the objects members
        //Input Type: None - setFileName for file name
        //Output Type: None - creates a file
        //****************************************************************
        private void writeCourseWorkXML()
        {
            //Call setFileName for user to set global fileName variable
            setFileName();
            //Open a writer using the file name given by user to write the JSON/XML to
            FileStream writer = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            //Currently it sets whatever is given as default in constructor, could change to user input also
            DataContractSerializer serializer = new DataContractSerializer(typeof(CourseWorkClass));
            serializer.WriteObject(writer, courseWork);
            writer.Close();
            Console.WriteLine("Writing Completed");
        }

        //****************************************************************
        //Name: displayCourseWork()
        //Purpose: Displays the current instance of the objects variable values
        //         on screen
        //Input Type: None
        //Output Type: None
        //****************************************************************
        private void displayCourseWork()
        {
            Console.WriteLine(courseWork);
        }

        //****************************************************************
        //Name: findSubmission()
        //Purpose: Find the submission after reading from file
        //Input Type: None - User enters assignment name
        //Output Type: None - Outputs if assignment found
        //****************************************************************
        private void findSubmission()
        {
            Console.Write("Enter assignment name: ");
            string name = Console.ReadLine();
            Submission result = courseWork.Submissions.Find(x => x.AssignmentName == name);
            if (result != null)
            {
                Console.WriteLine(result.CategoryName + ", " + result.AssignmentName + ", " + result.Grade);
            }
            else
            {
                Console.WriteLine("Assignment not found");
            }
        }

        #endregion

        #region File
        //Set File Name
        private void setFileName()
        {
            //Set fileName to default blank
            fileName = "";
            //If no name is set, ask for name
            do
            {
                Console.Write("Input your filename: ");
                fileName = Console.ReadLine();
            } while (fileName == "");
        }
        #endregion
    }
}
