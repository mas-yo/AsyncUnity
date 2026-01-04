using System.Collections.Generic;

namespace Genie
{
    public static class CreateHttpHeader
    {
        public static (string key, string value)[] Do(string authToken, string apiVersion)
        {
            var headers = new List<(string key, string value)>()
            {
                ("Content-Type", "application/x-msgpack"),
                ("Accept", "application/x-msgpack"),
                ("X-Polka-ApiVersion", apiVersion),
            };
            if (!string.IsNullOrWhiteSpace(authToken))
            {
                headers.Add(("Authorization", $"Bearer {authToken}"));
            }

            return headers.ToArray();
        }
        
    }
}