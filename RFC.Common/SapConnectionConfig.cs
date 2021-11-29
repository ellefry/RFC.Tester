using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common
{
    public class SapConnectionConfig
    {
        [DefaultValue("")]
        public string IPAddress { get; set; }
        [DefaultValue("")]
        public string SystemNumber { get; set; }
        [DefaultValue("")]
        public string SystemID { get; set; }
        [DefaultValue("")]
        public string User { get; set; }
        [DefaultValue("")]
        public string Password { get; set; }
        [DefaultValue("")]
        public string ReposotoryPassword { get; set; }
        [DefaultValue("")]
        public string Client { get; set; }
        [DefaultValue("")]
        public string Language { get; set; }
        [DefaultValue("")]
        public string PoolSize { get; set; }
    }
}
