using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AnalyzerDatabase.Enums;
using AnalyzerDatabase.Models.Springer;

namespace AnalyzerDatabase.Interfaces
{
    public interface ISearchResultsToDisplay
    {
        #region Variables
        string Title { get; set; }

        string PublicationName { get; set; }

        string PublicationDate { get; set; }

        string Creator { get; set; }

        string Volume { get; set; }

        string IssueIdentifier { get; set; }

        string Doi { get; set; }

        string Pii { get; set; }

        string Eid { get; set; }

        string Arnumber { get; set; }

        string Issn { get; set; }

        string Isbn { get; set; }

        string Identifier { get; set; }

        string OpenAccess { get; set; }

        string PageRange { get; set; }

        string Abstract { get; set; }

        SourceDatabase Source { get; set; }

        decimal PercentComplete { get; set; }

        bool IsDuplicate { get; set; }

        string Year { get; set; }

        List<string> GetCreator();

        #endregion
    }
}