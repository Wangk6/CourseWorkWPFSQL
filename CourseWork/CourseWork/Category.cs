//******************************************************
// File: Category.cs
//
// Purpose: Contains the class definition for Categorys.
//          Category will hold all category information 
//          such as the name of the category and the grade
//          received by the student.
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
using System.Runtime.Serialization;

namespace CourseWork
{
    [DataContract(Name = "categories")]
    public class Category
    {
        #region Private Member Variables
        private string m_Name;
        private double m_Percentage;
        #endregion

        #region Methods
        //****************************************************************
        //Name: Category()
        //Purpose: Default constructor that sets the name and percentage
        //         variables
        //Input Type: None
        //Output Type: None
        //****************************************************************
        public Category()
        {
            m_Name = "Homework";
            m_Percentage = 35;
        }
        //****************************************************************
        //Name: ToString()
        //Purpose: Displays descriptive text of name and percentage
        //Input Type: None
        //Output Type: None
        //****************************************************************
        public override string ToString()
        {
            return "Name: " + Name + " Percentage" + Percentage;
        }
        #endregion

        #region Properties
        [DataMember(Name = "name")]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        [DataMember(Name = "percentage")]
        public double Percentage
        {
            get
            {
                return m_Percentage;
            }
            set
            {
                m_Percentage = value;
            }
        }
        #endregion


    }
}
