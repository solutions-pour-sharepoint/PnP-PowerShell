﻿#if !ONPREMISES
using OfficeDevPnP.Core.Sites;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPTeamsTeam")]
    [CmdletHelp("Adds a Teams team to an existing, group connected, site collection",
        DetailedDescription = "This command allows you to add a Teams team to an existing, Office 365 group connected, site collection.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Add-PnPTeamsTeam",
        Remarks = @"This create a teams team for the connected site collection", SortOrder = 1)]
    public class AddTeamsTeam : PnPCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            var results = SiteCollection.TeamifySiteAsync(ClientContext);
            var returnedBool = results.GetAwaiter().GetResult();
            WriteObject(returnedBool);
        }
    }
}
#endif