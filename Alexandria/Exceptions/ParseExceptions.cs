﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Exceptions
{
	public abstract class GeneralParseException
	{
	}

	public abstract class DoesNotExistException : GeneralParseException
	{
	}
}