using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Perpetuum.Network
{
    public static class Http
    {
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Posts a request and returns the reply
        /// </summary>
        public static string Post(string address, IEnumerable<KeyValuePair<string, object>> data)
        {
            var stringData = data.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value.ToString()));
            using var content = new FormUrlEncodedContent(stringData);

            var request = new HttpRequestMessage(HttpMethod.Post, address)
            {
                Content = content
            };
            request.Headers.UserAgent.ParseAdd("PerpetuumServer/1.0");

            var response = _client.SendAsync(request).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
