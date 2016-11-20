namespace AnalyzerDatabase.Models.IeeeXplore
{
    public class testowa
    {

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class root
        {

            private byte totalfoundField;

            private uint totalsearchedField;

            private rootDocument documentField;

            /// <remarks/>
            public byte totalfound
            {
                get
                {
                    return this.totalfoundField;
                }
                set
                {
                    this.totalfoundField = value;
                }
            }

            /// <remarks/>
            public uint totalsearched
            {
                get
                {
                    return this.totalsearchedField;
                }
                set
                {
                    this.totalsearchedField = value;
                }
            }

            /// <remarks/>
            public rootDocument document
            {
                get
                {
                    return this.documentField;
                }
                set
                {
                    this.documentField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class rootDocument
        {

            private byte rankField;

            private string titleField;

            private string authorsField;

            private string affiliationsField;

            private string[] thesaurustermsField;

            private string pubtitleField;

            private string punumberField;

            private string pubtypeField;

            private string publisherField;

            private string pyField;

            private string spageField;

            private string epageField;

            private string abstractField;

            private string isbnField;

            private string arnumberField;

            private string doiField;

            private string publicationIdField;

            private string partnumField;

            private string mdurlField;

            private string pdfField;

            /// <remarks/>
            public byte rank
            {
                get
                {
                    return this.rankField;
                }
                set
                {
                    this.rankField = value;
                }
            }

            /// <remarks/>
            public string title
            {
                get
                {
                    return this.titleField;
                }
                set
                {
                    this.titleField = value;
                }
            }

            /// <remarks/>
            public string authors
            {
                get
                {
                    return this.authorsField;
                }
                set
                {
                    this.authorsField = value;
                }
            }

            /// <remarks/>
            public string affiliations
            {
                get
                {
                    return this.affiliationsField;
                }
                set
                {
                    this.affiliationsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("term", IsNullable = false)]
            public string[] thesaurusterms
            {
                get
                {
                    return this.thesaurustermsField;
                }
                set
                {
                    this.thesaurustermsField = value;
                }
            }

            /// <remarks/>
            public string pubtitle
            {
                get
                {
                    return this.pubtitleField;
                }
                set
                {
                    this.pubtitleField = value;
                }
            }

            /// <remarks/>
            public string punumber
            {
                get
                {
                    return this.punumberField;
                }
                set
                {
                    this.punumberField = value;
                }
            }

            /// <remarks/>
            public string pubtype
            {
                get
                {
                    return this.pubtypeField;
                }
                set
                {
                    this.pubtypeField = value;
                }
            }

            /// <remarks/>
            public string publisher
            {
                get
                {
                    return this.publisherField;
                }
                set
                {
                    this.publisherField = value;
                }
            }

            /// <remarks/>
            public string py
            {
                get
                {
                    return this.pyField;
                }
                set
                {
                    this.pyField = value;
                }
            }

            /// <remarks/>
            public string spage
            {
                get
                {
                    return this.spageField;
                }
                set
                {
                    this.spageField = value;
                }
            }

            /// <remarks/>
            public string epage
            {
                get
                {
                    return this.epageField;
                }
                set
                {
                    this.epageField = value;
                }
            }

            /// <remarks/>
            public string @abstract
            {
                get
                {
                    return this.abstractField;
                }
                set
                {
                    this.abstractField = value;
                }
            }

            /// <remarks/>
            public string isbn
            {
                get
                {
                    return this.isbnField;
                }
                set
                {
                    this.isbnField = value;
                }
            }

            /// <remarks/>
            public string arnumber
            {
                get
                {
                    return this.arnumberField;
                }
                set
                {
                    this.arnumberField = value;
                }
            }

            /// <remarks/>
            public string doi
            {
                get
                {
                    return this.doiField;
                }
                set
                {
                    this.doiField = value;
                }
            }

            /// <remarks/>
            public string publicationId
            {
                get
                {
                    return this.publicationIdField;
                }
                set
                {
                    this.publicationIdField = value;
                }
            }

            /// <remarks/>
            public string partnum
            {
                get
                {
                    return this.partnumField;
                }
                set
                {
                    this.partnumField = value;
                }
            }

            /// <remarks/>
            public string mdurl
            {
                get
                {
                    return this.mdurlField;
                }
                set
                {
                    this.mdurlField = value;
                }
            }

            /// <remarks/>
            public string pdf
            {
                get
                {
                    return this.pdfField;
                }
                set
                {
                    this.pdfField = value;
                }
            }
        }


    }
}