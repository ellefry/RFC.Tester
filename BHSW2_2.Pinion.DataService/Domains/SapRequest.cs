﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BHSW2_2.Pinion.DataService
{
    public class SapRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string FunctionName { get; set; }
        public SapRequestStatus SapRequestStatus { get; set; } = SapRequestStatus.Scheduled
        public string Content { get; set; }
        public int Retries { get; set; }
        public string Error { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }

        public void UpdateRetry(string error)
        {
            Retries++;
            Error = error;
            Modified = DateTimeOffset.Now;
        }
    }

    public enum SapRequestStatus
    {
        Scheduled,
        Failed
    }
}
