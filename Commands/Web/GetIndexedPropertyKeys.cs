﻿using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPIndexedPropertyKeys")]
    [CmdletHelp("Returns the keys of the property bag values that have been marked for indexing by search",
        Category = CmdletHelpCategory.Webs)]
    public class GetIndexedProperties : PnPWebCmdlet
    {
        protected override void ExecuteCmdlet()
        {
            var keys = SelectedWeb.GetIndexedPropertyBagKeys();
            WriteObject(keys);
        }
    }
}
