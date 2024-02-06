namespace API_Verification.Tests.ResponsePayload.GetUserByPage
{
    public class GetUserByPage
    {
        public int? Page { get; set; }
        public int? Per_Page { get; set; }
        public int? Total { get; set; }
        public int? Total_Pages { get; set; }
        public Datum[]? Data { get; set; }
        public Support? Support { get; set; }
    }

    public class Support
    {
        public string? Url { get; set; }
        public string? Text { get; set; }
    }

    public class Datum
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
    }
}