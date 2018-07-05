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
        public delegate void SerialPortReceivedHandler(object sender, SerialPortReceiveArgs e);
        public event SerialPortReceivedHandler Receive;
        private static ManualResetEvent _collectDone = new ManualResetEvent(false);
        public static List<double> _temperature = new List<double>();
        public static List<string> _time = new List<string>();
        public SerialPortService(string com)
        {
            Core._mySerialPort.Dispose();
            ConnectPort(com);
        }
        public void CollectCommand(int gap, int wait)
        {
            //
            Core.CollectCommand(GetPort());
            Thread.Sleep(gap);
            Core.CollectCommand(GetPort());
            Thread.Sleep(gap);
            Core.CollectCommand(GetPort());
            Thread.Sleep(wait-2*gap);
        }


        public void PortRelease()
        {
            Core._mySerialPort.Dispose();
        }

        public bool DataRequest(int timeout)
        {
            
            _collectDone = new ManualResetEvent(false);
            bool index;
            
           
            
            Core.DataCommand(GetPort());
           
            RegularListen();
            index = _collectDone.WaitOne(timeout);
            
            //Core._mySerialPort.Close(); 
            return index;
            
        }

        public void PeriodicListen()
        {
            
            Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(PeriodicDataReceivedHandler));
        }

        public void RegularListen()
        {
            
            Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(RegularDataReceivedHandler));
        }

        private void RegularDataReceivedHandler(
  object receiver,
  SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(5);
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
                BusinessLogics.FileWrite("Log.txt",hexValue);
            }

            if (tst.Length != 0)
            {
             
                Data d = BusinessLogics.GetData(tst);
                
                SN s = BusinessLogics.GetSN(tst);
                if (d != null)
                {
                    BusinessLogics.SetCurrentSN(s);
                    BusinessLogics.SetCurrentData(d);
                    _temperature.Add(double.Parse(BusinessLogics.GetCurrentTemperature()));
                    _time.Add(BusinessLogics.GetCurrentSignalTime());
                    _collectDone.Set();
                    SerialPortReceiveArgs ev = new SerialPortReceiveArgs(tst,true);
                    Received(ev);
                }
            }
            
        }

        private void PeriodicDataReceivedHandler(
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
                BusinessLogics.FileWrite("Log.txt",hexValue);
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
                else
                {
                    BusinessLogics.SetCurrentData(null);
                    BusinessLogics.SetCurrentSN(null);
                }
                SerialPortReceiveArgs ev = new SerialPortReceiveArgs(tst,true);
                Received(ev);
            }

    
        }

        public void ConnectPort(string com)
        {
            Core.SetReceiver(com);
            
            //try
            //{
            //    Core.SetReceiver(com);
            //    Core._mySerialPort.Open();
            //}
            //catch (Exception)
            //{

            //}
        }

        public SerialPort GetPort()
        {
            return Core._mySerialPort;
        }

        public void Received(SerialPortReceiveArgs e)
        {
            Receive?.Invoke(this, e);
        }
    }

    public class SerialPortReceiveArgs : EventArgs
    {

        private byte[] tst;
        public bool response;
        //Constructor.
        //
        public SerialPortReceiveArgs(byte[] tst, bool response)
        {
            this.tst = tst;
            this.response = response;
        }
        public byte[] Content
        {
            get { return tst; }
        }
    }
}
