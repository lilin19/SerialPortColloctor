using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    class SerialPortService
    {
        

        private static ManualResetEvent _collectDone = new ManualResetEvent(false);
        public static List<double> _temperature = new List<double>();
        public static List<string> _time = new List<string>();
        public static bool RegularMode(int gap, int wait,int timeout)
        {
            bool index;
            Core.CollectCommand(BusinessLogics.GetPort());
            Thread.Sleep(gap);
            Core.CollectCommand(BusinessLogics.GetPort());
            Thread.Sleep(gap);
            Core.CollectCommand(BusinessLogics.GetPort());
            Thread.Sleep(wait-2*gap);
            Core.DataCommand(BusinessLogics.GetPort());
            
            RegularListen();
            index = _collectDone.WaitOne(timeout);
            Core._mySerialPort.Close();
            
            return index;
        }

        public static void PeriodicListen()
        {

        Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(PeriodicDataReceivedHandler));
        }

        public static void RegularListen()
        {
            Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(RegularDataReceivedHandler));
        }

        private static void RegularDataReceivedHandler(
  object receiver,
  SerialDataReceivedEventArgs e)
        {
            
            Thread.Sleep(50);
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
                BusinessLogics.SetCurrentData(d);
                if (d != null)
                {
                    _temperature.Add(double.Parse(BusinessLogics.GetCurrentTemperature()));
                    _time.Add(BusinessLogics.GetCurrentSignalTime());
                    

                }
                _collectDone.Set();
            }
            
            
            
        }

        private static void PeriodicDataReceivedHandler(
   object receiver,
   SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(50);
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
                SN s = BusinessLogics.GetSN(tst);
                if (d != null)
                {
                    _temperature.Add(double.Parse(d.GetTemperature()));
                    _time.Add(BusinessLogics.GetCurrentSignalTime());
                    BusinessLogics.SetCurrentData(d);
                    BusinessLogics.SetCurrentSN(s);
                }
            }

            _collectDone.Set();
        }

    }
}
