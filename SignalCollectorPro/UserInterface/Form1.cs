﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    public partial class Form1 : Form
    {
        static Thread th;
        static System.Windows.Forms.Timer _mt = new System.Windows.Forms.Timer();
        static List<double> _temperature = new List<double>();
        static List<string> _time = new List<string>();
        
        static SerialPortService _sps;
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
            label9.ForeColor = Color.White;
            button7.Enabled = false;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
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



        private void start_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            button6.Enabled = true;
            label8.ForeColor = Color.Red;
            label9.ForeColor = Color.White;
            if (th != null)
            {
                if (th.IsAlive)
                {
                    th.Abort();
                    th = null;
                }
            }
            if (_sps != null)
            {
                _sps.StopService();
            }

            if (Ports.SelectedIndex > -1)
            {
                MessageBox.Show(String.Format("你选择了串口 '{0}'", Ports.SelectedItem));
                try
                {
                    _sps = new PeriodicModeDriver(Ports.SelectedItem.ToString());
                    _sps.ReceiveError += refresh;
                    _sps.ReceiveSuccess += ReceiveRequest;
                    _sps.StartService();
                    if (_sps.IsOnService == true)
                    {
                        label8.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        label8.ForeColor = Color.Red;
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Please select a port first");
            }
        }

        void refresh(object sender, PacketErrorReceiveArgs e)
        {
            Invoke(new MethodInvoker(() =>
           {  
               Mea.Text = "无数据";
               Temp.Text = "无数据";
               SNCode.Text = "无数据";
               if (e.response)
               {
                   SignalContent.Text = BitConverter.ToString(e.Content);
                   Time.Text = DateTime.Now.ToString();
                   label8.ForeColor = Color.Red;
                   label9.ForeColor = Color.White;
                   Length.Text = e.length.ToString();    
               }
               else
               {
                   Length.Text = "无数据";
                   SignalContent.Text = "无数据";
                   Time.Text = "无数据";
                   label8.ForeColor = Color.Red;
                   label9.ForeColor = Color.Red;
                   state.Text = "超时";
               }

               switch (e.type)
               { 
                   case 1:
                       state.Text = "命令类型错误";
                       break;
                   case 8:
                       state.Text = "CRC 检验错误";
                       break;
                   case 7:
                       state.Text = "长度/协议头错误";
                       break;
               }

           }));
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
            // Temp chart2 = new Temp(_sps._temperature, SerialPortService._time);
            //chart2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Ports.SelectedIndex > -1)
            {
                MessageBox.Show(String.Format("你选择了串口 '{0}'", Ports.SelectedItem));
                button7.Enabled = true;
                label9.ForeColor = Color.White;
                string port = Ports.SelectedItem.ToString();
                int portindex = Ports.SelectedIndex;
                if (portindex > -1)
                {
                    button6.Enabled = false;
                    try
                    {
                        label8.ForeColor = Color.Blue;
                        th = new Thread(() =>
                        {
                            try
                            {
                                _sps = new RegularModeDriver(port);
                                _sps.ReceiveError += refresh;
                                _sps.ReceiveSuccess += ReceiveRequest;
                                _sps.StartService();
                            }
                            catch (Exception)
                            {

                            }
                        });
                        th.Start();

                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    MessageBox.Show("Please select a port first");
                }

            }
            else
            {
                MessageBox.Show("Please select a port first");
            }



        }


        public void ReceiveRequest(object sender, PacketReceiveArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                label9.ForeColor = Color.White;
                //MessageBox.Show(e.response.ToString());

                    label8.ForeColor = Color.Green;
                    Length.Text = e.length.ToString();
                    Time.Text = DateTime.Now.ToString();
                    Mea.Text = e.pack._measurement.ToString();
                    Temp.Text = e.pack._temperature.ToString();
                    SignalContent.Text = BitConverter.ToString(e.tst);
                    SNCode.Text = e.sn.sn.ToString();
                    state.Text = (Convert.ToString(e.pack._error[0], 2).PadLeft(8, '0') + Convert.ToString(e.pack._error[1], 2).PadLeft(8, '0'));     
            }));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            button6.Enabled = true;
            label8.ForeColor = Color.Red;
            label9.ForeColor = Color.White;
            if (th != null)
            {
                th.Abort();
                th = null;
            }
            _sps.StopService();
            _sps = null;
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Determine if text has changed in the textbox by comparing to original text.
            th.Abort();
            th = null;
            Application.Exit();
        }
    }

}
