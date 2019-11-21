using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    public class CourseWorkUnitTesting
    {
        //Test to see if category works correctly
        public void UnitTestCategory()
        {
            Category cat = new Category();
            
            if(cat.Name == "History")
            {
                Console.WriteLine("Category Name Property: Pass");
            }
            else
            {
                Console.WriteLine("Category Name Property: Fail");
            }
            if (cat.Percentage >= 65)
            {
                Console.WriteLine("Category Percentage Property: Pass");
            }
            else
            {
                Console.WriteLine("Category Percentage Property: Failed");
            }

        }
        //Test to see if assignments works correctly
        public void UnitTestAssignment()
        {
            Assignment assn = new Assignment();

            if(assn.Name == "Math" && assn.CategoryName == "Quiz")
            {
                Console.WriteLine("Assignment Name Property: Pass");
                if(assn.Description == "Take the quiz by Wednesday")
                {
                    Console.WriteLine("Assignment Description Property: Pass");
                }
                else
                {
                    Console.WriteLine("Assignment Description Property: Failed");
                }

                if(assn.CategoryName == "Quiz")
                {
                    Console.WriteLine("Assignment CategoryName Property: Pass");
                }
                else
                {
                    Console.WriteLine("Assignment CategoryName Property: Failed");
                }
            }
            else
            {
                Console.WriteLine("Assignment Name Property: Fail");
            }
        }
    }
}
