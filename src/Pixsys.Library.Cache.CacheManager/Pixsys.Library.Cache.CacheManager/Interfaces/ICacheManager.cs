// -----------------------------------------------------------------------
// <copyright file="ICacheManager.cs" company="Pixsys">
// Copyright (c) Pixsys. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CacheManager.Core;

namespace Pixsys.Library.Cache.CacheManager.Interfaces
{
    /// <summary>
    /// The cache manager interface.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Adds an object to the cache.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="profileName">The profile name. if not found, object is added to the default cache.</param>
        /// <returns>The name of the profile where object has been added.</returns>
        string AddToCache<T>(object key, object value, string? profileName = null);

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <param name="profileName">The profile name. if null, the default cache is cleared.</param>
        void ClearCache(string? profileName = null);

        /// <summary>
        /// Check if profile exists.
        /// </summary>
        /// <param name="profileName">The profile name.</param>
        /// <returns>
        /// True if it exists, otherwise false.
        /// </returns>
        bool DoesProfileExist(string profileName);

        /// <summary>
        /// Gets the cache for a specific profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns>The cache.</returns>
        ICacheManager<object> GetCache(string profileName);

        /// <summary>
        /// Generates a cache key based on parameters.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The cache key.</returns>
        string GetCacheKey<T>(object key, params object[] args);

        /// <summary>
        /// Gets an object from the cache.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns>The object.</returns>
        T GetFromCache<T>(object key, string? profileName = null);

        /// <summary>
        /// Gets a specified profile, or the default one if not found.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns>The profile name.</returns>
        string GetProfileOrDefault(string profileName);

        /// <summary>
        /// Removes an object from the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns>true if the key was found and removed from the cache, otherwise false.</returns>
        bool RemoveFromCache(string cacheKey, string? profileName = null);
    }
}
