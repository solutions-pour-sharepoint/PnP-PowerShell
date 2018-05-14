﻿#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Properties;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Remove, "PnPClientSidePage")]
    [CmdletHelp("Removes a Client-Side Page",
      Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPClientSidePage -Identity ""MyPage""",
        Remarks = "Removes the Client-Side page named 'MyPage.aspx'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPClientSidePage $page",
        Remarks = "Removes the specified Client-Side page which is contained in the $page variable.",
        SortOrder = 2)]
    public class RemoveClientSidePage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(Resources.RemoveClientSidePage, Resources.Confirm))
            {
                var clientSidePage = Identity.GetPage(ClientContext);
                if (clientSidePage == null)
                    throw new Exception($"Page '{Identity?.Name}' does not exist");

                clientSidePage.Delete();
            }
        }
    }
}
#endif