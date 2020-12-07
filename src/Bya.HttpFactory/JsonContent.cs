using Newtonsoft.Json;
using System.Text;

namespace System.Net.Http
{
    /// <summary>
    /// Contenido en JSON
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Constructor con mimetype
        /// </summary>
        /// <param name="value"></param>
        public JsonContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
        {

        }

        /// <summary>
        /// Constructor con mimetype personalizado
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mediaType"></param>
        public JsonContent(object value, string mediaType)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
        {

        }
    }
}
