namespace BHSW2_2.Pinion.DataService.Clients.Dtos
{
    public class FinishPartInput
    {
        public string Plant { get; set; }
        public string Material { get; set; }
        public int Quantity { get; set; }
        public string Sku { get; set; }
        public string ProductVersion { get; set; }
        public string OprNumber { get; set; }
        public string ZProduction { get; set; }
    }
}
