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
    public delegate void ErrorReceivedHandler(object sender, PacketErrorReceiveArgs e);
    public delegate void DataReceivedHandler(object sender, PacketReceiveArgs e);

    interface SerialPortService
    {
        event ErrorReceivedHandler ReceiveError;
        event DataReceivedHandler ReceiveSuccess;
        bool StartService();
        void StopService();
        bool IsOnService { get; }
    }

    public class RegularModeDriver : SerialPortService
    {
        private ManualResetEvent _collectDone = new ManualResetEvent(false);
        public static List<double> _temperature = new List<double>();
        public static List<string> _time = new List<string>();
        private string _port;
        private bool _canReceive;
        public bool IsOnService { get => Core._mySerialPort.IsOpen; }

        public event ErrorReceivedHandler ReceiveError;
        public event DataReceivedHandler ReceiveSuccess;
        public bool StartService()
        {
            bool response = false;
            bool power = true;
            ConnectPort(_port);

            while (power)
            {
                DateTime start = DateTime.Now;
                if (!IsOnService)
                {
                    ConnectPort(_port);
                }
                CollectCommand(100, 3000);
                Core._mySerialPort.DiscardInBuffer();
                Listen();
                this._canReceive = true;
                response = DataRequest(1000);
                this._canReceive = false;
                if (response == false)
                {
                    ReleasePort();
                    PacketErrorReceiveArgs ev = new PacketErrorReceiveArgs(null,0, false, _port, 9);
                    RaiseErrorHandler(ev);
                }
                else
                {
                    ReleasePort();
                }
                DateTime end = DateTime.Now;
                TimeSpan span = TimeSpan.FromSeconds(5) - (end - start);
                Thread.Sleep(span);
            }
            return true;
        }


        public void StopService()
        {
            ReleasePort();
        }
        public RegularModeDriver(string com)
        {
            Core._mySerialPort.Dispose();
            this._port = com;
            Core._mySerialPort = null;

        }
        private void CollectCommand(int gap, int wait)
        {
            //
            Core.CollectCommand(GetPort());
            //Thread.Sleep(gap);
            //Core.CollectCommand(GetPort());
            //Thread.Sleep(gap);
            //Core.CollectCommand(GetPort());
            Thread.Sleep(wait - 2 * gap);
        }


        private void ReleasePort()
        {

            Core._mySerialPort.Dispose();

        }
        private bool DataRequest(int timeout)
        {
            _collectDone = new ManualResetEvent(false);
            bool index;
            Core.DataCommand(GetPort());
            index = _collectDone.WaitOne(timeout);
            //Core._mySerialPort.Close(); 
            return index;
        }

        private void Listen()
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
            if (_canReceive==false){
                return;
            }
            //string get = sp.ReadExisting();
            sp.Read(tst, 0, sp.BytesToRead);
            //Int16 get = BitConverter.ToInt16(tst,0);     
            string hexValue = BitConverter.ToString(tst);
            if (hexValue != "")
            {
                FileWrite("Log.txt", hexValue);
            }


            if (tst.Length == 32 && tst[2] == 28)
            {
                byte[] crc = (byte[])tst.Clone();
                Core.ModRTU_CRC(ref crc, 30);
                if (crc[30]==tst[30] && crc[31]==tst[31])
                {
                    Data d = GetData(tst);
                    SN s = GetSN(tst);
                    if (d == null)
                    {
                        PacketErrorReceiveArgs eb = new PacketErrorReceiveArgs(tst, tst.Length,true, Core.port, 1);
                        RaiseErrorHandler(eb);
                    }
                    else
                    {
                        PacketReceiveArgs ev = new PacketReceiveArgs(tst, tst.Length, true,d,s);
                        RaiseReceiveHandler(ev);
                    }
                }
                else
                {
                    // throw (new Exception());
                    PacketErrorReceiveArgs ev = new PacketErrorReceiveArgs(tst,tst.Length, true, Core.port, 8);
                    RaiseErrorHandler(ev);
                }
            }
            else
            {
                PacketErrorReceiveArgs ev = new PacketErrorReceiveArgs(tst,tst.Length, true, Core.port, 7);
                RaiseErrorHandler(ev);
            }
            _collectDone.Set();
        }

        private SN GetSN(byte[] input)
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
        private Data GetData(byte[] input)
        {
            if (input[6] == 65)
            {
                var tmperature = BitConverter.ToInt16(input, 22) / 100.0;
                var mes = BitConverter.ToInt32(input, 24) / 100.0;
                byte[] state = new byte[2];
                state[0] = input[28];
                state[1] = input[29];
                Data data = new Data(tmperature, mes, state);
                return data;
            }
            else
            {
                return null;
            }
        }

        private void ConnectPort(string com)
        {
            Core.SetReceiver(com);

            //{

            //}
        }
        private void FileWrite(string path, string hex)
        {

            FileStream myStream = new FileStream(@"Log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(myStream);
            sWriter.WriteLine(DateTime.Now + " Receive: " + hex);

            sWriter.Close();
            myStream.Close();

        }
        private SerialPort GetPort()
        {
            return Core._mySerialPort;
        }

        private void RaiseErrorHandler(PacketErrorReceiveArgs e)
        {
            ReceiveError?.Invoke(this, e);
        }

        private void RaiseReceiveHandler(PacketReceiveArgs e)
        {
            ReceiveSuccess?.Invoke(this, e);
        }


    }

    public class PeriodicModeDriver : SerialPortService
    {
        private static ManualResetEvent _collectDone = new ManualResetEvent(false);
        public static List<double> _temperature = new List<double>();
        public static List<string> _time = new List<string>();
        public event ErrorReceivedHandler ReceiveError;
        public event DataReceivedHandler ReceiveSuccess;

        public bool IsOnService { get => Core._mySerialPort.IsOpen; }
        public bool StartService()
        {
            Listen();
            return true;
        }

        public void StopService()
        {
            ReleasePort();
        }
        public PeriodicModeDriver(string com)
        {
            Core._mySerialPort.Dispose();
            ConnectPort(com);
        }
        private void ConnectPort(string com)
        {
            Core.SetReceiver(com);
        }

        private void Listen()
        {

            Core.StartListen(Core._mySerialPort, new SerialDataReceivedEventHandler(DataReceivedHandler));
        }

        private void ReleasePort()
        {
            Core._mySerialPort.Dispose();
        }

        private bool SerialPortisOpen()
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
            if (hexValue != "")
            {
                FileWrite("Log.txt", hexValue);
            }
            if (tst.Length == 32 && tst[2] == 28)
            {
                byte[] crc = (byte[])tst.Clone();
                Core.ModRTU_CRC(ref crc, 30);
                if (crc[30] == tst[30] && crc[31] == tst[31])
                {
                    Data d = GetData(tst);
                    SN s = GetSN(tst);
                    if (d == null)
                    {
                        PacketErrorReceiveArgs eb = new PacketErrorReceiveArgs(tst,tst.Length, true, Core.port, 1);
                        RaiseErrorHandler(eb);
                    }
                    else
                    {
                        PacketReceiveArgs ev = new PacketReceiveArgs(tst,tst.Length, true,d,s);
                        RaiseReceiveHandler(ev);
                    }
                }
                else
                {
                    // throw (new Exception());
                    PacketErrorReceiveArgs ev = new PacketErrorReceiveArgs(tst,tst.Length, true, Core.port, 8);
                    RaiseErrorHandler(ev);
                }
            }
            else
            {
                PacketErrorReceiveArgs ev = new PacketErrorReceiveArgs(tst,tst.Length, true, Core.port, 7);
                RaiseErrorHandler(ev);
            }
        }
        private SN GetSN(byte[] input)
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
        private Data GetData(byte[] input)
        {
            if (input[6] == 65)
            {
                var tmperature = BitConverter.ToInt16(input, 21) / 100.0;
                var mes = BitConverter.ToInt32(input, 23) / 100.0;
                byte[] state = new byte[2];
                state[0] = input[27];
                state[1] = input[28];
                Data data = new Data(tmperature, mes, state);
                return data;
            }
            else
            {
                return null;
            }

        }
        private void FileWrite(string path, string hex)
        {

            FileStream myStream = new FileStream(@"Log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(myStream);
            sWriter.WriteLine(DateTime.Now + " Receive: " + hex);

            sWriter.Close();
            myStream.Close();

        }
        private void RaiseErrorHandler(PacketErrorReceiveArgs e)
        {
            ReceiveError?.Invoke(this, e);
        }

        private void RaiseReceiveHandler(PacketReceiveArgs e)
        {
            ReceiveSuccess?.Invoke(this, e);
        }
    }
    public class PacketErrorReceiveArgs : EventArgs
    {

        public byte[] tst;
        public bool response;
        public string port;
        public int length;
        public int type;
        //Constructor.
        //
        public PacketErrorReceiveArgs(byte[] tst, int length ,bool response, string port, int type)
        {
            this.tst = tst;
            this.response = response;
            this.port = port;
            this.type = type;
            this.length = length;
        }
        public byte[] Content
        {
            get { return tst; }
        }
    }

    public class PacketReceiveArgs : EventArgs
    {

        public byte[] tst;
        public bool response;
        public int length;
        public Data pack;
        public SN sn;
        //Constructor.
        //
        public PacketReceiveArgs(byte[] tst,int length, bool response, Data pack,SN sn)
        {
            this.tst = tst;
            this.response = response;
            this.length = length;
            this.pack = pack;
            this.sn = sn;
        }
        public byte[] Content
        {
            get { return tst; }
        }
    }

}
