using System;
using System.ComponentModel.DataAnnotations;

namespace BHSW2_2.Pinion.DataService
{
    public class SapRequest
    {
        [Key]
        public Guid Id { get; set; }
        public string FunctionName { get; set; }
        public SapRequestStatus SapRequestStatus { get; set; } = SapRequestStatus.Scheduled;
        public string Content { get; set; }
        public int Retries { get; set; }
        public string Error { get; set; }
        public DateTimeOffset? Created { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? Modified { get; set; }

        public int ProcessOrder { get; set; }

        public void UpdateRetry(string error)
        {
            SapRequestStatus = SapRequestStatus.Failed;
            Retries++;
            Error = error;
            Modified = DateTimeOffset.Now;
        }

        public void UpateProcessOrder(string content, int processOrder)
        {
            Content = content;
            ProcessOrder = processOrder;
            Modified = DateTimeOffset.Now;
        }
    }

    public enum SapRequestStatus
    {
        Scheduled,
        Failed
    }
}
