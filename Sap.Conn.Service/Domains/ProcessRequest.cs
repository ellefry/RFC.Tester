﻿using System;
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
        public FunctionType FunctionType { get; set; }
        public string FunctionName { get; set; }
        public string Content { get; set; }
        public int Retries { get; set; }
        public string Error { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }

    public enum FunctionType
    {
        RFC,
        WebService
    }
}
