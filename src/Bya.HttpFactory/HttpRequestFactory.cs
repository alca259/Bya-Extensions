using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Factoría para crear peticiones
    /// </summary>
    public static class HttpRequestFactory
    {
        /// <summary>
        /// Peticiones GET
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(string requestUri) => await Get(requestUri, "");

        /// <summary>
        /// Peticiones GET con Token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        /// <summary>
        /// Peticiones POST
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post(string requestUri, object value) => await Post(requestUri, value, "");

        /// <summary>
        /// Peticiones POST con Token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        /// <summary>
        /// Peticiones PUT
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Put(string requestUri, object value) => await Put(requestUri, value, "");

        /// <summary>
        /// Peticiones PUT con Token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Put(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        /// <summary>
        /// Peticiones PATCH
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Patch(string requestUri, object value) => await Patch(requestUri, value, "");

        /// <summary>
        /// Peticiones PATCH con Token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Patch(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(new HttpMethod("PATCH"))
                                .AddRequestUri(requestUri)
                                .AddContent(new PatchContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        /// <summary>
        /// Peticiones DELETE
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Delete(string requestUri) => await Delete(requestUri, "");

        /// <summary>
        /// Peticiones DELETE con Token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Delete(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        /// <summary>
        /// Peticiones POST con ruta de fichero
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="filePath"></param>
        /// <param name="apiParamName"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostFile(string requestUri, string filePath, string apiParamName) => await PostFile(requestUri, filePath, apiParamName, "");

        /// <summary>
        /// Peticiones POST con ruta de fichero y Token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="filePath"></param>
        /// <param name="apiParamName"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostFile(string requestUri, string filePath, string apiParamName, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new FileContent(filePath, apiParamName))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }
    }
}
