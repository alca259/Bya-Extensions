using Microsoft.Extensions.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System
{
    /// <summary>
    /// Extensiones para enumerados
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Clase de recursos para traducciones
        /// </summary>
        public static IStringLocalizer ResourcesStringLocalizer { get; set; }

        /// <summary>
        /// Obtiene los flags de un valor enumerado
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            if (value == null) return new List<Enum>();
            return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
        }

        /// <summary>
        /// Obtiene los valores de flag por separado
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
        {
            if (value == null) return new List<Enum>();
            return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
        }

        /// <summary>
        /// Traduce el resultado de un enumerado al idioma actual.
        /// (NombreDeLaEnumeracion_ValorEnumerado) es lo usado en el archivo de recursos
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToReadableString(this Enum value)
        {
            if (value == null) return "";
            return GetDescription(value);
        }

        /// <summary>
        /// Traduce el restultado de un enumerado con flags al idioma actual, concatenado con comas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToReadableFlagString(this Enum value)
        {
            if (value == null) return "";
            var flagValues = value.GetIndividualFlags().ToList();
            return string.Join(", ", flagValues.Select(s => s.ToReadableString()));
        }

        /// <summary>
        /// Traduce el resultado de un enumerado al idioma actual.
        /// (NombreDeLaEnumeracion_ValorEnumerado) es lo usado en el archivo de recursos
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToReadableString(Type enumType, int value)
        {
            var description = Enum.GetName(enumType, value);

            // Obtengo el archivo de recursos
            var prefix = enumType.Name;

            // Buscamos la traducción en el fichero de recursos
            var rmDescripcion = ResourcesStringLocalizer.GetString($"{prefix}_{description}");

            // Damos prioridad a la descripción de los recursos
            if (!string.IsNullOrEmpty(rmDescripcion))
            {
                return rmDescripcion;
            }

            return description;
        }

        /// <summary>
        /// Indica si una enumeración es nulable
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullableEnum(this Type t)
        {
            var u = Nullable.GetUnderlyingType(t);
            return u != null && u.IsEnum;
        }

        /// <summary>
        /// Obtiene los valores de un enumerado en un listado
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList ToList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var list = new ArrayList();

            var enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            }

            return list;
        }

        /// <summary>
        /// Convierte un valor string (traducido o literal) a una enumeración
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum? ParseEnum<TEnum>(string value) where TEnum : struct
        {
            if (string.IsNullOrEmpty(value)) return null;

            TEnum result;

            if (int.TryParse(value, out int aux))
            {
                if (Enum.TryParse(value, out result))
                {
                    return result;
                }

                return null;
            }

            // No es un numero, es una letra, intentamos convertir y si no buscamos las enumeraciones que empiezen por el texto suministrado
            if (Enum.TryParse(value, out result))
            {
                return result;
            }

            var acceptedValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();

            // Buscamos con el nombre de la enumeracion
            var canBeFound = acceptedValues.Any(f => f.ToString().StartsWith(value, true, CultureInfo.InvariantCulture));

            if (canBeFound)
            {
                return acceptedValues.FirstOrDefault(f => f.ToString().StartsWith(value, true, CultureInfo.InvariantCulture));
            }

            // Si no, intentamos buscar por el nombre traducido
            foreach (var acceptedValue in acceptedValues)
            {
                var translatedValue = ToReadableString(acceptedValue.GetType(), acceptedValue.GetHashCode());
                if (translatedValue.StartsWith(value, true, CultureInfo.InvariantCulture))
                {
                    return acceptedValue;
                }
            }

            return null;
        }

        #region Private methods
        private static string GetDescription(Enum value)
        {
            if (value == null)
            {
                //throw new ArgumentNullException("value");
                return "";
            }

            string description = value.ToString();

            // Obtengo el archivo de recursos
            var prefix = value.GetType().Name;

            // Buscamos la traducción en el fichero de recursos
            var rmDescripcion = ResourcesStringLocalizer.GetString($"{prefix}_{description}");

            // Damos prioridad a la descripción de los recursos
            if (!string.IsNullOrEmpty(rmDescripcion))
            {
                return rmDescripcion;
            }

            return description;
        }

        private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
        {
            var bits = Convert.ToUInt64(value);
            var results = new List<Enum>();
            for (var i = values.Length - 1; i >= 0; i--)
            {
                var mask = Convert.ToUInt64(values[i]);
                if (i == 0 && mask == 0L)
                {
                    break;
                }
                if ((bits & mask) == mask)
                {
                    results.Add(values[i]);
                    bits -= mask;
                }
            }
            if (bits != 0L)
            {
                return Enumerable.Empty<Enum>();
            }
            if (Convert.ToUInt64(value) != 0L)
            {
                return results.Reverse<Enum>();
            }
            if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
            {
                return values.Take(1);
            }
            return Enumerable.Empty<Enum>();
        }

        private static IEnumerable<Enum> GetFlagValues(Type enumType)
        {
            ulong flag = 0x1;
            foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
            {
                var bits = Convert.ToUInt64(value);
                if (bits == 0L)
                {
                    //yield return value;
                    continue; // skip the zero value
                }

                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits)
                {
                    yield return value;
                }
            }
        }
        #endregion
    }
}
