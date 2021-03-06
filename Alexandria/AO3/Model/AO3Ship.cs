﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Alexandria.AO3.RequestHandles;
using Alexandria.Exceptions;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for parsing a tag from AO3.
    /// </summary>
    internal sealed class AO3Ship : AO3TagBase<AO3Ship>, IShip
    {
        static readonly Regex _matchingParentheses = new Regex( @"\([^\)]*\)" );

        AO3Ship( AO3Source source, Uri url, HtmlNode mainDiv )
            : base( source, url, mainDiv )
        {
            string internalShipName = RemoveParentheses( Name );
            if ( TryDetermineShipTypeFromName( internalShipName, out ShipType type, out string[] characterNames ) )
            {
                Type = type;
                Characters = GetCharacters( mainDiv, characterNames );
            }
        }

        /// <summary>
        /// Gets the full name of the ship, as requested. There can be many valid names
        /// for a ship, and this will only be the name of the ship as it was requested.
        /// While it might be *A* valid name for the ship, this is not guaranteed to be the
        /// canonical, "official" name for that ship.
        /// </summary>
        public string Name => Text;

        /// <summary>
        /// Gets the type of the ship: whether this ship is platonic, romantic, or some other
        /// type of relationship.
        /// </summary>
        public ShipType Type { get; }

        /// <summary>
        /// Gets a list of the characters that are involved in this ship.
        /// </summary>
        public IReadOnlyList<ICharacterRequestHandle> Characters { get; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3Ship"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then stored for
        /// querying fanfics and also passed along to any nested request handles for them to parse
        /// data with as well.</param>
        /// <param name="document">The document that came from the website itself.</param>
        /// <returns>An instance of <seealso cref="AO3Ship"/> that was parsed and configured using
        /// the information provided.</returns>
        internal static AO3Ship Parse( AO3Source source, Document document )
        {
            HtmlNode mainDiv = GetMainDiv( document );

            TagType type = ParseTagType( mainDiv, source.Website, document.Url );
            if ( type != TagType.Relationship )
            {
                string name = ParseTagText( mainDiv );
                throw new InvalidTagTypeAlexandriaException( TagType.Relationship, type, name );
            }

            return new AO3Ship( source, document.Url, mainDiv );
        }

        static string RemoveParentheses( string name )
        {
            return _matchingParentheses.Replace( name, string.Empty ).Trim();
        }

        static bool TryDetermineShipTypeFromName( string name, out ShipType type, out string[] characterNames )
        {
            if ( name.Contains( "/" ) || name.Contains( "\\" ) )
            {
                type = ShipType.Romantic;
                characterNames = name.Split( '/', '\\' );
                return true;
            }

            if ( name.Contains( "&" ) )
            {
                type = ShipType.Platonic;
                characterNames = name.Split( '&' );
                return true;
            }

            string[] namePieces = name.Split( ' ' );
            if ( namePieces.Any( piece => piece.Equals( "x", StringComparison.OrdinalIgnoreCase ) ) )
            {
                type = ShipType.Romantic;
                characterNames = namePieces;
                return true;
            }

            type = ShipType.Unknown;
            characterNames = null;
            return false;
        }

        static bool IsCharacterTag( ITagRequestHandle tagRequest, ICollection<string> uniqueNames )
        {
            string tagName = RemoveParentheses( tagRequest.Text );
            return uniqueNames.Contains( tagName );
        }

        IReadOnlyList<ICharacterRequestHandle> GetCharacters( HtmlNode mainDiv, IEnumerable<string> characterNames )
        {
            List<ICharacterRequestHandle> characters = new List<ICharacterRequestHandle>();
            HashSet<string> uniqueNames = new HashSet<string>( characterNames, StringComparer.InvariantCultureIgnoreCase );

            foreach ( ITagRequestHandle tagRequest in ParseParentTags( this, mainDiv ) )
            {
                if ( IsCharacterTag( tagRequest, uniqueNames ) )
                {
                    characters.Add( new AO3CharacterRequestHandle( Source, tagRequest.Text ) );
                }
            }

            return characters;
        }
    }
}
