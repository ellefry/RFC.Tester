using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SapWebService.Common.Abstracts
{
    public interface IOutboundService
    {
        Task Transfer();
    }
}
