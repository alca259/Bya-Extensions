using System.IO;

namespace System.Net.Http
{
    /// <summary>
    /// Clase para devolver contenido multimedia
    /// </summary>
    public class FileContent : MultipartFormDataContent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="apiParamName"></param>
        public FileContent(string filePath, string apiParamName)
        {
            var filestream = File.Open(filePath, FileMode.Open);
            var filename = Path.GetFileName(filePath);

            Add(new StreamContent(filestream), apiParamName, filename);
        }
    }
}
