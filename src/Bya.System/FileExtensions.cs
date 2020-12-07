using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    /// <summary>
    /// Extensiones de ficheros
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        /// and the official specifications for the file types you wish to add.
        /// </summary>
        public static Dictionary<string, List<byte[]>> FileSignatures { get; private set; } = new Dictionary<string, List<byte[]>>
        {
            { ".bmp", new List<byte[]>
                {
                    new byte[] { 0x42, 0x4D }
                }
            },
            { ".gif", new List<byte[]>
                {
                    new byte[] { 0x47, 0x49, 0x46, 0x38 },
                }
            },
            { ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                }
            },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".zip", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                }
            },
            { ".rar", new List<byte[]>
                {
                    new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 },
                }
            },
            { ".doc", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },
                    new byte[] { 0x0D, 0x44, 0x4F, 0x43 },
                    new byte[] { 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1, 0x00 },
                    new byte[] { 0xDB, 0xA5, 0x2D, 0x00 },
                    new byte[] { 0xEC, 0xA5, 0xC1, 0x00 },
                }
            },
            { ".docx", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },
                }
            },
            { ".xls", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },
                    new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x10 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x1F },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x22 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x23 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x28 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x29 },
                }
            },
            { ".xlsx", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },
                }
            },
            { ".pdf", new List<byte[]>
                {
                    new byte[] { 0x25, 0x50, 0x44, 0x46 },
                }
            },
            { ".ppt", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCD, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },
                    new byte[] { 0x00, 0x6E, 0x1E, 0xF0 },
                    new byte[] { 0x0F, 0x00, 0xE8, 0x03 },
                    new byte[] { 0xA0, 0x46, 0x1D, 0xF0 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x0E, 0x00, 0x00, 0x00 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x1C, 0x00, 0x00, 0x00 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x43, 0x00, 0x00, 0x00 },
                }
            },
            { ".pptx", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },
                }
            }
        };

        /// <summary>
        /// Añade una extensión con su firma (solo en runtime)
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="byteArrays"></param>
        public static void AddExtension(string ext, List<byte[]> byteArrays)
        {
            if (FileSignatures.ContainsKey(ext)) return;
            FileSignatures.Add(ext, byteArrays);
        }

        /// <summary>
        /// Elimina una extensión con su firma (solo en runtime)
        /// </summary>
        /// <param name="ext"></param>
        public static void RemoveExtension(string ext)
        {
            if (!FileSignatures.ContainsKey(ext)) return;
            FileSignatures.Remove(ext);
        }

        /// <summary>
        /// Comprueba la cabecera de bytes de números mágicos para comprobar la extensión
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool CheckSignature(string fileName, Stream data)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    // Limits characters to ASCII encoding.
                    for (var i = 0; i < data.Length; i++)
                    {
                        if (reader.ReadByte() > sbyte.MaxValue)
                        {
                            return false;
                        }
                    }

                    return true;
                }

                // Uncomment the following code block if you must permit
                // files whose signature isn't provided in the FileSignatures
                // dictionary. We recommend that you add file signatures
                // for files (when possible) for all file types you intend
                // to allow on the system and perform the file signature
                // check.
                /*
                if (!FileSignatures.ContainsKey(ext))
                {
                    return true;
                }
                */

                // File signature check
                // --------------------
                // With the file signatures provided in the FileSignatures
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = FileSignatures[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}
