using Newtonsoft.Json.Linq;

namespace Lychee.Scrapper.Domain.Extensions
{
    public static class JTokenExtension
    {
        public static string GetValue(this JToken token, string defaultValue = "")
        {
            return token != null ? token.Value<string>() : defaultValue;
        }
    }
}
