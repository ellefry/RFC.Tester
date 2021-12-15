using Sap.Conn.Service.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sap.Conn.Service.Domains
{
    public class SapSwitcher : ISapSwitcher
    {
        public bool IsEnabled { 
            get => MvcApplication.SapOn; 
            set => MvcApplication.SapOn = value; 
        }
    }
}