using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// Extensiones de reflexión
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Añade una propiedad
        /// </summary>
        /// <param name="self"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static object AddProperty(this object self, string propertyName, object propertyValue)
        {
            var originProperties = self.GetType().GetProperties();
            var destinationItem = new Dictionary<string, object>();

            foreach (var property in originProperties)
            {
                destinationItem.Add(property.Name, property.GetValue(self));
            }

            destinationItem.Add(propertyName, propertyValue);

            return (object)destinationItem;
        }

        /// <summary>
        /// Elimina una propiedad
        /// </summary>
        /// <param name="self"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object RemoveProperty(this object self, string propertyName)
        {
            var originProperties = self.GetType().GetProperties();
            var destinationItem = new Dictionary<string, object>();

            foreach (var property in originProperties)
            {
                if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)) continue;
                destinationItem.Add(property.Name, property.GetValue(self));
            }

            return (object)destinationItem;
        }

        /// <summary>
        /// Reemplaza el valor de una propiedad
        /// </summary>
        /// <param name="self"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static object ReplaceProperty(this object self, string propertyName, object propertyValue)
        {
            var originProperties = self.GetType().GetProperties();
            var destinationItem = new Dictionary<string, object>();

            foreach (var property in originProperties)
            {
                if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    destinationItem.Add(property.Name, propertyValue);
                    continue;
                }

                destinationItem.Add(property.Name, property.GetValue(self));
            }

            return (object)destinationItem;
        }

        /// <summary>
        /// Copia las propiedades de un objeto especifico a otro por nomenclatura y tipo de dato, y lo devuelve
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="parent"></param>
        /// <param name="excludedList"></param>
        /// <returns></returns>
        public static T CopyPropertiesFrom<T>(this T self, T parent, IEnumerable<string> excludedList = null)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                if (excludedList != null && excludedList.Any(a => string.Equals(fromProperty.Name, a, StringComparison.InvariantCultureIgnoreCase))) continue;

                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }

            return self;
        }

        /// <summary>
        /// Copia las propiedades de un objeto cualquiera a otro por nomenclatura y tipo de dato
        /// </summary>
        /// <param name="self"></param>
        /// <param name="parent"></param>
        /// <param name="excludedList"></param>
        public static void CopyPropertiesFrom(this object self, object parent, IEnumerable<string> excludedList = null)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                if (excludedList != null && excludedList.Any(a => string.Equals(fromProperty.Name, a, StringComparison.InvariantCultureIgnoreCase))) continue;

                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene la información extendida de una propiedad de un tipo de objeto concreto
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="prop"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CustomPropertyModel GetAllInfo<TValue>(this PropertyInfo prop, TValue data)
        {
            var propResult = new CustomPropertyModel
            {
                PropertyIsEnum = false,
                PropertyIsEnumFlag = false,
                PropertyIsNullable = false,
                PropertyName = prop.Name,
                PropertyValue = prop.GetValue(data, null),
                PropertyType = prop.PropertyType,
                PropertyTypeName = CustomPropertyModel.PropertyTypeData.Others
            };

            return Parse(prop, propResult);
        }

        /// <summary>
        /// Obtiene la información extendida de una propiedad de cualquier tipo de objeto 
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static CustomPropertyModel GetAllInfo(this PropertyInfo prop)
        {
            var propResult = new CustomPropertyModel
            {
                PropertyIsEnum = false,
                PropertyIsEnumFlag = false,
                PropertyIsNullable = false,
                PropertyName = prop.Name,
                PropertyValue = null,
                PropertyType = prop.PropertyType,
                PropertyTypeName = CustomPropertyModel.PropertyTypeData.Others
            };

            return Parse(prop, propResult);
        }

        /// <summary>
        /// Obtiene un atributo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static T GetAssemblyAttribute<T>(this Assembly assembly) where T : Attribute
        {
            // Get attributes of this type.
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            // If we didn't get anything, return null.
            if ((attributes == null) || (attributes.Length == 0))
                return null;

            // Convert the first attribute value into
            // the desired type and return it.
            return (T)attributes[0];
        }

        /// <summary>
        /// Obtiene la carpeta donde se está ejecutando el ensamblado
        /// </summary>
        /// <param name="assembly">Ensamblado</param>
        /// <returns></returns>
        public static string GetAssemblyDirectory(this Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return IO.Path.GetDirectoryName(path);
        }

        #region Private
        private static CustomPropertyModel Parse(PropertyInfo prop, CustomPropertyModel propResult)
        {
            if (propResult.PropertyType.GenericTypeArguments.Any())
            {
                propResult.PropertyType = prop.PropertyType.GenericTypeArguments[0];
                propResult.PropertyIsNullable = true;

                if (propResult.PropertyType.BaseType != null && "System.Enum".Equals(propResult.PropertyType.BaseType.FullName))
                {
                    propResult.PropertyIsEnum = true;
                    propResult.PropertyTypeName = CustomPropertyModel.PropertyTypeData.Enum;
                }
            }
            else
            {
                if (propResult.PropertyType.BaseType != null && "System.Enum".Equals(propResult.PropertyType.BaseType.FullName))
                {
                    propResult.PropertyIsEnum = true;
                    propResult.PropertyTypeName = CustomPropertyModel.PropertyTypeData.Enum;
                }
            }

            if (propResult.PropertyIsEnum)
            {
                propResult.PropertyIsEnumFlag = propResult.PropertyType.GetCustomAttribute(typeof(FlagsAttribute)) != null;
            }
            else
            {
                CustomPropertyModel.PropertyTypeData propTypeName;
                Enum.TryParse(propResult.PropertyType.Name, out propTypeName);
                propResult.PropertyTypeName = propTypeName;
            }

            return propResult;
        }
        #endregion
    }
}