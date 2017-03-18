﻿using System;
using System.Collections.Generic;

namespace Alexandria.Model
{
	public interface IShip
	{
		String Name { get; }

		ShipType Type { get; }

		IReadOnlyList<IRequestHandle<ICharacter>> Characters { get; }

		IRequestHandle<IShipInfo> Info { get; }
	}
}
