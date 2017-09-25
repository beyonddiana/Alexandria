﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Caching;
using Alexandria.Utils;
using NUnit.Framework;

namespace Alexandria.Tests
{
    [TestFixture]
    [Category( UnitTestConstants.UtilTestsCategory )]
    public class Test_FlagUtils
    {
        [Test]
        public void FlagUtils_DetectsMultipleFlags()
        {
            Assert.IsFalse( CacheableObjects.FanficHtml.HasMultipleFlagsSet() );
            Assert.IsTrue( ( CacheableObjects.FanficHtml | CacheableObjects.TagHtml ).HasMultipleFlagsSet() );
            Assert.IsTrue( CacheableObjects.All.HasMultipleFlagsSet() );
            Assert.IsFalse( CacheableObjects.None.HasMultipleFlagsSet() );
        }
    }
}
