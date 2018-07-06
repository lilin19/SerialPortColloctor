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
        public static string sn = "UC001807020001";

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
            _mySerialPort.Open();
        }

        public static void StartListen(SerialPort port, SerialDataReceivedEventHandler handler)
        {
            port.DataReceived += handler;

        }



        static public int DataLength(byte[] input)
        {
            return 0;
        }


        static public void CollectCommand(SerialPort port)
        {
            ModRTU_CRC(ref _collectrequest, 7);
            port.Write(_collectrequest, 0, 9);
        }

        static public void DataCommand(SerialPort port)
        {
            byte[] d = Encoding.ASCII.GetBytes(sn);
            Array.Copy(d, 0, _datarequest, 7, 14);
            ModRTU_CRC(ref _datarequest, 21);
            port.Write(_datarequest, 0, 23);
        }


        static public void ModRTU_CRC(ref byte[] buf, int len)
        {
            UInt16 crc = 0xFFFF;

            for (int pos = 0; pos < len; pos++)
            {
                crc ^= (UInt16)buf[pos]; // XOR byte into least sig. byte of crc

                for (int i = 8; i != 0; i--)
                { // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    { // If the LSB is set
                        crc >>= 1; // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else // Else LSB is not set
                        crc >>= 1; // Just shift right
                }
            }
            byte[] o = BitConverter.GetBytes(crc);
            // Note, this number has low and high bytes swapped, so use it accordingly (or swap bytes)
            buf[len] = o[1];
            buf[len + 1] = o[0];
        }


    }
}

