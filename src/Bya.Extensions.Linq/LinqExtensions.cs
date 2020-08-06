using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Extensiones de LINQ
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Tamaño de página por defecto
        /// </summary>
        public const int DEFAULT_PAGE_SIZE = 50;

        #region IQueryable
        /// <summary>
        /// Aplica a la consulta todos los filtros suministrados
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereFilters<TSource>(this IQueryable<TSource> source, params QueryableFilter<TSource>[] filters)
        {
            if (filters == null) return source;
            var query = source;

            foreach (var filter in filters)
            {
                if (filter.Condition == null)
                {
                    query = query.Where(filter.Filter);
                    continue;
                }

                if (filter.Condition.Value)
                {
                    query = query.WhereIf(filter.Condition.Value, filter.Filter);
                }
            }

            return query;
        }

        /// <summary>
        /// Ejecuta el where si se cumple la condicion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// Ejecuta el take si se cumple la condicion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public static IQueryable<TSource> TakeIf<TSource>(this IQueryable<TSource> source, bool condition, int maxResults)
        {
            return condition ? source.Take(maxResults) : source;
        }

        /// <summary>
        /// Controla que no haya páginas negativas, y salta el pagesize multiplicado por la página actual
        /// </summary>
        /// <typeparam name="T">El tipo de dato</typeparam>
        /// <param name="query">La consulta</param>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>La misma query pero con los datos acotados</returns>
        public static IQueryable<T> GoToPage<T>(this IQueryable<T> query, int page = 1, int pageSize = DEFAULT_PAGE_SIZE)
        {
            page--;
            if (page < 0) page = 0;
            return query.Skip(pageSize * page).Take(pageSize);
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Ejecuta el where si se cumple la condicion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// Ejecuta el take si se cumple la condicion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeIf<TSource>(this IEnumerable<TSource> source, bool condition, int maxResults)
        {
            return condition ? source.Take(maxResults) : source;
        }

        /// <summary>
        /// Controla que no haya páginas negativas, y salta el pagesize multiplicado por la página actual
        /// </summary>
        /// <typeparam name="T">El tipo de dato</typeparam>
        /// <param name="query">La consulta</param>
        /// <param name="page">Página actual</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>La misma query pero con los datos acotados</returns>
        public static IEnumerable<T> GoToPage<T>(this IEnumerable<T> query, int page = 1, int pageSize = DEFAULT_PAGE_SIZE)
        {
            page--;
            if (page < 0) page = 0;
            return query.Skip(pageSize * page).Take(pageSize);
        }
        #endregion
    }
}
