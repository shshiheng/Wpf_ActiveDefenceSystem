using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.DynamicDataDisplay.Common;

namespace WpfActiveDefenceSystem.PlotViewModel
{
    public class PlotPointCollection : RingArray <PlotPoint>
    {
        private const int TOTAL_POINTS = 300;

        public PlotPointCollection()
            : base(TOTAL_POINTS) // here i set how much values to show 
        {    
        }
    }

    public class PlotPoint
    {
        public int Time { get; set; }

        public double Value { get; set; }

        public PlotPoint(double value, int time)
        {
            this.Time = time;
            this.Value = value;
        }
    }
}
