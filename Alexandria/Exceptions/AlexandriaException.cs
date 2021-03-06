﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Exceptions
{
    public abstract class AlexandriaException : Exception
    {
        protected AlexandriaException()
        {
        }

        protected AlexandriaException( string message )
            : base( message )
        {
        }

        protected AlexandriaException( string message, Exception innerException )
            : base( message, innerException )
        {
        }
    }
}
