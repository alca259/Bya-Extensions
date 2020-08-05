using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Extensiones de entity framework
    /// </summary>
    public static class EfCoreExtensions
    {
        /// <summary>
        /// Use this setup for all DateTime properties by default
        /// As per as I am concerned Entity Framework Core by default generate datetime2 column for C# DateTime
        /// https://stackoverflow.com/questions/55347406/ef-core-conventions-for-datetime2
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ModelBuilder UseSqlServerDatetimeAsDefault(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var dateTimeProps = entity.GetProperties().Where(p => p.PropertyInfo != null && p.PropertyInfo.PropertyType == typeof(DateTime));
                foreach (var prop in dateTimeProps)
                {
                    builder.Entity(entity.Name).Property(prop.Name).HasColumnType("datetime");
                }
            }

            return builder;
        }

        /// <summary>
        /// Use this setup for all float number properties by default
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="decimalLength"></param>
        /// <param name="maxDecimals"></param>
        /// <returns></returns>
        public static ModelBuilder UseSqlServerNumberAsDefault(this ModelBuilder builder, int decimalLength = 18, int maxDecimals = 4)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var decimalProps = entity.GetProperties().Where(p => p.PropertyInfo != null && p.PropertyInfo.PropertyType == typeof(decimal)).ToArray();
                foreach (var prop in decimalProps)
                {
                    builder.Entity(entity.Name).Property(prop.Name).HasColumnType($"decimal({decimalLength},{maxDecimals})");
                }

                var doubleProps = entity.GetProperties().Where(p => p.PropertyInfo != null && p.PropertyInfo.PropertyType == typeof(double)).ToArray();
                foreach (var prop in doubleProps)
                {
                    builder.Entity(entity.Name).Property(prop.Name).HasColumnType($"float({decimalLength},{maxDecimals})");
                }

                var floatProps = entity.GetProperties().Where(p => p.PropertyInfo != null && p.PropertyInfo.PropertyType == typeof(float)).ToArray();
                foreach (var prop in floatProps)
                {
                    builder.Entity(entity.Name).Property(prop.Name).HasColumnType($"real({decimalLength},{maxDecimals})");
                }
            }

            return builder;
        }

        /// <summary>
        /// Ejecuta el include si se cumple la condición
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> IncludeIf<TSource, TProperty>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, TProperty>> predicate)
            where TSource : class
        {
            return condition ? source.Include(predicate) : source;
        }

        /// <summary>
        /// Ejecuta el AsNoTracking si se cumple la condición
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<TSource> AsNoTrackingIf<TSource>(this IQueryable<TSource> source, bool condition)
            where TSource : class
        {
            return condition ? source.AsNoTracking() : source;
        }
    }
}
