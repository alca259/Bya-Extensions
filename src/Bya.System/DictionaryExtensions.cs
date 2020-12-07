namespace System.Collections.Generic
{
    /// <summary>
    /// Extensiones de diccionario
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Copy and return a new dictionary for one level
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="myDict"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Copy<TKey, TValue>(this IDictionary<TKey, TValue> myDict)
        {
            var newDict = new Dictionary<TKey, TValue>();
            foreach (var v in myDict)
            {
                newDict.Add(v.Key, v.Value);
            }
            return newDict;
        }

        /// <summary>
        /// Intenta sumar a un diccionario una cantidad si la clave existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="addedValue"></param>
        public static void TryAddSum<T>(this IDictionary<T, decimal> dict, T key, decimal addedValue = 0)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += addedValue;
                return;
            }

            dict.Add(key, addedValue);
        }

        /// <summary>
        /// Intenta sumar a un diccionario una cantidad si la clave existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="addedValue"></param>
        public static void TryAddSum<T>(this IDictionary<T, float> dict, T key, float addedValue = 0)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += addedValue;
                return;
            }

            dict.Add(key, addedValue);
        }

        /// <summary>
        /// Intenta sumar a un diccionario una cantidad si la clave existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="addedValue"></param>
        public static void TryAddSum<T>(this IDictionary<T, double> dict, T key, double addedValue = 0)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += addedValue;
                return;
            }

            dict.Add(key, addedValue);
        }

        /// <summary>
        /// Intenta sumar a un diccionario una cantidad si la clave existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="addedValue"></param>
        public static void TryAddSum<T>(this IDictionary<T, int> dict, T key, int addedValue = 0)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += addedValue;
                return;
            }

            dict.Add(key, addedValue);
        }

        /// <summary>
        /// Intenta sumar a un diccionario una cantidad si la clave existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="addedValue"></param>
        public static void TryAddSum<T>(this IDictionary<T, long> dict, T key, long addedValue = 0)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += addedValue;
                return;
            }

            dict.Add(key, addedValue);
        }

        /// <summary>
        /// Intenta sumar a un diccionario una cantidad si la clave existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="addedValue"></param>
        public static void TryAddSum<T>(this IDictionary<T, short> dict, T key, short addedValue = 0)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += addedValue;
                return;
            }

            dict.Add(key, addedValue);
        }

        /// <summary>
        /// Returns the value associated with the specified key if there
        /// already is one, or inserts a new value for the specified key and
        /// returns that.
        /// </summary>
        /// <typeparam name="TKey">Type of key</typeparam>
        /// <typeparam name="TValue">Type of value, which must either have
        /// a public parameterless constructor or be a value type</typeparam>
        /// <param name="dictionary">Dictionary to access</param>
        /// <param name="key">Key to lookup</param>
        /// <returns>Existing value in the dictionary, or new one inserted</returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            if (!dictionary.TryGetValue(key, out TValue ret))
            {
                ret = new TValue();
                dictionary[key] = ret;
            }
            return ret;
        }

        /// <summary>
        /// Returns the value associated with the specified key if there already
        /// is one, or calls the specified delegate to create a new value which is
        /// stored and returned.
        /// </summary>
        /// <typeparam name="TKey">Type of key</typeparam>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="dictionary">Dictionary to access</param>
        /// <param name="key">Key to lookup</param>
        /// <param name="valueProvider">Delegate to provide new value if required</param>
        /// <returns>Existing value in the dictionary, or new one inserted</returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueProvider)
        {
            if (!dictionary.TryGetValue(key, out TValue ret))
            {
                ret = valueProvider();
                dictionary[key] = ret;
            }
            return ret;
        }

        /// <summary>
        /// Returns the value associated with the specified key if there
        /// already is one, or inserts the specified value and returns it.
        /// </summary>
        /// <typeparam name="TKey">Type of key</typeparam>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="dictionary">Dictionary to access</param>
        /// <param name="key">Key to lookup</param>
        /// <param name="missingValue">Value to use when key is missing</param>
        /// <returns>Existing value in the dictionary, or new one inserted</returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue missingValue)
        {
            if (!dictionary.TryGetValue(key, out TValue ret))
            {
                ret = missingValue;
                dictionary[key] = ret;
            }
            return ret;
        }
    }
}
