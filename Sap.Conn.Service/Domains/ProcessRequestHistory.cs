using System;
using System.ComponentModel.DataAnnotations;

namespace Sap.Conn.Service.Domains
{
    public class ProcessRequestHistory
    {
        [Key]
        public Guid Id { get; set; }
        public FunctionType FunctionType { get; set; }
        public string FunctionName { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? Created { get; set; }
    }
}