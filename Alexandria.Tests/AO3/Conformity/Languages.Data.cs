﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Exceptions.Input;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.Tests.AO3.Conformity
{
    public partial class LanguagesTests
    {
        static IReadOnlyList<AO3Language> PullDownLanguages()
        {
            IWebClient webClient = new HttpWebClient();
            HtmlDocument searchPage;
            using ( WebResult result = webClient.Get( "http://archiveofourown.org/works/search" ) )
            {
                searchPage = HtmlUtils.ParseHtmlDocument( result.ResponseText );
            }

            HtmlNode languageSelect = searchPage.DocumentNode.SelectSingleNode( "//select[@id='work_search_language_id']" );
            List<AO3Language> ao3Languages = new List<AO3Language>();
            foreach ( HtmlNode option in languageSelect.Elements( HtmlUtils.OptionsHtmlTag ) )
            {
                string idStr = option.GetAttributeValue( "value", null );
                if ( string.IsNullOrWhiteSpace( idStr ) )
                {
                    continue;
                }

                ao3Languages.Add( new AO3Language( option.InnerText, idStr ) );
            }

            return ao3Languages;
        }

        class AO3Language
        {
            public AO3Language( string ao3Name, string ao3Id )
            {
                if ( string.IsNullOrWhiteSpace( ao3Name ) )
                {
                    throw new ArgumentNullException( nameof( ao3Name ) );
                }

                if ( string.IsNullOrWhiteSpace( ao3Id ) )
                {
                    AO3Id = ao3Id;
                }

                AO3Name = ao3Name;
                AO3Id = ao3Id;

                try
                {
                    AlexandriaValue = LanguageUtils.Parse( ao3Name );
                }
                catch ( NoSuchLanguageAlexandriaException )
                {
                    AlexandriaValue = null;
                }
            }

            public string AO3Name { get; }

            public string AO3Id { get; }

            public Language? AlexandriaValue { get; }
        }
    }
}
