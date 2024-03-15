using System.Net;
using System.Net.Http.Headers;

namespace Todoist.Net
{
    internal sealed class TodoistRestClient : ITodoistRestClient
    {
        private readonly HttpClient _httpClient;

        public TodoistRestClient()
            : this(null, null) { }

        public TodoistRestClient(string token)
            : this(token, null) { }

        public TodoistRestClient(IWebProxy proxy)
            : this(null, proxy) { }

        public TodoistRestClient(string token, IWebProxy proxy)
        {
            var httpClientHandler = new HttpClientHandler();
            if (proxy != null)
            {
                httpClientHandler.Proxy = proxy;
                httpClientHandler.UseProxy = true;
            }

            _httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri("https://api.todoist.com/sync/v9/")
            };

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<HttpResponseMessage> PostAsync(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters,
            CancellationToken cancellationToken = default
        )
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(resource));
            }

            using (var content = new FormUrlEncodedContent(parameters))
            {
                return await _httpClient
                    .PostAsync(resource, content, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task<HttpResponseMessage> PostFormAsync(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters,
            IEnumerable<ByteArrayContent> files,
            CancellationToken cancellationToken = default
        )
        {
            using (var multipartFormDataContent = new MultipartFormDataContent())
            {
                foreach (var keyValuePair in parameters)
                {
                    multipartFormDataContent.Add(
                        new StringContent(keyValuePair.Value),
                        $"\"{keyValuePair.Key}\""
                    );
                }

                foreach (var file in files)
                {
                    multipartFormDataContent.Add(
                        file,
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString()
                    );
                }

                return await _httpClient
                    .PostAsync(resource, multipartFormDataContent, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
