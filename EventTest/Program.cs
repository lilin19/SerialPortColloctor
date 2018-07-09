using System;

namespace EventSample
{
    // Class that contains the data for
    // the alarm event. Derives from System.EventArgs.
    //
    public class AlarmEventArgs : EventArgs
    {
        private bool snoozePressed;
        private int nrings;

        //Constructor.
        //

        public void ModRTU_CRC(ref byte[] buf, int len)
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
        public AlarmEventArgs(bool snoozePressed, int nrings)
        {
            this.snoozePressed = snoozePressed;
            this.nrings = nrings;
        }

        // The NumRings property returns the number of rings
        // that the alarm clock has sounded when the alarm event
        // is generated.
        //
        public int NumRings
        {
            get { return nrings; }
        }

        // The SnoozePressed property indicates whether the snooze
        // button is pressed on the alarm when the alarm event is generated.
        //
        public bool SnoozePressed
        {
            get { return snoozePressed; }
        }

        // The AlarmText property that contains the wake-up message.
        //
        public string AlarmText
        {
            get
            {
                if (snoozePressed)
                {
                    return ("Wake Up!!! Snooze time is over.");
                }
                else
                {
                    return ("Wake Up!");
                }
            }
        }
    }

    // Delegate declaration.
    //
    public delegate void AlarmEventHandler(object sender, AlarmEventArgs e);

    // The Alarm class that raises the alarm event.
    //
    public class AlarmClock
    {
        private bool snoozePressed = false;
        private int nrings = 0;
        private bool stop = false;

        // The Stop property indicates whether the
        // alarm should be turned off.
        //
        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        // The SnoozePressed property indicates whether the snooze
        // button is pressed on the alarm when the alarm event is generated.
        //
        public bool SnoozePressed
        {
            get { return snoozePressed; }
            set { snoozePressed = value; }
        }

        // The event member that is of type AlarmEventHandler.
        //
        public event AlarmEventHandler Alarm;

        // The protected OnAlarm method raises the event by invoking
        // the delegates. The sender is always this, the current instance
        // of the class.
        //
        protected virtual void OnAlarm(AlarmEventArgs e)
        {
            AlarmEventHandler handler = Alarm;
            if (handler != null)
            {
                // Invokes the delegates.
                handler(this, e);
            }
        }

        // This alarm clock does not have
        // a user interface.
        // To simulate the alarm mechanism it has a loop
        // that raises the alarm event at every iteration
        // with a time delay of 300 milliseconds,
        // if snooze is not pressed. If snooze is pressed,
        // the time delay is 1000 milliseconds.
        //
        public void Start()
        {
            for (; ; )
            {
                nrings++;
                if (stop)
                {
                    break;
                }
                else
                {
                    if (snoozePressed)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(300);
                    }
                    AlarmEventArgs e = new AlarmEventArgs(snoozePressed, nrings);
                    OnAlarm(e);
                }
            }
        }
    }

    // The WakeMeUp class has a method AlarmRang that handles the
    // alarm event.
    //
    public class WakeMeUp
    {
        public void AlarmRang(object sender, AlarmEventArgs e)
        {
            Console.WriteLine(e.AlarmText + "\n");

            if (!(e.SnoozePressed))
            {
                if (e.NumRings % 10 == 0)
                {
                    Console.WriteLine(" Let alarm ring? Enter Y");
                    Console.WriteLine(" Press Snooze? Enter N");
                    Console.WriteLine(" Stop Alarm? Enter Q");
                    String input = Console.ReadLine();

                    if (input.Equals("Y") || input.Equals("y"))
                    {
                        return;
                    }
                    else if (input.Equals("N") || input.Equals("n"))
                    {
                        ((AlarmClock)sender).SnoozePressed = true;
                        return;
                    }
                    else
                    {
                        ((AlarmClock)sender).Stop = true;
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine(" Let alarm ring? Enter Y");
                Console.WriteLine(" Stop Alarm? Enter Q");
                String input = Console.ReadLine();
                if (input.Equals("Y") || input.Equals("y"))
                {
                    return;
                }
                else
                {
                    ((AlarmClock)sender).Stop = true;
                    return;
                }
            }
        }
    }


    // The driver class that hooks up the event handling method of
    // WakeMeUp to the alarm event of an Alarm object using a delegate.
    // In a forms-based application, the driver class is the
    // form.
    //
    public class AlarmDriver
    {
        public static void Main(string[] args)
        {
            // Instantiates the event receiver.
            WakeMeUp w = new WakeMeUp();

            // Instantiates the event source.
            AlarmClock clock = new AlarmClock();

            // Wires the AlarmRang method to the Alarm event.
            clock.Alarm += new AlarmEventHandler(w.AlarmRang);

            clock.Start();
        }
    }
}

