using API_Application.RequestPayload;
using System.Text.Json;

namespace API_Application
{
    public static class ConstructPayload
    {
        public static string ConstructCreateUserPayload(dynamic payload)
        {
            var userDetails = new CreatUser()
            {
                Name = payload.CreateUserApiDetails.name.ToString(),
                Job = payload.CreateUserApiDetails.job.ToString(),
            };
            var jsonPayload = JsonSerializer.Serialize(userDetails);
            return jsonPayload;
        }
    }
}