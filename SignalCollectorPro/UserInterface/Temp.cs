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
    public partial class Temp : Form
    {
        public Temp(List<double> x, List<string> t)
        {
            InitializeComponent();
            chart1.Series["温度"].Points.DataBindXY(t, x);
        }
    }
}
