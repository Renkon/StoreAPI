namespace StoreAPI.Core.Dto
{
    public class PurchaseRecordDto
    {
        public int UserNationalId { get; set; }

        public string Product { get; set; }

        public double Quantity { get; set; }

        public double Cost { get; set; }

        public double TotalCost => Cost * Quantity;
    }
}
