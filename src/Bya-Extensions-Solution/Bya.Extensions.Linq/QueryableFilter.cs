using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Query filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryableFilter<T>
    {
        /// <summary>
        /// Filtro a aplicar
        /// </summary>
        public Expression<Func<T, bool>> Filter { get; }

        /// <summary>
        /// La condición a aplicar para que se ejecute éste filtro (si es null no se tiene en cuenta)
        /// </summary>
        public bool? Condition { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filter">Filtro a aplicar</param>
        /// <param name="condition">La condición a aplicar para que se ejecute éste filtro</param>
        public QueryableFilter(Expression<Func<T, bool>> filter, bool? condition = null)
        {
            Filter = filter;
            Condition = condition;
        }
    }
}
