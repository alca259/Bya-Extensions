using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    /// <summary>
    /// Extensiones de excepciones
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Obtiene todos los mensajes de excepción de sus subexcepciones
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="incluideExceptionName">Incluir el nombre de la excepción</param>
        /// <param name="logStackTrace">Obtener también el stacktrace</param>
        /// <param name="isHtml">En formato HTML</param>
        /// <returns></returns>
        public static string ToReadableString(this Exception ex, bool incluideExceptionName = true, bool logStackTrace = true, bool isHtml = false)
        {
            var message = new StringBuilder();
            var exceptions = ex.FromHierarchy(exc => exc.InnerException).ToList();

            var newLine = isHtml ? "<br/>" : Environment.NewLine;

            foreach (var exception in exceptions)
            {
                if (exception is TargetInvocationException)
                {
                    continue;
                }

                if (incluideExceptionName)
                {
                    message.Append($"Exception: {ex.GetType().Name}{newLine}");
                }

                message.Append($"Error: {exception.Message}{newLine}");

                if (!logStackTrace) continue;
                var stackTrace = isHtml ? $"<pre>{exception.StackTrace}</pre>" : exception.StackTrace;
                message.Append($"Stacktrace: {stackTrace}{newLine}");
            }

            return message.ToString();
        }

        #region Private
        // all error checking left out for brevity
        // a.k.a., linked list style enumerator
        private static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return source.FromHierarchy(nextItem, s => s != null);
        }
        #endregion
    }
}
