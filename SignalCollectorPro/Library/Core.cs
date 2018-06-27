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

        static public void CollectCommand()
        {
            byte[] input = new byte[9];
            input[0] = 0xA6;
            input[1] = 0x6A;
            input[2] = 0x05;
            input[3] = 0x00;
            input[4] = 0x01;
            input[5] = 0x01;
            input[6] = 0x00;
            input[7] = 0xFF;
            input[8] = 0xFF;

            _mySerialPort.Write(input, 0, 9);
        }

        static public void RegularMode(int gap, int wait)
        {
            Core.CollectCommand();
            Thread.Sleep(gap);
            Core.CollectCommand();
            Thread.Sleep(gap);
            Core.CollectCommand();
            Thread.Sleep(wait);
            Core.DataCommand();
        }
        static public void DataCommand()
        {
            byte[] input = new byte[23];
            input[0] = 0xA6;
            input[1] = 0x6A;
            input[2] = 0x13;
            input[3] = 0x00;
            input[4] = 0x01;
            input[5] = 0x01;
            input[6] = 0x01;
            //sn
            input[7] = 0xAA;
            input[8] = 0xAA;
            input[9] = 0xAA;
            input[10] = 0xAA;
            input[11] = 0xAA;
            input[12] = 0xAA;
            input[13] = 0xAA;
            input[14] = 0xAA;
            input[15] = 0xAA;
            input[16] = 0xAA;
            input[17] = 0xAA;
            input[18] = 0xAA;
            input[19] = 0xAA;
            input[20] = 0xAA;

            input[21] = 0xFF;
            input[22] = 0xFF;

            _mySerialPort.Write(input, 0, 23);
        }
    }
}

