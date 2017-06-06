﻿using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsLifecycle.Request, "PnPReIndexList")]
    [CmdletAlias("Request-SPOReIndexList")]
    [CmdletHelp("Marks the list for full indexing during the next incremental crawl",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"PS:> Request-PnPReIndexList -Identity ""Demo List""",
        SortOrder = 1)]
    public class RequestReIndexList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);

            if (list != null)
            {
                list.ReIndexList();
            }
         
        }
    }
}
