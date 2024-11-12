using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skyray.EDX.Common
{

    public class ImageSample
    {
        public string ImagePath;
    }

    public class TotalAnalysisReport
    {
        private string _sampleName;

        public string SampleName
        {
            get { return _sampleName; }
            set { _sampleName = value; }
        }

        private int _testTimes;

        public int TestTimes
        {
            get { return _testTimes; }
            set { _testTimes = value; }
        }

        private System.Drawing.Image _sample;

        public System.Drawing.Image Sample
        {
            get { return _sample; }
            set {
                _sample = value; }
        }

        private string _supplier;

        public string Supplier
        {
            get { return _supplier; }
            set { _supplier = value; }
        }

        private int _voltage;

        public int Voltage
        {
            get { return _voltage; }
            set { _voltage = value; }
        }

        private string _operator;

        public string Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        private int _current;//管流

        public int _Current
        {
            get { return _current; }
            set { _current = value; }
        }

        private string _lotNO;

        public string LotNO
        {
            get { return _lotNO; }
            set { _lotNO = value; }
        }

        private string _mode;

        public string Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private string _pendNO;

        public string PendNO
        {
            get { return _pendNO; }
            set { _pendNO = value; }
        }

        private string _submittedUnit;

        public string SubmittedUnit
        {
            get { return _submittedUnit; }
            set { _submittedUnit = value; }
        }

        private string _testDate;

        public string TestDate
        {
            get { return _testDate; }
            set { _testDate = value; }
        }

        private string _workCurveName;

        public string WorkCurveName
        {
            get { return _workCurveName; }
            set { _workCurveName = value; }
        }

        private string _number;

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private System.Data.DataTable _dataTable;

        public System.Data.DataTable DataTable
        {
            get { return _dataTable; }
            set { _dataTable = value; }
        }
    }
}
