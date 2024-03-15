using System.Net.Http.Headers;

namespace Todoist.Net
{
    public class TodoistClient : ITodoistClient
    {
        private readonly HttpClient _httpClient;

        public TodoistClient(string token)
        {
            _httpClient = new HttpClient()
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

        public async Task<HttpResponseMessage> TestGetRequest()
        {
            return await _httpClient.GetAsync("/projects");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
