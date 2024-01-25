using Newtonsoft.Json.Linq;

namespace API_Helper
{
    public static class JsonHelper
    {
        public static JObject GetTestData(string path)
        {
            return JObject.Parse(File.ReadAllText(path));
        }
    }
}