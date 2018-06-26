using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalCollectorPro
{
    public partial class Chart : Form
    {
        public Chart(List<string> x, List<double> y)
        {
            

            InitializeComponent();
            Measure.Series["测数实测比较"].Points.DataBindXY(x, y);
        }
    }
}
