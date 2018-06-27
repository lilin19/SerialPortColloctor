using SignalCollectorPro.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer _mt = new System.Windows.Forms.Timer();
        static List<double> _temperature = new List<double>();
        static List<string> _time = new List<string>();
        public Form1()
        {
            InitializeComponent();
            string[] names = SerialPort.GetPortNames();
            Ports.DataSource = names;
            _mt.Tick += new EventHandler(dispatcherTimer_Tick);
            _mt.Interval = 1;
            _mt.Start();


            // Set the view to show details.
            Screen.View = View.Details;
            // Allow the user to edit item text.
            Screen.LabelEdit = true;
            // Allow the user to rearrange columns.
            Screen.AllowColumnReorder = true;
            // Display check boxes.
            Screen.CheckBoxes = false;
            // Select the item and subitems when selection is made.
            Screen.FullRowSelect = true;
            // Display grid lines.
            Screen.GridLines = true;
            // Sort the items in the list in ascending order.
            // Screen.ListViewItemSorter = new ListViewItemComparer(4);

            //Screen.Sorting = SortOrder.Descending;
            Screen.Columns.Add("ID", 100, HorizontalAlignment.Left);
            Screen.Columns.Add("测数", 100, HorizontalAlignment.Left);
            Screen.Columns.Add("温度", 100, HorizontalAlignment.Left);
            Screen.Columns.Add("实际测数", 100, HorizontalAlignment.Center);
            Screen.Columns.Add("实际温度", 200, HorizontalAlignment.Center);
            GetList();
            label8.ForeColor = Color.Red;
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Length.Text = BusinessLogics.GetCurrentSignalLength();
            Time.Text = BusinessLogics.GetCurrentSignalTime();
            Mea.Text = BusinessLogics.GetCurrentMeasurement();
            Temp.Text = BusinessLogics.GetCurrentTemperature();
            SignalContent.Text = BusinessLogics.GetCurrentSignal();
            if (Core._mySerialPort.IsOpen == true)
            {
                label8.ForeColor = Color.LightGreen;
            }

        }

        private void GetList()
        {
            List<List<string>> l = BusinessLogics.GetListFromDataBase();
            Screen.Items.Clear();
            for (int i = l.Count() - 1; i >= 0; i--)
            {

                ListViewItem item1 = new ListViewItem(l[i][0], 0);
                item1.Checked = false;
                item1.SubItems.Add(l[i][1]);
                item1.SubItems.Add(l[i][2]);
                item1.SubItems.Add(l[i][3]);
                item1.SubItems.Add(l[i][4]);
                item1.SubItems.Add(l[i][5]);
                Screen.Items.AddRange(new ListViewItem[] { item1 });
                //Screen.ListViewItemSorter = new ListViewItemComparer(4);
                //Screen.Sorting = SortOrder.Descending;
            }
        }

        private void DataReceivedHandler(
            object receiver,
            SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            SerialPort sp = (SerialPort)receiver;
            byte[] tst = new byte[sp.BytesToRead];
            //string get = sp.ReadExisting();
            sp.Read(tst, 0, sp.BytesToRead);
            //Int16 get = BitConverter.ToInt16(tst,0);     
            string hexValue = BitConverter.ToString(tst);
            if (tst.Length != 0)
            {
                BusinessLogics.SetCurrentSignalTime();
                BusinessLogics.SetCurrentSignalLength(tst.Length);
                //Length.Text = tst.Length.ToString();
                string y = "";
                for (int i = 0; i < tst.Length; i++)
                {
                    y += " " + tst[i].ToString();
                }
                BusinessLogics.SetCurrentSignal(hexValue);
            }
            if (hexValue != "")
            {
                BusinessLogics.WriteLog(hexValue);
            }

            if (tst.Length != 0)
            {
                Data d = BusinessLogics.GetData(tst);
                if (d != null)
                {
                    _temperature.Add(double.Parse(BusinessLogics.GetCurrentTemperature()));
                    _time.Add(BusinessLogics.GetCurrentSignalTime());
                    BusinessLogics.SetCurrentData(d);
                }
            }



        }

        private void start_Click(object sender, EventArgs e)
        {
            if (Core._mySerialPort != null)
            {
                Core._mySerialPort.Close();
            }
            if (Ports.SelectedIndex > -1)
            {
                MessageBox.Show(String.Format("你选择了串口 '{0}'", Ports.SelectedItem));
                try
                {
                    BusinessLogics.ConnectPort(Ports.SelectedItem.ToString());
                }
                catch (Exception)
                {

                }
                BusinessLogics.GetPort().DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            else
            {
                MessageBox.Show("Please select a port first");
            }
        }


        private bool Validation(string t, string m)
        {
            string pattern1 = "^([0-9]{1,3})$";
            string pattern2 = "^[\\+\\-]?[\\d]+(\\.[\\d]+)?$";
            Regex inte = new Regex(pattern1, RegexOptions.IgnoreCase);
            Regex flo = new Regex(pattern2, RegexOptions.IgnoreCase);
            if ((inte.IsMatch(m) || flo.IsMatch(m)))
            {
                if ((inte.IsMatch(t) || flo.IsMatch(t)))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                MessageBox.Show("数据类型错误");
                return false;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (Validation(Temp.Text.ToString(), Mea.Text.ToString()))
            {
                if (RealMea.Text != "" && RealTemp.Text != "")
                {
                    if (Validation(RealTemp.Text.ToString(), RealMea.Text.ToString()))
                    {
                        BusinessLogics.AddData(double.Parse(Mea.Text.ToString()), double.Parse(Temp.Text.ToString()), double.Parse(RealMea.Text.ToString()), double.Parse(RealTemp.Text.ToString()));
                        RealMea.Text = "";
                        RealTemp.Text = "";
                        MessageBox.Show("添加成功");
                    }
                }
                else
                {
                    MessageBox.Show("数据不能为空");
                }

            }
            else
            {
                MessageBox.Show("请打开串口");
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            DialogResult ds = MessageBox.Show("你是否要清空历史数据？", "Warn", MessageBoxButtons.OKCancel);
            if (ds == DialogResult.OK)
            {
                //del
                BusinessLogics.Clear();
            }
            else if (ds == DialogResult.Cancel)
            {
                //do nothing
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // DataBase.GetXLS(BusinessLogics.DumpDataSet(DataBase.LoadFromFile("Measure.xls")));
            BusinessLogics.GernerateXls();
            System.Diagnostics.Process.Start("数据.xls");
            MessageBox.Show("Excel 生成成功");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Chart chart = new Chart(BusinessLogics.GetChartXList(), BusinessLogics.GetChartYList());
            chart.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Temp chart2 = new Temp(_temperature, _time);
            chart2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Core.RegularMode(100,3000);

        }
    }

}
