using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sap.Conn.Service.Domains
{
    public class ProcessRequest
    {
        [Key]
        public Guid Id { get; set; }
        public int FunctionType { get; set; }
    }
}
