using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common.Interfaces
{
  public interface IRfcManager
  {
        void ProcessRequest(ProcessRequestInput input);
  }
}
