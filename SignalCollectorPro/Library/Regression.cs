using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Regression.Linear;

namespace SignalCollectorPro.Library
{
    public class Regression
    {

        public static double RegressSlope(double[] inputs, double[] outputs)
        {


            OrdinaryLeastSquares ols = new OrdinaryLeastSquares();

            // Use OLS to learn the simple linear regression
            SimpleLinearRegression regression = ols.Learn(inputs, outputs);

            // Compute the output for a given input:
            //double y = regression.Transform(85); // The answer will be 28.088

            // We can also extract the slope and the intercept term
            // for the line. Those will be -0.26 and 50.5, respectively.
            double s = regression.Slope;     // -0.264706
            double c = regression.Intercept; // 50.588235

            return s;
        }

        public static double RegressCut(double[] inputs, double[] outputs)
        {


            OrdinaryLeastSquares ols = new OrdinaryLeastSquares();

            // Use OLS to learn the simple linear regression
            SimpleLinearRegression regression = ols.Learn(inputs, outputs);

            // Compute the output for a given input:
            //double y = regression.Transform(85); // The answer will be 28.088

            // We can also extract the slope and the intercept term
            // for the line. Those will be -0.26 and 50.5, respectively.
            double s = regression.Slope;     // -0.264706
            double c = regression.Intercept; // 50.588235

            return c;
        }
    }
}
