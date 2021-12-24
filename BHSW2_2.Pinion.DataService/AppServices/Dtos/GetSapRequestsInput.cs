using System;

namespace BHSW2_2.Pinion.DataService.AppServices.Dtos
{
    public class GetSapRequestsInput
    {
        /// <summary>
        /// Finish/Scrap....
        /// </summary>
        public string SapRequestType { get; set; }

        public int? ItemCount { get; set; } = 100;
    }
}
