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

		#endregion // IFanfic

		/*public static AO3Fanfic ParseType1( HtmlNode tree )
		{
			HtmlNode titleAuthorFandom = tree.SelectSingleNode( "div[@class='header module']" );
			HtmlNode requiredTags = titleAuthorFandom.SelectSingleNode( "ul" );

			AO3Fanfic parsed = new AO3Fanfic
			{
				Title = titleAuthorFandom.SelectSingleNode( "h4/a" ).InnerText,
				Rating = ParseUtils.ParseMaturityRatingFromAO3( requiredTags.SelectSingleNode( "//span[contains( @class, 'rating' )]/span" ).InnerText ),
				
			};
			return parsed;
		}*/

		public static AO3Fanfic Parse( HtmlNode tree )
		{
			HtmlNode workMetaGroup = tree.SelectSingleNode( "div[@class='wrapper']/dl" );
			HtmlNode workskin = tree.SelectSingleNode( "div[@id='workskin']" );
			HtmlNode prefaceGroup = workskin.SelectSingleNode( "div[@class='preface group']" );

			AO3Fanfic parsed = new AO3Fanfic
			{
				Title = prefaceGroup.SelectSingleNode( "h2[@class='title heading']" ).InnerText.Trim(),
				Rating = ParseUtils.ParseMaturityRatingFromAO3( workMetaGroup.SelectSingleNode( "dd[@class='rating tags']//a").InnerText ),
				ContentWarnings = ParseUtils.ParseContentWarningsFromAO3( workMetaGroup.SelectSingleNode( "dd[@class='warning tags']/ul" ) )
			};
			return parsed;
		}
	}
}
