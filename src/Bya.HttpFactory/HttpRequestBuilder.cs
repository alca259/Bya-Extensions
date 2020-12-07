using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Builder
    /// </summary>
    public class HttpRequestBuilder : IDisposable
    {
        private HttpMethod method = null;
        private string requestUri = "";
        private HttpContent content = null;
        private string bearerToken = "";
        private string acceptHeader = "application/json";
        private TimeSpan timeout = new TimeSpan(0, 0, 30);
        private bool allowAutoRedirect = false;
        private Action<HttpResponseMessage> successCallback = null;
        private Action<string> failedCallback = null;

        /// <summary>
        /// Ctor
        /// </summary>
        public HttpRequestBuilder()
        {
        }

        /// <summary>
        /// Añade un método (GET/POST/ETC)
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        /// <summary>
        /// Añade la URL
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }

        /// <summary>
        /// Añade el contenido
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        /// <summary>
        /// Añade el token JWT
        /// </summary>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }

        /// <summary>
        /// Añade cabeceras permitidas
        /// </summary>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }

        /// <summary>
        /// Añade un timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        /// <summary>
        /// Añade una autoredirección
        /// </summary>
        /// <param name="allowAutoRedirect"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
            return this;
        }

        /// <summary>
        /// Añade una petición de callback al acabar bien
        /// </summary>
        /// <param name="successCallback"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddSuccessCallback(Action<HttpResponseMessage> successCallback)
        {
            this.successCallback = successCallback;
            return this;
        }

        /// <summary>
        /// Añade una petición de callback al acabar mal
        /// </summary>
        /// <param name="failedCallback"></param>
        /// <returns></returns>
        public HttpRequestBuilder AddFailedCallback(Action<string> failedCallback)
        {
            this.failedCallback = failedCallback;
            return this;
        }

        /// <summary>
        /// Lo que debe hacer para eliminar basura de memoria
        /// </summary>
        public void Dispose()
        {
            successCallback = null;
            failedCallback = null;
        }

        /// <summary>
        /// Construye la petición y la envía
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            EnsureArguments();

            try
            {
                // Setup request
                var request = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(this.requestUri)
                };

                if (content != null)
                {
                    request.Content = content;
                }

                if (!string.IsNullOrEmpty(bearerToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                request.Headers.Accept.Clear();
                if (!string.IsNullOrEmpty(acceptHeader))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));
                }

                // Setup client
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = allowAutoRedirect
                };

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = timeout;

                    var response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        failedCallback?.Invoke(response.ReasonPhrase);
                    }
                    else
                    {
                        successCallback?.Invoke(response);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                failedCallback?.Invoke(ex.Message);
            }

            return null;
        }

        private void EnsureArguments()
        {
            if (method == null)
                throw new ArgumentNullException("Method");

            if (string.IsNullOrEmpty(this.requestUri))
                throw new ArgumentNullException("Request Uri");

            if (successCallback == null)
            {
                AddSuccessCallback((res) =>
                {
                    Dispose();
                });
            }

            if (failedCallback == null)
            {
                AddFailedCallback((res) =>
                {
                    Dispose();
                });
            }
        }
    }
}
