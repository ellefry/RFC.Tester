using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSW2_2.Pinion.DataService.Interfaces
{
    public interface ISapSwitcher
    {
        Task EnableSwitcher(bool isEnabled);
        Task<bool> GetSwitcherStatus();
    }
}
