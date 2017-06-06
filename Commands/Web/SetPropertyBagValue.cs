﻿using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "PnPPropertyBagValue")]
    [CmdletAlias(("Set-SPOPropertyBagValue"))]
    [CmdletHelp("Sets a property bag value",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
      Code = @"PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue",
      Remarks = "This sets or adds a value to the current web property bag",
      SortOrder = 1)]
    [CmdletExample(
      Code = @"PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue -Folder /",
      Remarks = "This sets or adds a value to the root folder of the current web",
      SortOrder = 2)]
    [CmdletExample(
      Code = @"PS:> Set-PnPPropertyBagValue -Key MyKey -Value MyValue -Folder /MyFolder",
      Remarks = "This sets or adds a value to the folder MyFolder which is located in the root folder of the current web",
      SortOrder = 3)]
    public class SetPropertyBagValue : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "Web")]
        [Parameter(Mandatory = true, ParameterSetName = "Folder")]
        public string Key;

        [Parameter(Mandatory = true, ParameterSetName = "Web")]
        [Parameter(Mandatory = true, ParameterSetName = "Folder")]
        [Parameter(Mandatory = true)]
        public string Value;

        [Parameter(Mandatory = true, ParameterSetName = "Web")]
        public SwitchParameter Indexed;

        [Parameter(Mandatory = false, ParameterSetName = "Folder", HelpMessage = "Site relative url of the folder. See examples for use.")]
        public string Folder;

        protected override void ExecuteCmdlet()
        {
            if (SelectedWeb.IsNoScriptSite())
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Site has NoScript enabled, and setting property bag values is not supported"), "NoScriptEnabled", ErrorCategory.InvalidOperation, this));
                return;
            }
            if (!MyInvocation.BoundParameters.ContainsKey("Folder"))
            {
                if (!Indexed)
                {
                    // If it is already an indexed property we still have to add it back to the indexed properties
                    Indexed = !string.IsNullOrEmpty(SelectedWeb.GetIndexedPropertyBagKeys().FirstOrDefault(k => k == Key));
                }

                SelectedWeb.SetPropertyBagValue(Key, Value);
                if (Indexed)
                {
                    SelectedWeb.AddIndexedPropertyBagKey(Key);
                }
                else
                {
                    SelectedWeb.RemoveIndexedPropertyBagKey(Key);
                }
            }
            else
            {
                SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

                var folderUrl = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder);
                var folder = SelectedWeb.GetFolderByServerRelativeUrl(folderUrl);

                folder.EnsureProperty(f => f.Properties);

                folder.Properties[Key] = Value;
                folder.Update();
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}
