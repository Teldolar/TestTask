using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.DB.Models
{
    public class Users
    {
        public Users()
        {
            MouseLogs = new HashSet<MouseLogs>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }

        public virtual ICollection<MouseLogs> MouseLogs { get; set; }
    }
}
