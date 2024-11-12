using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{
    public class SelfCheckObject
    {
        private string _HVLock;

        public string HVLock
        {
            get { return _HVLock; }
            set { _HVLock = value; }
        }

        private string _Collimator;

        public string Collimator
        {
            get { return _Collimator; }
            set { _Collimator = value; }
        }

        private string _Filter;

        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        private string _HVoltage;

        public string HVoltage
        {
            get { return _HVoltage; }
            set { _HVoltage = value; }
        }

        private string _Resolve;

        public string Resolve
        {
            get { return _Resolve; }
            set { _Resolve = value; }
        }

        private string _Peak;

        public string Peak
        {
            get { return _Peak; }
            set { _Peak = value; }
        }

        private string _Pump;

        public string Pump
        {
            get { return _Pump; }
            set { _Pump = value; }
        }

        private string _HalfWidth;

        public string HalfWidth
        {
            get { return _HalfWidth; }
            set { _HalfWidth = value; }
        }

        private string _PeakSec;

        public string PeakSec
        {
            get { return _PeakSec; }
            set { _PeakSec = value; }
        }

        private string _CountRate;

        public string CountRate
        {
            get { return _CountRate; }
            set { _CountRate = value; }
        }

        private string _Result;

        public string Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        private string _Operator;

        public string Operator
        {
            get { return _Operator; }
            set { _Operator = value; }
        }

        private string _Date;

        public string Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        private string _Device;

        public string Device
        {
            get { return _Device; }
            set { _Device = value; }
        }

        private string _SpecTimes;

        public string SpecTimes
        {
            get { return _SpecTimes; }
            set { _SpecTimes = value; }
        }

        private string _CollimatorId;

        public string CollimatorId
        {
            get { return _CollimatorId; }
            set { _CollimatorId = value; }
        }

        private string _FilterId;

        public string FilterId
        {
            get { return _FilterId; }
            set { _FilterId = value; }
        }

        private string _Voltage;

        public string Voltage
        {
            get { return _Voltage; }
            set { _Voltage = value; }
        }

        private string _Current;

        public string Current
        {
            get { return _Current; }
            set { _Current = value; }
        }

        public SelfCheckObject()
        {
            _HVLock = "--";
            _Collimator = "--";
            _Filter = "--";
            _HVoltage = "--";
            _Resolve = "--";
            _Peak = "--";
            _Pump = "--";
            _HalfWidth = "--";
            _PeakSec = "--";
            _CountRate = "--";
            _Result = "Pass";
            _Operator = "--";
            _SpecTimes = "--";
            _Device = "--";
            _CollimatorId = "--";
            _FilterId = "--";
            _Voltage = "--";
            _Current = "--";
            _Date = "--";
        }
    }
}
