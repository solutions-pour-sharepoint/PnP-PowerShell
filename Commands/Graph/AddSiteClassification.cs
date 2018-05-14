﻿#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Graph
{
    [Cmdlet(VerbsCommon.Add, "PnPSiteClassification")]
    [CmdletHelp("Adds one ore more site classification values to the list of possible values. Requires a connection to the Microsoft Graph.",
        Category = CmdletHelpCategory.Graph,
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Add-PnPSiteClassification -Classifications ""Top Secret""",
        Remarks = @"Adds the ""Top Secret"" classification to the already existing classification values.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Connect-PnPOnline -Scopes ""Directory.ReadWrite.All""
PS:> Add-PnPSiteClassification -Classifications ""Top Secret"",""HBI""",
        Remarks = @"Adds the ""Top Secret"" and the ""For Your Eyes Only"" classification to the already existing classification values.",
        SortOrder = 2)]
    public class AddSiteClassification : PnPGraphCmdlet
    {

        [Parameter(Mandatory = true)]
        public List<string> Classifications;

        protected override void ExecuteCmdlet()
        {
            try
            {
                var settings = OfficeDevPnP.Core.Framework.Graph.SiteClassificationsUtility.GetSiteClassificationsSettings(AccessToken);
                foreach (var classification in Classifications)
                {
                    if (!settings.Classifications.Contains(classification))
                    {
                        settings.Classifications.Add(classification);
                    }
                }
                OfficeDevPnP.Core.Framework.Graph.SiteClassificationsUtility.UpdateSiteClassificationsSettings(AccessToken, settings);
            }
            catch (ApplicationException ex)
            {
                if (ex.Message == @"Missing DirectorySettingTemplate for ""Group.Unified""")
                {
                    WriteError(new ErrorRecord(new InvalidOperationException("Site Classification is not enabled for this tenant"), "SITECLASSIFICATION_NOT_ENABLED", ErrorCategory.ResourceUnavailable, null));
                } else
                {
                    throw;
                }
            }
        }
    }
}
#endif