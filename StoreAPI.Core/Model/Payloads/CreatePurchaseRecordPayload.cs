namespace StoreAPI.Core.Model.Payloads
{
    public class CreatePurchaseRecordPayload
    {
        public int UserNationalId { get; set; }

        public string Product { get; set; }

        public double Quantity { get; set; }

        public double Cost { get; set; }
    }
}
