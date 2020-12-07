using System.ComponentModel;
using System.Data;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extensiones de IEnumerable
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Convierte un IEnumerable en un DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data) where T : class
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            int countProperties = 0;

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.Name.Equals(typeof(Enum).Name))
                {
                    table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, typeof(string));
                }
                else if (prop.PropertyType.Name.Contains("Nullable"))
                {
                    if (prop.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, prop.PropertyType.GenericTypeArguments[0]);
                    }
                    else
                    {
                        table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, typeof(string));
                    }
                }
                else
                {
                    table.Columns.Add(!string.IsNullOrEmpty(prop.Description) ? prop.Description : prop.Name, prop.PropertyType);
                }
            }

            countProperties += props.Count;

            object[] values = new object[countProperties];
            foreach (T item in data)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    if (prop.PropertyType.Name.Equals(typeof(Enum).Name))
                    {
                        values[i] = ((Enum)props[i].GetValue(item)).ToString();
                    }
                    else
                    {
                        values[i] = props[i].GetValue(item);
                    }
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// Reordena un array de forma aleatoria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T[] Shuffle<T>(this T[] data)
        {
            Random rnd = new Random();
            var result = data.OrderBy(c => rnd.Next()).ToArray();
            return result;
        }

        /// <summary>
        /// Reordena una lista de forma aleatoria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<T> Shuffle<T>(this IList<T> data)
        {
            Random rnd = new Random();
            var result = data.OrderBy(c => rnd.Next()).ToList();
            return result;
        }

        /// <summary>
        /// Reordena un enumerable de forma aleatoria
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> data)
        {
            Random rnd = new Random();
            var result = data.OrderBy(c => rnd.Next());
            return result;
        }
    }
}
