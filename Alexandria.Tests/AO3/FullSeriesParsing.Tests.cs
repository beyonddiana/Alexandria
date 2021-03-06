﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using Alexandria.Languages;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public class Test_FullSeriesParsing
    {
        readonly LibrarySource _source;

        public Test_FullSeriesParsing()
        {
            HttpWebClient webClient = new HttpWebClient();
            WebLanguageManager languageManager = new WebLanguageManager( webClient );
            _source = new AO3Source( webClient, languageManager, null );
        }

        [Test]
        public void AO3Tag_JanuaryJackrabbitWeek2014()
        {
            ISeriesRequestHandle request = _source.MakeSeriesRequest( UnitTestConstants.SeriesHandleJanuaryJackrabbitWeek2014 );
            ISeries series = request.Request();

            Assert.IsNotNull( series );

            Assert.IsNotNull( series.Authors );
            Assert.That( series.Authors.Count, Is.EqualTo( 1 ) );
            Assert.AreEqual( "Melissae", series.Authors[0].Username );
            AO3Assert.IsDate( 2014, 1, 25, series.DateStarted );
            AO3Assert.IsDate( 2016, 3, 28, series.DateLastUpdated );
            Assert.IsFalse( series.IsCompleted );

            Assert.IsNotNull( series.Fanfics );
            Assert.AreEqual( 6, series.Fanfics.Count );
            AO3Assert.IsFanficRequest( "1153087", series.Fanfics[0] );
            AO3Assert.IsFanficRequest( "1153126", series.Fanfics[1] );
            AO3Assert.IsFanficRequest( "1155713", series.Fanfics[2] );
            AO3Assert.IsFanficRequest( "1158578", series.Fanfics[3] );
            AO3Assert.IsFanficRequest( "1160404", series.Fanfics[4] );
            AO3Assert.IsFanficRequest( "1169503", series.Fanfics[5] );
        }
    }
}
