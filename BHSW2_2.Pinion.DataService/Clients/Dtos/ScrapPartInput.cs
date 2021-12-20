namespace BHSW2_2.Pinion.DataService.Clients.Dtos
{
    public class ScrapPartInput
    {
        public string ScrapNumber { get; set; }
        public string ParentMaterial { get; set; }
        /// <summary>
        /// 报废工位号
        /// </summary>
        public string Vornr { get; set; }
        public string Plant { get; set; }
        public string StorageLocation { get; set; }
        public string Material { get; set; }
        public int Quantity { get; set; }
        public int ScrapReason { get; set; }
    }
}
