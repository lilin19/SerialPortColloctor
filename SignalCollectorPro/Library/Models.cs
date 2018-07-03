using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalCollectorPro
{
    class DataObjects
    {
        public class Data
        {
            public double _temperature;
            public double _measurement;
            public double _error;
            public DateTime _time;

            public Data(double temp, double measurement, double err)
            {
                this._temperature = temp;
                this._measurement = measurement;
                this._error = err;
                this._time = DateTime.Now;
            }
            public string ToString()
            {
                return _temperature.ToString() + "   " + _measurement.ToString();
            }

            public string GetMeasurement()
            {
                return _measurement.ToString();
            }

            public string GetTemperature()
            {
                return _temperature.ToString();
            }


        }

        public class SaveMeasure
        {
            public double _temperature;
            public double _measurement;
            public double _realtemperature;
            public double _realmeasurement;
            public DateTime _time;
            public SaveMeasure(double temp,double measure, double realtemp, double realmeasure)
            {
                _temperature = temp;
                _measurement = measure;
                _realmeasurement = realmeasure;
                _realtemperature = realtemp;
                _time = DateTime.Now;
            }
        }

        public class SN
        {
            public string sn;

            public SN(string sn)
            {
                this.sn = sn;
            }
        }

    }
}
