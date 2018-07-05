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
    class Core
    {
        public static SerialPort _mySerialPort = new SerialPort("COM5");
        public static string port;
        public static byte[] _datarequest =
        {
             0xA6,
             0x6A,
             0x13,
             0x00,
             0x01,
             0x01,
             0x01,
            //sn
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,
             0xAA,

             0xFF,
             0xFF
        };
        public static byte[] _collectrequest =
            {
            0xA6,
            0x6A,
            0x05,
            0x00,
            0x01,
            0x01,
            0x00,
            0xFF,
            0xFF,
            };
 

        public static void SetReceiver(string setport)
        {
            port = setport;
            _mySerialPort = new SerialPort(port);
            _mySerialPort.BaudRate = 9600;
            _mySerialPort.Parity = Parity.None;
            _mySerialPort.StopBits = StopBits.One;
            _mySerialPort.DataBits = 8;
            _mySerialPort.Handshake = Handshake.None;
            _mySerialPort.ReceivedBytesThreshold = 5;
           // _mySerialPort.Handshake = Handshake.RequestToSend;

        }

        public static void StartListen(SerialPort port, SerialDataReceivedEventHandler handler)
        {
            port.DataReceived += handler;

        }

        static public void FileWrite(string path, string hex)
        {

            FileStream myStream = new FileStream(@"Log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sWriter = new StreamWriter(myStream);
            sWriter.WriteLine(DateTime.Now + " Receive: " + hex);

            sWriter.Close();
            myStream.Close();

        }

        static public int DataLength(byte[] input)
        {
            return 0;
        }

        static public Data GetData(byte[] input)
        {
            if (input.Length != 0)
            {
                if (input[6] == 65)
                {
                    var tmperature = BitConverter.ToInt16(input, 22) / 100.0;
                    var mes = BitConverter.ToInt32(input, 24) / 100.0;
                    var state = BitConverter.ToUInt16(input, 28);
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

        static public SN GetSN(byte[] input)
        {
            if (input.Length != 0)
            {
                if (input[6] == 65)
                {
                    byte[] sn = new byte[14]; 
                    for(int i = 0; i < 13; i++)
                    {
                        sn[i] = input[7+i];
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

        static public void CollectCommand(SerialPort port)
        {

            port.Write(_collectrequest, 0, 9);
        }

        static public void DataCommand(SerialPort port)
        {
            port.Write(_datarequest, 0, 23);
        }


    }
}

