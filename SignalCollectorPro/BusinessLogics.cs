using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    class BusinessLogics
    {
        public static string GetCurrentData()
        {
            if (DataCentre.CurrentData != null)
            {
                return DataCentre.CurrentData.ToString();
            }
            else
            { return "无数据"; }
        }

        public static bool GetCurrentTimeout()
        {
            if (DataCentre.CurrentSignal != null)
            {
                return DataCentre.Timeout;
            }
            else
            { return false; }
        }

        public static string GetCurrentState()
        {
            if (DataCentre.CurrentData != null)
            {
                return DataCentre.CurrentData._error.ToString();
            }
            else
            { return "无数据"; }
        }


        public static string GetCurrentSN()
        {
            if (DataCentre.CurrentSN != null)
            {
                return DataCentre.CurrentSN.sn;
            }
            else
            { return "无数据"; }
        }

        public static string GetCurrentMeasurement()
        {
            if (DataCentre.CurrentData != null)
            {
                return DataCentre.CurrentData.GetMeasurement();
            }
            else
            { return "无数据"; }
        }

        public static string GetCurrentTemperature()
        {
            if (DataCentre.CurrentData != null)
            {
                return DataCentre.CurrentData.GetTemperature();
            }
            else
            { return "无数据"; }
        }
        public static string GetCurrentSignal()
        {
            if (DataCentre.CurrentSignal != null)
            {
                return DataCentre.CurrentSignal.ToString();
            }
            else
            { return "无数据"; }
        }


        public static string GetCurrentSignalTime()
        {
            if (DataCentre.CurrentSignal != null)
            {
                return DataCentre.CurrentSignalTime.ToString();
            }
            else
            { return "无数据"; }
        }

        public static string GetCurrentSignalLength()
        {
            if (DataCentre.CurrentSignalLength != 0)
            {
                return DataCentre.CurrentSignalLength.ToString();
            }
            else
            { return "无数据"; }
        }

        public static void SetCurrentSignalTime()
        {
            DataCentre.CurrentSignalTime = DateTime.Now;
        }

        public static void SetCurrentData(Data input)
        {
            DataCentre.CurrentData = input;
        }

        public static void SetCurrentSN(SN s)
        {
            DataCentre.CurrentSN = s;
        }

        public static void SetCurrentSignal(string input)
        {
            DataCentre.CurrentSignal = input;
        }

        public static void SetCurrentSignalLength(int length)
        {
            DataCentre.CurrentSignalLength = length;
        }

        public static void AddData(double m, double t, double rm, double rt)
        {
            DataBaseManager.AddToDatabase(new SaveMeasure(m, t, rm, rt));
        }

        public static void Clear()
        {
            DataBaseManager.CleanDatabase();
        }

        public static void GernerateXls()
        {
            DataBaseManager.GetXLS(BusinessLogics.DumpDataSet(DataBaseManager.LoadFromFile("Measure.xls")));
        }


        public static List<string> GetChartXList()
        {
            List<List<string>> l = BusinessLogics.GetListFromDataBase();
            List<string> x = new List<string>(l.Capacity);
            for (int i = 0; i < l.Count; i++)
            {
                x.Add(l[i][1]);
            }
            return x;
        }

        public static double[] GetMeasureList()
        {
            List<List<string>> l = BusinessLogics.GetListFromDataBase();
            List<double> x = new List<double>(l.Capacity);
            for (int i = 0; i < l.Count; i++)
            {
                x.Add(double.Parse(l[i][2]));
            }
            return x.ToArray();
        }

        public static double[] GetRealMeasureList()
        {
            List<List<string>> l = BusinessLogics.GetListFromDataBase();
            List<double> x = new List<double>(l.Capacity);
            for (int i = 0; i < l.Count; i++)
            {
                x.Add(double.Parse(l[i][2]));
            }
            return x.ToArray();
        }


        public static List<double> GetChartYList()
        {
            List<List<string>> l = BusinessLogics.GetListFromDataBase();
            List<double> y = new List<double>(l.Capacity);

            for (int i = 0; i < l.Count; i++)
            {
                y.Add(double.Parse(l[i][3]) / double.Parse(l[i][1]));
            }
            return y;
        }


        public static List<List<string>> GetListFromDataBase()
        {
            return DumpDataSet(DataBaseManager.LoadFromFile("Measure.xls"));
        }

        public static List<List<string>> DumpDataSet(DataTable dt)
        {
            List<List<string>> list = new List<List<string>>();

            //Console.Out.WriteLine("\tTableName: {0}", dt.TableName);

            // ... Write the table schema
            //foreach (DataColumn col in dt.Columns)
            //{
            //    Console.Out.Write("\t\t" + col.ColumnName + " ");
            //}
            //Console.Out.WriteLine("\t\t");

            // ... Write the table contents
            foreach (DataRow row in dt.Rows)
            {
                List<string> l = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //Console.Out.Write("\t\t" + row[i]);
                    l.Add(row[i].ToString());
                }
                list.Add(l);
                //Console.Out.WriteLine("");
            }
            return list;

        }


    }
}

