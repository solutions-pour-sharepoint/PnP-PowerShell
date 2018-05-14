﻿using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq.Expressions;
using System;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPList")]
    [CmdletHelp("Returns a List object",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(List),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.list.aspx")]
    [CmdletExample(
        Code = "PS:> Get-PnPList",
        Remarks = "Returns all lists in the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = "Returns a list with the given id.",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity Lists/Announcements",
        Remarks = "Returns a list with the given url.",
        SortOrder = 3)]
    public class GetList : PnPWebRetrievalsCmdlet<List>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, name or Url (Lists/MyList) of the list.")]
        public ListPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Switch parameter if an exception should be thrown if the requested list does not exist (true) or if omitted, nothing will be returned in case the list does not exist")]
        public SwitchParameter ThrowExceptionIfListNotFound;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<List, object>>[] { l => l.Id, l => l.BaseTemplate, l => l.OnQuickLaunch, l => l.DefaultViewUrl, l => l.Title, l => l.Hidden, l => l.RootFolder.ServerRelativeUrl };

            if (Identity != null)
            {
                var list = Identity.GetList(SelectedWeb);

                if (ThrowExceptionIfListNotFound && list == null)
                { 
                    throw new PSArgumentException($"No list found with id, title or url '{Identity}'", "Identity");
                }
                list?.EnsureProperties(RetrievalExpressions);

                WriteObject(list);
            }
            else
            {
                var query = SelectedWeb.Lists.IncludeWithDefaultProperties(RetrievalExpressions);
                var lists = ClientContext.LoadQuery(query);
                ClientContext.ExecuteQueryRetry();
                WriteObject(lists, true);
            }
        }
    }
}