namespace StoreAPI.Core.Model.Payloads
{
    public class CreateUserPayload
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int NationalId { get; set; }
    }
}
