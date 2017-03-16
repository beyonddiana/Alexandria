﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Fanfic : IFanfic
	{
		AO3Fanfic()
		{
		}

		#region IFanfic

		public String Title { get; private set; }

		public MaturityRating Rating { get; private set; }

		public ContentWarnings ContentWarnings { get; private set; }

		public Int32 NumberWords { get; private set; }

		public Boolean IsCompleted { get; private set; }

		public DateTime DateStartedUtc { get; private set; }

		public Int32 NumberLikes { get; private set; }

		public Int32 NumberComments { get; private set; }

		public ISeriesEntry SeriesInfo { get; private set; }

		#endregion // IFanfic

		public static AO3Fanfic Parse( HtmlDocument document )
		{
			AO3Fanfic parsed = new AO3Fanfic();

			HtmlNode workMetaGroup = document.DocumentNode.SelectSingleNode( "//dl[@class='work meta group']" );
			parsed.Rating = ParseUtils.ParseMaturityRatingFromAO3( workMetaGroup.SelectSingleNode( "dd[@class='rating tags']//a" ).InnerText );
			parsed.ContentWarnings = ParseUtils.ParseContentWarningsFromAO3( workMetaGroup.SelectSingleNode( "dd[@class='warning tags']/ul" ) );

			// We wind up looking at every <dd> anyways, so this is more efficient than needing to make a lot of XPath calls over the same datasets
			HtmlNode statsDl = workMetaGroup.SelectSingleNode( "dd[@class='stats']/dl" );
			foreach ( HtmlNode dd in statsDl.Elements( "dd" ) )
			{
				String ddClass = dd.GetAttributeValue( "class", null )?.ToLowerInvariant();
				String ddValue = dd.InnerText;
				switch ( ddClass )
				{
					case "words":
						{
							parsed.NumberWords = Int32.Parse( ddValue );
							break;
						}
					case "kudos":
						{
							parsed.NumberLikes = Int32.Parse( ddValue );
							break;
						}
					case "comments":
						{
							parsed.NumberComments = Int32.Parse( ddValue );
							break;
						}
					case "published":
						{
							parsed.DateStartedUtc = DateTime.Parse( ddValue );
							break;
						}
				}
			}

			HtmlNode seriesSpan = workMetaGroup.SelectSingleNode( "dd[@class='series']/span" );
			if ( seriesSpan != null )
			{
				parsed.SeriesInfo = AO3SeriesEntry.Parse( seriesSpan );
			}

			HtmlNode prefaceGroup = document.DocumentNode.SelectSingleNode( "//div[@class='preface group']" );
			parsed.Title = prefaceGroup.SelectSingleNode( "h2[@class='title heading']" ).InnerText.Trim();

			return parsed;
		}
	}
}