﻿using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using File = System.IO.File;

namespace SharePointPnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Add, "PnPWebPartToWebPartPage")]
    [CmdletHelp("Adds a web part to a web part page in a specified zone",
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
   Code = @"PS:> Add-PnPWebPartToWebPartPage -ServerRelativePageUrl ""/sites/demo/sitepages/home.aspx"" -Path ""c:\myfiles\listview.webpart"" -ZoneId ""Header"" -ZoneIndex 1 ",
   Remarks = @"This will add the web part as defined by the XML in the listview.webpart file to the specified page in the specified zone and with the order index of 1", SortOrder = 1)]
    [CmdletExample(
  Code = @"PS:> Add-PnPWebPartToWebPartPage -ServerRelativePageUrl ""/sites/demo/sitepages/home.aspx"" -XML $webpart -ZoneId ""Header"" -ZoneIndex 1 ",
  Remarks = @"This will add the web part as defined by the XML in the $webpart variable to the specified page in the specified zone and with the order index of 1", SortOrder = 1)]
    public class AddWebPartToWebPartPage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Server Relative Url of the page to add the web part to.")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "XML", HelpMessage = "A string containing the XML for the web part.")]
        public string Xml = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "FILE", HelpMessage = "A path to a web part file on a the file system.")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The Zone Id where the web part must be placed")]
        public string ZoneId;

        [Parameter(Mandatory = true, HelpMessage = "The Zone Index where the web part must be placed")]
        public int ZoneIndex;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }


            WebPartEntity wp = null;

            switch (ParameterSetName)
            {
                case "FILE":
                    if (!System.IO.Path.IsPathRooted(Path))
                    {
                        Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                    }

                    if (File.Exists(Path))
                    {
                        var fileStream = new StreamReader(Path);
                        var webPartString = fileStream.ReadToEnd();
                        fileStream.Close();

                        wp = new WebPartEntity {WebPartZone = ZoneId, WebPartIndex = ZoneIndex, WebPartXml = webPartString};
                    }
                    break;
                case "XML":
                    wp = new WebPartEntity {WebPartZone = ZoneId, WebPartIndex = ZoneIndex, WebPartXml = Xml};
                    break;
            }
            if (wp != null)
            {
                SelectedWeb.AddWebPartToWebPartPage(ServerRelativePageUrl, wp);
            }
        }
    }
}
