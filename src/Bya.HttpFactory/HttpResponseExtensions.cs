using Newtonsoft.Json;

namespace System.Net.Http
{
    /// <summary>
    /// Extensiones de Http
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Tipo de como va a leer el contenido
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(data) ? default(T) : JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Va a leer el contenido como Json
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Va a leer el contenido como string
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
