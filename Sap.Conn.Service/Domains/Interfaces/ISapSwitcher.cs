using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sap.Conn.Service.Domains.Interfaces
{
    public interface ISapSwitcher
    {
        bool IsEnabled { get; set; }
    }
}
