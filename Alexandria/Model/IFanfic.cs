﻿using System;
using System.Collections.Generic;

namespace Alexandria.Model
{
	public interface IFanfic : IRequestable
	{
		String Title { get; }

		IRequestHandle<IAuthor> Author { get; }

		MaturityRating Rating { get; }

		ContentWarnings ContentWarnings { get; }

		IReadOnlyList<IShip> Ships { get; }

		IReadOnlyList<ITag> Tags { get; }

		Int32 NumberWords { get; }

		DateTime DateStartedUtc { get; }

		Int32 NumberLikes { get; }

		Int32 NumberComments { get; }

		ISeriesEntry SeriesInfo { get; }
	}
}
