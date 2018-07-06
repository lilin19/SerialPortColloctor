using SignalCollectorPro;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    public delegate void SerialPortReceivedHandler(object sender, SerialPortReceiveArgs e);
    interface SerialPortService
    {
        event SerialPortReceivedHandler Receive;

        void ReleasePort();
        void Listen();
        bool SerialPortisOpen();
        void ConnectPort(string com);
        SerialPort GetPort();
        void Received(SerialPortReceiveArgs e);
        void CollectCommand(int gap, int wait);
        bool DataRequest(int timeout);
        void FileWrite(string path, string hex);


    }
    public class RegularModeService : SerialPortService
    {
        private static ManualResetEvent _collectDone = new ManualResetEvent(false);
        public static List<double> _temperature = new List<double>();
        public static List<string> _time = new List<string>();

        public event SerialPortReceivedHandler Receive;

        public RegularModeService(string com)
        {
            Core._mySerialPort.Dispose();
            ConnectPort(com);
        }
        public void CollectCommand(int gap, int wait)
        {
            //
            Core.CollectCommand(GetPort());
            //Thread.Sleep(gap);
            //Core.CollectCommand(GetPort());
            //Thread.Sleep(gap);
            //Core.CollectCommand(GetPort());
            Thread.Sleep(wait - 2 * gap);
        }


        public void ReleasePort()
        {
            Core._mySerialPort.Dispose();
        }
        public bool SerialPortisOpen()
        {
            return Core._mySerialPort.IsOpen;
        }
        public bool DataRequest(int timeout)
        {
            _collectDone = new ManualResetEvent(false);
            bool index;
            Core.DataCommand(GetPort());
            Listen();
            index = _collectDone.WaitOne(timeout);
            //Core._mySerialPort.Close(); 
            return index;
        }


        public void Listen()
        {

            Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(DataReceivedHandler));
        }

        private void DataReceivedHandler(
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
                FileWrite("Log.txt", hexValue);
            }

            if (tst.Length != 0)
            {

                Data d = GetData(tst);
                SN s = GetSN(tst);
                if (d != null)
                {
                    BusinessLogics.SetCurrentSN(s);
                    BusinessLogics.SetCurrentData(d);
                    _temperature.Add(double.Parse(BusinessLogics.GetCurrentTemperature()));
                    _time.Add(BusinessLogics.GetCurrentSignalTime());
                    _collectDone.Set();
                    SerialPortReceiveArgs ev = new SerialPortReceiveArgs(tst, true);
                    Received(ev);
                }
            }

        }

        private SN GetSN(byte[] input)
        {
            if (input.Length != 0)
            {
                if (input[6] == 65)
                {
                    byte[] sn = new byte[14];
                    for (int i = 0; i < 14; i++)
                    {
                        sn[i] = input[7 + i];
                    }

                    ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
                    SN s = new SN(ascii.GetString(sn));

                    return s;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        private Data GetData(byte[] input)
        {
            if (input.Length != 0)
            {
                if (input[6] == 65)
                {
                    var tmperature = BitConverter.ToInt16(input, 21) / 100.0;
                    var mes = BitConverter.ToInt32(input, 23) / 100.0;
                    var state = BitConverter.ToUInt16(input, 27);
                    Data data = new Data(tmperature, mes, state);
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
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
        public void FileWrite(string path, string hex)
        {

            FileStream myStream = new FileStream(@"Log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(myStream);
            sWriter.WriteLine(DateTime.Now + " Receive: " + hex);

            sWriter.Close();
            myStream.Close();

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

    public class PeriodicModeService : SerialPortService
    {
        private static ManualResetEvent _collectDone = new ManualResetEvent(false);
        public static List<double> _temperature = new List<double>();
        public static List<string> _time = new List<string>();

        public event SerialPortReceivedHandler Receive;

        public PeriodicModeService(string com)
        {
            Core._mySerialPort.Dispose();
            ConnectPort(com);
        }
        public void ConnectPort(string com)
        {
            Core.SetReceiver(com);
        }

        public SerialPort GetPort()
        {
            return Core._mySerialPort;
        }
        public bool DataRequest(int timeout)
        {
            Core.DataCommand(GetPort());
            Listen();
            //Core._mySerialPort.Close(); 
            return true;
        }
        public void Listen()
        {

            Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(DataReceivedHandler));
        }

        public void Received(SerialPortReceiveArgs e)
        {
            Receive?.Invoke(this, e);
        }

        public void ReleasePort()
        {
            Core._mySerialPort.Dispose();
        }

        public bool SerialPortisOpen()
        {
            return Core._mySerialPort.IsOpen;
        }

        private void DataReceivedHandler(
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
                FileWrite("Log.txt", hexValue);
            }

            if (tst.Length != 0)
            {
                Data d = GetData(tst);
                SN s = GetSN(tst);
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
                SerialPortReceiveArgs ev = new SerialPortReceiveArgs(tst, true);
                Received(ev);
            }
        }
        private SN GetSN(byte[] input)
        {
            if (input.Length != 0)
            {
                if (input[6] == 65)
                {
                    byte[] sn = new byte[14];
                    for (int i = 0; i < 14; i++)
                    {
                        sn[i] = input[7 + i];
                    }

                    ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
                    SN s = new SN(ascii.GetString(sn));

                    return s;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        private Data GetData(byte[] input)
        {
            if (input.Length != 0)
            {
                if (input[6] == 65)
                {
                    var tmperature = BitConverter.ToInt16(input, 21) / 100.0;
                    var mes = BitConverter.ToInt32(input, 23) / 100.0;
                    var state = BitConverter.ToUInt16(input, 27);
                    Data data = new Data(tmperature, mes, state);
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public void FileWrite(string path, string hex)
        {

            FileStream myStream = new FileStream(@"Log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(myStream);
            sWriter.WriteLine(DateTime.Now + " Receive: " + hex);

            sWriter.Close();
            myStream.Close();

        }
        public void CollectCommand(int gap, int wait)
        {
            throw new NotImplementedException();
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
