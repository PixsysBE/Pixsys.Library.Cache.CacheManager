// -----------------------------------------------------------------------
// <copyright file="CacheManagerExtensions.cs" company="Pixsys">
// Copyright (c) Pixsys. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pixsys.Library.Cache.CacheManager.Interfaces;
using Pixsys.Library.Cache.CacheManager.Models;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Pixsys.Library.Cache.CacheManager
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// The cache manager extensions.
    /// </summary>
    public static class CacheManagerExtensions
    {
        /// <summary>
        /// Adds the cache manager.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>the updated builder.</returns>
        public static WebApplicationBuilder AddCacheManager(this WebApplicationBuilder builder)
        {
            if (!builder.Services.Any(x => x.ServiceType == typeof(ICacheManager)))
            {
                _ = builder.Services.Configure<CacheManagerSettings>(builder.Configuration.GetSection("Cache"));
                builder.Services.TryAddSingleton<ICacheManager, CacheManager>();
            }

            return builder;
        }
    }
}