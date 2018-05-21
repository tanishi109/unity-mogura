﻿using System.Collections.Generic;
using System.Collections.Specialized;

namespace ExtraLinq
{
    public static partial class NameValueCollectionExtensions
    {
        /// <summary>
        /// Returns a new <see cref="Dictionary&lt;TKey,TValue&gt;"/>
        /// from the specified <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>A <see cref="Dictionary&lt;TKey, TValue&gt;"/>.</returns>
        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            ThrowIf.Argument.IsNull(collection, "collection");

            var dictionary = new Dictionary<string, string>(collection.Count);

            foreach (string key in collection.Keys)
            {
                dictionary.Add(key, collection[key]);
            }

            return dictionary;
        }
    }
}
