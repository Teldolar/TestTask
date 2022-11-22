using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApplication.Models
{
    public class DataItem
    {
        public DateTime DateTime { get; set; }
        public string Event { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public string[] GetStringArray()
        {
            return new string[] {
                DateTime.ToString(),
                Event,
                PositionX.ToString(),
                PositionY.ToString()
            };
        }
    }
}
