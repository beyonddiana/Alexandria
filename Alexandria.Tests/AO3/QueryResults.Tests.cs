﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.AO3;
using Alexandria.Languages;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.Querying;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public class Test_QueryResults
    {
        readonly LibrarySource _source;

        public Test_QueryResults()
        {
            HttpWebClient webClient = new HttpWebClient();
            WebLanguageManager languageManager = new WebLanguageManager( webClient );
            _source = new AO3Source( webClient, languageManager, null );
        }

        [Test]
        public void AO3QueryResults_ThrowsWhenRetrievingAtEnd()
        {
            IQueryResultsPage<IFanfic, IFanficRequestHandle> results = null;
            try
            {
                ITagRequestHandle request = _source.MakeTagRequest( UnitTestConstants.TagPOVJackFrost );
                ITag tag = request.Request();
                results = tag.QueryFanfics();
            }
            catch
            {
                // In case one of the above functions throws InvalidOperationException, we don't want the test to pass.
                Assert.Fail();
            }

            Assert.IsNotNull( results );
            Assert.IsFalse( results.HasMoreResults );
            Assert.Throws<InvalidOperationException>( () => results.RetrieveNextPage() );
        }
    }
}
