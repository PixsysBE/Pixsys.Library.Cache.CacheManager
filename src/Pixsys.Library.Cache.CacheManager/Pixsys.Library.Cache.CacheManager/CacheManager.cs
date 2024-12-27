// -----------------------------------------------------------------------
// <copyright file="CacheManager.cs" company="Pixsys">
// Copyright (c) Pixsys. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CacheManager.Core;
using Microsoft.Extensions.Options;
using Pixsys.Library.Cache.CacheManager.Interfaces;
using Pixsys.Library.Cache.CacheManager.Models;
using System.Text;

namespace Pixsys.Library.Cache.CacheManager
{
    /// <summary>
    /// The cache manager.
    /// </summary>
    /// <seealso cref="ICacheManager" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1000:Keywords should be spaced correctly", Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "Reviewed.")]
    public class CacheManager : ICacheManager
    {
        private const string DefaultProfileName = "Default";
        private readonly CacheManagerSettings? settings;
        private readonly object cacheSyncLock = new();
        private readonly Dictionary<string, WeakReference> cacheManagers = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="settings">The cahe manager settings.</param>
        public CacheManager(IOptions<CacheManagerSettings>? settings)
        {
            this.settings = settings?.Value != null ? settings.Value : new CacheManagerSettings { DefaultProfileName = DefaultProfileName, Profiles = [new CacheManagerProfile { Name = DefaultProfileName, ValidityPeriod = TimeSpan.FromHours(1) }] };
        }

        /// <inheritdoc/>
        public bool DoesProfileExist(string? profileName)
        {
            return !string.IsNullOrWhiteSpace(profileName) && settings?.Profiles?.Any(x => x.Name == profileName) == true;
        }

        /// <inheritdoc/>
        public ICacheManager<object> GetCache(string profileName)
        {
            if (profileName != DefaultProfileName && !DoesProfileExist(profileName))
            {
                throw new InvalidOperationException($"A cache profile with the name {profileName} has not been initalized. Please review your settings");
            }

            if (!cacheManagers.TryGetValue(profileName, out WeakReference? weakReference))
            {
                lock (cacheSyncLock)
                {
                    if (!cacheManagers.TryGetValue(profileName, out weakReference))
                    {
                        ICacheManager<object> local1 = CacheFactory.Build(profileName, settings => _ = settings.WithSystemRuntimeCacheHandle());
                        weakReference = new WeakReference(local1);
                        cacheManagers.Add(profileName, weakReference);
                        return local1;
                    }
                }
            }

            ICacheManager<object>? cacheManager = (ICacheManager<object>?)weakReference.Target;
            if (cacheManager != null)
            {
                return cacheManager;
            }

            // if weakReferenceis no more valid, removes it from the dictionary
            lock (cacheSyncLock)
            {
                if (cacheManagers.TryGetValue(profileName, out WeakReference? local4) && weakReference == local4)
                {
                    _ = cacheManagers.Remove(profileName);
                    return GetCache(profileName); // Exit condition to avoid infinite loop
                }
            }

            return GetCache(profileName);
        }

        /// <inheritdoc/>
        public void ClearCache(string? profileName)
        {
            GetCache(GetProfileOrDefault(profileName)).Clear();
        }

        /// <inheritdoc/>
        public T GetFromCache<T>(object key, string? profileName = null)
        {
            return (T)GetCache(GetProfileOrDefault(profileName)).Get(key.ToString());
        }

        /// <inheritdoc/>
        public string AddToCache<T>(object key, object value, string? profileName = null)
        {
            string cacheName = GetProfileOrDefault(profileName);
            ICacheManager<object> cache = GetCache(cacheName);
            cache.Put(new CacheItem<object>(key.ToString(), value, ExpirationMode.Absolute, GetCacheValidPeriod(cache)));
            return cacheName;
        }

        /// <inheritdoc/>
        public bool RemoveFromCache(string cacheKey, string? profileName = null)
        {
            return GetCache(GetProfileOrDefault(profileName)).Remove(cacheKey);
        }

        /// <inheritdoc/>
        public string GetProfileOrDefault(string? profileName)
        {
            string? profileNameFound = DoesProfileExist(profileName) ? profileName : settings?.DefaultProfileName;
            return string.IsNullOrEmpty(profileNameFound) ? DefaultProfileName : profileNameFound;
        }

        /// <inheritdoc/>
        public string GetCacheKey<T>(object key, params object[] args)
        {
            StringBuilder sb = new();
            _ = sb.Append(typeof(T).FullName + "|" + key);
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    _ = sb.Append('|').Append(args[i]);
                }
            }

            return sb.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Gets the cache validity period.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <returns>The validity period.</returns>
        internal TimeSpan GetCacheValidPeriod(ICacheManager<object> cache)
        {
            CacheManagerProfile? cacheprofile = GetCacheManagerProfile(cache?.Name);
            return cacheprofile != null ? cacheprofile.ValidityPeriod : TimeSpan.FromHours(1);
        }

        /// <summary>
        /// Gets the specified cache profile.
        /// </summary>
        /// <param name="profileName">The profile name.</param>
        /// <returns>The cache profile if exists.</returns>
        internal CacheManagerProfile? GetCacheManagerProfile(string? profileName)
        {
            return string.IsNullOrWhiteSpace(profileName) ? null : settings?.Profiles?.FirstOrDefault(x => x.Name == profileName);
        }
    }
}