namespace StoreAPI.Data.Model
{
    class PurchaseRecord
    {
        public string UserId { get; set; }

        public string Product { get; set; }

        public double Quantity { get; set; }

        public double Cost { get; set; }

        public double TotalCost => Cost * Quantity;
    }
}
