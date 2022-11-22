using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.DB.Models
{
    public class MouseLogs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public virtual Users User { get; set; }
    }
}
