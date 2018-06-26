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
        public static void SetReceiver(string port)
        {
            _mySerialPort = new SerialPort(port);
            _mySerialPort.BaudRate = 9600;
            _mySerialPort.Parity = Parity.None;
            _mySerialPort.StopBits = StopBits.One;
            _mySerialPort.DataBits = 8;
            _mySerialPort.Handshake = Handshake.None;
            _mySerialPort.ReceivedBytesThreshold = 5;
           // _mySerialPort.Handshake = Handshake.RequestToSend;

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
    }
}

