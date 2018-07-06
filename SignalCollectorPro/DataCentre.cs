using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    class DataCentre
    {
        public static Data CurrentData;
        public static string CurrentSignal;
        public static int CurrentSignalLength;
        public static DateTime CurrentSignalTime;
        public static SN CurrentSN;
        public static bool Timeout;
    }
}
