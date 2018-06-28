using Accord.IO;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlopeRegression
{
    class Program
    {
        static void Main(string[] args)
        {

            // Create a new reader, opening a given path
            ExcelReader reader = new ExcelReader("数据.xls");

            // Finally, we can request an specific sheet:
            DataTable table = reader.GetWorksheet("测量");

            // Now, we have loaded the Excel file into a DataTable. We
            // can go further and transform it into a matrix to start
            // running other algorithms on it: 



            // We can also do it retrieving the name for each column:
            //string[] columnNames;
            // table.ToArray();

            // Or we can extract specific columns into single arrays:
           
            double[] x = table.Columns[1].ToArray();
            double[] y = table.Columns[3].ToArray();

            // PS: you might need to import the Accord.Math namespace in
            //   order to be able to call the ToMatrix extension methods.



            double[] inputs =
           { 3.0 , 7.0, 3.0 ,3.0 ,6.0 };

            // The task is to output a weighted sum of those numbers 
            // plus an independent constant term: 7.4x + 1.1y + 42
            double[] outputs =
            {12.3,28.6,12.3,12.4,24.5};


            // Use the algorithm to learn the machine
            Console.WriteLine(Regression.RegressSlope(x, y));
            Console.WriteLine(Regression.RegressCut(x, y));
            Console.ReadKey();


            // Compute the error in the prediction (should be 0.0)
           
        }
    }
}
