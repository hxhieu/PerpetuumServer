using Perpetuum.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Perpetuum.Data
{
    public static class Database
    {
        #region Caching

         /// <summary>
        /// Sql table cache factory using EF
        /// </summary>
        public static IDictionary<TKey, TValue> CreateCache<TKey, TValue, TEntity>(
            Func<TEntity, TKey> keySelector,
            Func<TEntity, TValue> valueFactory,
            Func<TEntity, bool> selector = null,
            Func<TKey, TKey> keyFactory = null
        ) where TEntity : class
        {
            return new LazyDictionary<TKey, TValue>(() => LoadEntityToDictionary(keySelector, valueFactory, selector, keyFactory));
        }

        public static ILookup<TKey, TValue> CreateLookupCache<TKey, TValue, TEntity>(
            Func<TEntity, TKey> keySelector,
            Func<TEntity, TValue> valueFactory,
            Func<TEntity, bool> selector = null
        ) where TEntity : class
        {
            return new LazyLookup<TKey, TValue>(() =>
            {
                var repo = GlobalServiceManager.CreateReadOnlyRepository<TEntity>();
                var entities = repo.GetMany();
                return entities.Where(e => selector == null || selector(e)).ToLookup(e => keySelector(e), valueFactory);
            });
        }

        private static Dictionary<TKey, TV> LoadEntityToDictionary<TKey, TV, TEntity>(
            Func<TEntity, TKey> keySelector,
            Func<TEntity, TV> valueFactory,
            Func<TEntity, bool> selector = null,
            Func<TKey, TKey> keyFactory = null
        ) where TEntity : class
        {
            var repo = GlobalServiceManager.CreateReadOnlyRepository<TEntity>();
            var entities = repo.GetMany();

            var result = new Dictionary<TKey, TV>();

            foreach (var e in entities)
            {
                if (!(selector?.Invoke(e) ?? true))
                    continue;

                var key = keySelector(e);

                if (keyFactory != null)
                {
                    key = keyFactory(key);
                }

                var value = valueFactory(e);
                result.Add(key, value);
            }

            return result;
        }

        #endregion
    }
}