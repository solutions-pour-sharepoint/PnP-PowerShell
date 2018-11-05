using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    public static class XmlPageDataHelper
    {
        /// <summary>
        /// Extract properties from Xml data hidden in aspx file
        /// </summary>
        /// <param name="documentContent">Full page content including xml data</param>
        /// <returns>Properties extracted from the page.</returns>
        public static IDictionary<string, string> ExtractProperties(string documentContent)
        {
            // Seek for the Xml data within the page
            var match = Regex.Match(documentContent, "<SharePoint:CTFieldRefs.*<xml>(.*)<\\/xml>.*<\\/SharePoint:CTFieldRefs>", RegexOptions.Singleline);
            if (match.Success)
            {
                // Wrap the actual data to be xml compliant
                var xmlDataStr = $"<data xmlns:mso='mso' xmlns:msdt='msdt'>{match.Groups[1].Value}</data>";

                var xmlData = XElement.Parse(xmlDataStr);

                return xmlData
                    .Elements("{mso}CustomDocumentProperties").Elements()
                    .Where(el => !string.IsNullOrEmpty(el.Value))
                    .ToDictionary(el => el.Name.LocalName, el => el.Value);
            }
            else
            {
                throw new ApplicationException("Invalid documentContent. Is is an .aspx file?");
            }
        }
    }
}