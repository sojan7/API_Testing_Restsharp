namespace API_Verification.Tests.ResponsePayload.GetUserById
{
    public class GetUserById
    {
        public Data? Data { get; set; }
        public Support? Support { get; set; }
    }

    public class Data
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set; }
        public string? Avatar { get; set; }
    }

    public class Support
    {
        public string? Url { get; set; }
        public string? Text { get; set; }
    }
}