// -----------------------------------------------------------------------
// <copyright file="CacheManagerProfile.cs" company="Pixsys">
// Copyright (c) Pixsys. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Pixsys.Library.Cache.CacheManager.Models
{
    /// <summary>
    /// The cache manager profile model.
    /// </summary>
    public class CacheManagerProfile
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the validity period.
        /// </summary>
        /// <value>
        /// The validity period.
        /// </value>
        public TimeSpan ValidityPeriod { get; set; }
    }
}