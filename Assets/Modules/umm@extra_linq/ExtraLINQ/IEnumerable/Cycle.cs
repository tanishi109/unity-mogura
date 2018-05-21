﻿using System.Collections.Generic;

namespace ExtraLinq
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Turns a finite sequence into a circular one, or equivalently,
        /// repeats the original sequence indefinitely.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to cycle through.</param>
        /// <returns>An infinite sequence cycling through the given sequence.</returns>
        public static IEnumerable<TSource> Cycle<TSource>(this IEnumerable<TSource> source)
        {
            ThrowIf.Argument.IsNull(source, "source");

            return CycleIterator(source);
        }

        private static IEnumerable<TSource> CycleIterator<TSource>(IEnumerable<TSource> source)
        {
            var collection = source as ICollection<TSource>;

            var elementBuffer = collection == null
                ? new List<TSource>()
                : new List<TSource>(collection.Count);

            foreach (TSource element in source)
            {
                yield return element;

                // We add this element to a local element buffer so that
                // we don't have to enumerate the sequence multiple times
                elementBuffer.Add(element);
            }

            if (elementBuffer.IsEmpty())
            {
                // If the element buffer is empty, so was the source sequence.
                // In this case, we can stop here and simply return an empty sequence.
                yield break;
            }

            int index = 0;
            while (true)
            {
                yield return elementBuffer[index];
                index = (index + 1) % elementBuffer.Count;
            }
        }
    }
}
