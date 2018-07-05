using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;


namespace SerialPortWriter
{
    class Program
    {
        static SerialPort _mySerialPort = new SerialPort("COM2");
       static System.Timers.Timer _t = new System.Timers.Timer(5000);
       
        static void Main(string[] args)
        {
            _mySerialPort.BaudRate = 9600;
            _mySerialPort.Parity = Parity.None;
            _mySerialPort.StopBits = StopBits.One;
            _mySerialPort.DataBits = 8;
            _mySerialPort.Handshake = Handshake.None;
            _mySerialPort.Open();
            //_mySerialPort.ReceivedBytesThreshold = 23;
            _t.Elapsed += OnTimedEvent;
            _t.AutoReset = true;
            _t.Enabled = true;
            _t.Start();

            Console.ReadKey();
            _t.Stop();
            _t.Dispose();
            _mySerialPort.Close();
        }

            private static void SetReceiver()
            {
                _mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                Console.ReadKey();
                _mySerialPort.Close();

            }
            private static void DataReceivedHandler(
                        object receiver,
                        SerialDataReceivedEventArgs e)
            {
                Thread.Sleep(50);

                SerialPort sp = (SerialPort)receiver;
                byte[] tst = new byte[sp.BytesToRead];
                //string get = sp.ReadExisting();
                sp.Read(tst, 0, sp.BytesToRead);
                //Int16 get = BitConverter.ToInt16(tst,0);

                Console.WriteLine("Data byte: {0}", tst.Length);
                for (int i = 0; i < tst.Length; i++)
                {
                    Console.WriteLine("Data Received: {0}", tst[i]);
                }


                string hexValue = BitConverter.ToString(tst);
                if (hexValue != "")
                {
                    FileWrite("test.txt", hexValue);
                }
            }


            static public void FileWrite(string path, string hex)
            {
                FileStream myStream = new FileStream(@"test.txt", FileMode.Append, FileAccess.Write);
                StreamWriter sWriter = new StreamWriter(myStream);
                sWriter.WriteLine(DateTime.Now + " Receive: " + hex);

                sWriter.Close();
                myStream.Close();

                //string info = new UTF8Encoding(true).GetString(content);
                // info = content;
                // Add some information to the file.
            }

            static public int DataLength(byte[] input)
            {
                return 0;
            }

          
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
                byte[] input = new byte[31];
                for (int i = 0; i < 30; i++)
                {
                    input[i] = 255;
                }
                _mySerialPort.Write(input, 0, 31);
                Console.WriteLine("Sent" + BitConverter.ToString(input));
            }

        }

    }



