namespace API_Application
{
    public static class ReqresApplication
    {
        public static readonly string BaseUrl = GetBaseUrl();

        private static string GetBaseUrl()
        {
            return "https://reqres.in/";
        }
    }
}