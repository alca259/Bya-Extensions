namespace System.Reflection
{
    /// <summary>
    /// Como las PropertyModel pero custom
    /// </summary>
    public class CustomPropertyModel
    {
        /// <summary>
        /// Tipos de datos
        /// </summary>
        public enum PropertyTypeData
        {
            /// <summary>
            /// Otros
            /// </summary>
            Others,
            /// <summary>
            /// Entero de 32 bits
            /// </summary>
            Int32,
            /// <summary>
            /// Texto
            /// </summary>
            String,
            /// <summary>
            /// Número
            /// </summary>
            Double,
            /// <summary>
            /// Flotante
            /// </summary>
            Float,
            /// <summary>
            /// Decimal
            /// </summary>
            Decimal,
            /// <summary>
            /// Fecha
            /// </summary>
            DateTime,
            /// <summary>
            /// Booleano
            /// </summary>
            Boolean,
            /// <summary>
            /// Enumerado
            /// </summary>
            Enum
        }

        /// <summary>
        /// Nombre de la propiedad
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Tipo de la propiedad
        /// </summary>
        public PropertyTypeData PropertyTypeName { get; set; }

        /// <summary>
        /// Valor de la propiedad
        /// </summary>
        public object PropertyValue { get; set; }

        /// <summary>
        /// Si es nulable
        /// </summary>
        public bool PropertyIsNullable { get; set; }

        /// <summary>
        /// Si es Enum
        /// </summary>
        public bool PropertyIsEnum { get; set; }

        /// <summary>
        /// Si es enum Flags
        /// </summary>
        public bool PropertyIsEnumFlag { get; set; }

        /// <summary>
        /// Tipo real
        /// </summary>
        public Type PropertyType { get; set; }
    }
}