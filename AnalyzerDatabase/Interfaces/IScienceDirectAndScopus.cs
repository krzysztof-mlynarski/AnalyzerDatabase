﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Models.Springer;

namespace AnalyzerDatabase.Interfaces
{
    public interface IScienceDirectAndScopus
    {
        string Title { get; set; }

        string PublicationName { get; set; }

        string PublicationDate { get; set; }

        string Creator { get; set; }

        //IList<Creator> Creators { get; set; }

        string Volume { get; set; }

        string IssueIdentifier { get; set; }

        string Doi { get; set; }

        string Pii { get; set; }

        string Issn { get; set; }

        string Isbn { get; set; }

        string Identifier { get; set; }

        string OpenAccess { get; set; }

        string PageRange { get; set; }

        string Abstract { get; set; }

        SourceDatabase Source { get; set; }

    }
}