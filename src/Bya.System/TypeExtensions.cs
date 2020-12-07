using System.Linq;

namespace System
{
    /// <summary>
    /// Extensiones de tipos
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Comprueba si un tipo hereda de otro tipo
        /// </summary>
        /// <param name="t"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static bool InheritsFrom(this Type t, Type parentType)
        {
            if (t == parentType) return true;

            var foundInterfaces = t.GetInterfaces().ToList();

            foreach (var typeInt in foundInterfaces)
            {
                var inherits = typeInt.InheritsFrom(parentType);
                if (inherits)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
