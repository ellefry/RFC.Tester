using System;

namespace BHSW2_2.Pinion.DataService.AppServices.Dtos
{
    public class ReSendSapRequestInput
    {
        public Guid SapRequestId { get; set; }
        public string Content { get; set; }
        public int ProcessOrder { get; set; }
    }
}
