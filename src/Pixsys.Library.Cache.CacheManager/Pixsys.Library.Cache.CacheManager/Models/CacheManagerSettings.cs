// -----------------------------------------------------------------------
// <copyright file="CacheManagerSettings.cs" company="Pixsys">
// Copyright (c) Pixsys. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Pixsys.Library.Cache.CacheManager.Models
{
    /// <summary>
    /// The cache manager settings model.
    /// </summary>
    public class CacheManagerSettings
    {
        /// <summary>
        /// Gets or sets the name of the default profile.
        /// </summary>
        /// <value>
        /// The name of the default profile.
        /// </value>
        public string? DefaultProfileName { get; set; }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        /// <value>
        /// The profiles.
        /// </value>
        public List<CacheManagerProfile>? Profiles { get; set; }
    }
}