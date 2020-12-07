using Newtonsoft.Json;
using System.Text;

namespace System.Net.Http
{
    /// <summary>
    /// Contenido patch
    /// </summary>
    public class PatchContent : StringContent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public PatchContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json-patch+json")
        {

        }
    }
}
