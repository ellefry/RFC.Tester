using System;
using System.ComponentModel.DataAnnotations;

namespace BHSW2_2.Pinion.DataService
{
    public class SapRequestHistory
    {
        [Key]
        public Guid Id { get; set; }
        public string FunctionName { get; set; }
        public string Content { get; set; }
        public string SapMessage { get; set; }
        public DateTimeOffset? Created { get; set; }
    }
}