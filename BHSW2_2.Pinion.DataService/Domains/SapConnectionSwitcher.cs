using System;
using System.ComponentModel.DataAnnotations;

namespace BHSW2_2.Pinion.DataService.Domains
{
    public class SapConnectionSwitcher
    {
        [Key]
        public string Id { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
    }
}
