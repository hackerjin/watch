using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skyray.UC
{
    public partial class UCSingStand : Skyray.Language.UCMultiple
    {
        public UCSingStand()
        {
            InitializeComponent();
            this.lblfloorTxt.Text = this._layerName;
            this.textBoxW1.Text = this._standMin.ToString();
            this.textBoxW2.Text = this._standMax.ToString();
            
        }

        private string _layerName;
        private double _standMin;
        private double _standMax;
        private string _sname;

        public string SName
        {
            get { return _sname; }
            set { _sname = value; }
        }

        public string LayerName
        {
            get { return _layerName; }
            set { _layerName = value;
            this.lblfloorTxt.Text = value;
            }
        }

        public double StandMin
        {
            get
            {
                double min=0;
                double.TryParse(this.textBoxW1.Text, out min);
                _standMin = min;
                return _standMin;
            }
            set { _standMin = value;
            this.textBoxW1.Text = value.ToString();
            }
        }

        public double StandMax
        {
            //get { return _standMax; }
            get
            {
                double max=0;
                double.TryParse(this.textBoxW2.Text, out max);
                _standMax = max;
                return _standMax;
            }
            set { _standMax = value;
            this.textBoxW2.Text = value.ToString();
            }
        }

        private void lblFirstTxt_Click(object sender, EventArgs e)
        {

        }

        private void UCSingStand_Load(object sender, EventArgs e)
        {
           
        }


    }




   
    //public class SingleLayer
    //{
    //    public string LayerName { get; set; }
    //    public double StandMin { get; set; }
    //    public double StandMax { get; set; }

    //    public SingleLayer(string layerName, double standMin, double standMax)
    //    {
    //        this.LayerName = layerName;
    //        this.StandMin = standMin;
    //        this.StandMax = standMax;
    //    }
    //    //private string _layerName;
    //    //private double _standMin;
    //    //private double _standMax;

    //    //public string LayerName
    //    //{
    //    //    get { return _layerName; }
    //    //    set { _layerName = value; }
    //    //}

    //    //public double StandMin
    //    //{
    //    //    get { return _standMin; }
    //    //    set { _standMin = value; }
    //    //}

    //    //public double StandMax
    //    //{
    //    //    get { return _standMax; }
    //    //    set { _standMax = value; }
    //    //}
    //}
}
