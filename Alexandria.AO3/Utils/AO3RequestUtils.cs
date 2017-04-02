﻿using System;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Utils
{
	public static class AO3RequestUtils
	{
		public static IFanficRequestHandle MakeFanficRequest( String handle )
		{
			if ( String.IsNullOrEmpty( handle ) )
			{
				throw new ArgumentNullException( nameof( handle ) );
			}

			foreach ( Char character in handle )
			{
				if ( !Char.IsDigit( character ) )
				{
					throw new ArgumentException( "Handles to fanfics on AO3 may only consist of numbers.", nameof( handle ) );
				}
			}

			return new AO3FanficRequestHandle( handle );
		}

		public static IAuthorRequestHandle MakeAuthorRequest( String username, String pseud = null )
		{
			if ( String.IsNullOrEmpty( username ) )
			{
				throw new ArgumentNullException( nameof( username ) );
			}

			if ( String.IsNullOrWhiteSpace( pseud ) )
			{
				pseud = null;
			}

			return new AO3AuthorRequestHandle( username, pseud );
		}

		public static ITagRequestHandle MakeTagRequest( String tag )
		{
			if ( String.IsNullOrEmpty( tag ) )
			{
				throw new ArgumentNullException( nameof( tag ) );
			}

			return new AO3TagRequestHandle( tag );
		}
	}
}
