using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Bya.XmlExport
{
    /// <summary>
    /// Asistente para guardado, carga y borrado de ficheros XML
    /// </summary>
    public static class XmlAssistant
    {
        private static string GetRealName(this PropertyInfo prop)
        {
            var args = prop.PropertyType.IsGenericType && !prop.PropertyType.IsGenericTypeDefinition
                ? prop.PropertyType.GetGenericArguments()
                : Type.EmptyTypes;

            if (args.Any())
            {
                return args[0].FullName;
            }

            return prop.PropertyType.FullName;
        }

        /// <summary>
        /// Directorio donde van a almacenarse los datos
        /// </summary>
        public static string DataDirectory { get; set; }

        private static readonly object lockFile = new object();

        private static void CheckDirectory()
        {
            if (Directory.Exists(DataDirectory)) return;
            Directory.CreateDirectory(DataDirectory);
        }

        private static XElement GetXElement<T>(T item)
        {
            if (item == null) return null;
            var itemType = item.GetType();
            var name = itemType.Name.ToUpperInvariant();
            var props = itemType.GetProperties().ToList();
            var itemXml = new XElement(name);

            foreach (PropertyInfo prop in props)
            {
                if (!prop.CanRead) continue;
                var propName = prop.Name.ToUpperInvariant();
                var value = prop.GetValue(item, null);
                if (value == null)
                {
                    value = "";
                }

                var propType = prop.PropertyType.FullName;

                switch (propType)
                {
                    case "System.Int32":
                    case "System.UInt32":
                        itemXml.SetAttributeValue(propName, value.ToString().Trim());
                        break;
                    case "System.Int64":
                    case "System.UInt64":
                        itemXml.SetAttributeValue(propName, value.ToString().Trim());
                        break;
                    case "System.Single":
                    case "System.Double":
                    case "System.Decimal":
                        itemXml.SetAttributeValue(propName, value.ToString().Trim());
                        break;
                    case "System.Boolean":
                        itemXml.SetAttributeValue(propName, value.ToString().Trim());
                        break;
                    case "System.DateTime":
                        DateTime? castDate = value as DateTime?;
                        itemXml.SetAttributeValue(propName, castDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                        break;
                    default:
                        itemXml.SetAttributeValue(propName, value.ToString().Trim());
                        break;
                }
            }

            return itemXml;
        }

        /// <summary>
        /// Guarda todos los elementos que se le han pasado en un XML
        /// </summary>
        /// <typeparam name="T">Tipo a guardar</typeparam>
        /// <param name="items">Elementos a guardar</param>
        /// <param name="forceDelete">Forzar borrado de fichero</param>
        /// <param name="customName">Nombre personalizado para el fichero</param>
        public static void SaveAll<T>(IEnumerable<T> items, bool forceDelete = false, string customName = null) where T : class
        {
            lock (lockFile)
            {
                CheckDirectory();

                if (!items.Any()) return;

                var itemType = items.First().GetType();
                var fileName = itemType.Name.ToUpperInvariant() + ".xml";
                if (!string.IsNullOrEmpty(customName)) fileName = customName + ".xml";
                var destination = Path.Combine(DataDirectory, fileName);

                var elements = new List<XElement>();
                var copiedToArrayItems = items.ToArray();
                for (int i = 0; i < copiedToArrayItems.Length; i++)
                {
                    var convertedElement = GetXElement(copiedToArrayItems[i]);
                    if (convertedElement == null) continue;
                    elements.Add(convertedElement);
                }

                if (!File.Exists(destination) || forceDelete)
                {
                    var dataXml = new XElement("DATA", elements);
                    dataXml.Save(destination);
                }
                else
                {
                    XDocument doc = null;

                    using (var fs = new FileStream(destination, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete))
                    {
                        using (var sr = new StreamReader(fs, Encoding.Default))
                        {
                            doc = XDocument.Load(sr, LoadOptions.SetLineInfo);
                        }
                    }

                    doc.Root?.Add(elements);
                    doc.Save(destination);
                }
            }
        }

        /// <summary>
        /// Borra un archivo xml de disco
        /// </summary>
        /// <typeparam name="T">Tipo a guardar</typeparam>
        /// <param name="customName">Nombre personalizado para el fichero</param>
        public static void Delete<T>(string customName = null) where T : class
        {
            lock (lockFile)
            {
                CheckDirectory();

                var itemType = typeof(T);
                var fileName = itemType.Name.ToUpperInvariant() + ".xml";
                if (!string.IsNullOrEmpty(customName)) fileName = customName + ".xml";
                var destination = Path.Combine(DataDirectory, fileName);

                if (!File.Exists(destination)) return;

                File.Delete(destination);
            }
        }

        /// <summary>
        /// Carga un archivo de disco y devuelve todos los elementos que hubiese
        /// </summary>
        /// <typeparam name="T">Tipo a guardar</typeparam>
        /// <param name="customName">Nombre personalizado para el fichero</param>
        /// <returns>Los elementos encontrados de tipo T</returns>
        public static IEnumerable<T> Load<T>(string customName = null) where T : class, new()
        {
            lock (lockFile)
            {
                CheckDirectory();

                var items = new List<T>();

                var itemType = typeof(T);
                var fileName = itemType.Name.ToUpperInvariant() + ".xml";
                if (!string.IsNullOrEmpty(customName)) fileName = customName + ".xml";
                var destination = Path.Combine(DataDirectory, fileName);
                var props = itemType.GetProperties().ToList();

                try
                {
                    if (!File.Exists(destination)) return items;

                    XDocument doc = null;

                    using (var fs = new FileStream(destination, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete))
                    {
                        using (var sr = new StreamReader(fs, Encoding.Default))
                        {
                            doc = XDocument.Load(sr, LoadOptions.SetLineInfo);
                        }
                    }

                    if (doc?.Root == null) return items;


                    var childs = doc.Root.Descendants().ToList();

                    foreach (var child in childs)
                    {
                        var item = new T();

                        foreach (PropertyInfo prop in props)
                        {
                            if (!prop.CanWrite) continue;
                            var propName = prop.Name.ToUpperInvariant();
                            var value = child.Attribute(propName)?.Value;

                            var propType = prop.GetRealName();

                            switch (propType)
                            {
                                case "System.Int32":
                                    int.TryParse(value, out var castInt);
                                    prop.SetValue(item, castInt, null);
                                    break;
                                case "System.UInt32":
                                    uint.TryParse(value, out var castUint);
                                    prop.SetValue(item, castUint, null);
                                    break;
                                case "System.Int64":
                                    long.TryParse(value, out var castLong);
                                    prop.SetValue(item, castLong, null);
                                    break;
                                case "System.UInt64":
                                    ulong.TryParse(value, out var castUlong);
                                    prop.SetValue(item, castUlong, null);
                                    break;
                                case "System.Single":
                                    float.TryParse(value, out var castFloat);
                                    prop.SetValue(item, castFloat, null);
                                    break;
                                case "System.Double":
                                    double.TryParse(value, out var castDouble);
                                    prop.SetValue(item, castDouble, null);
                                    break;
                                case "System.Decimal":
                                    decimal.TryParse(value, out var castDecimal);
                                    prop.SetValue(item, castDecimal, null);
                                    break;
                                case "System.Boolean":
                                    bool.TryParse(value, out var castBool);
                                    prop.SetValue(item, castBool, null);
                                    break;
                                case "System.DateTime":
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        DateTime.TryParseExact(value, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var castDate);
                                        prop.SetValue(item, castDate, null);
                                    }
                                    break;
                                default:
                                    prop.GetRealName();
                                    prop.SetValue(item, value, null);
                                    break;
                            }
                        }

                        items.Add(item);
                    }
                }
                catch (IOException ex)
                {
                    // Rethrow
                    throw ex;
                }
                catch (Exception ex)
                {
                    // Fichero corrupto, borrar
                    //File.Delete(destination);
                    // Rethrow
                    throw ex;
                }

                return items;
            }
        }
    }
}
