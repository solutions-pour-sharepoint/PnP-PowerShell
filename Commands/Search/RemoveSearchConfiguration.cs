﻿using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Remove, "PnPSearchConfiguration")]
    [CmdletHelp("Remove the search configuration",
        Category = CmdletHelpCategory.Search)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSearchConfiguration -Configuration $config",
        Remarks = "Remove the search configuration for the current web (does not remove managed property mappings)",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSearchConfiguration -Configuration $config -Scope Site",
        Remarks = "Remove the search configuration for the current site collection (does not remove managed property mappings)",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSearchConfiguration -Configuration $config -Scope Subscription",
        Remarks = "Remove the search configuration for the current tenant (does not remove managed property mappings)",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPSearchConfiguration -Path searchconfig.xml -Scope Subscription",
        Remarks = "Reads the search configuration from the specified XML file and remove it for the current tenant (does not remove managed property mappings)",
        SortOrder = 4)]

    public class RemoveSearchConfiguration : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "Config", HelpMessage = "Search configuration string")]
        public string Configuration;

        [Parameter(Mandatory = true, ParameterSetName = "Path", HelpMessage = "Path to a search configuration")]
        public string Path;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SearchConfigurationScope Scope = SearchConfigurationScope.Web;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "Path")
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }
                Configuration = System.IO.File.ReadAllText(Path);
            }
            switch (Scope)
            {
                case SearchConfigurationScope.Web:
                    {
                        SelectedWeb.DeleteSearchConfiguration(Configuration);
                        break;
                    }
                case SearchConfigurationScope.Site:
                    {
                        ClientContext.Site.DeleteSearchConfiguration(Configuration);
                        break;
                    }
                case SearchConfigurationScope.Subscription:
                    {
                        if (!ClientContext.Url.ToLower().Contains("-admin"))
                        {
                            throw new InvalidOperationException(Resources.CurrentSiteIsNoTenantAdminSite);
                        }

                        ClientContext.DeleteSearchSettings(Configuration, SearchObjectLevel.SPSiteSubscription);
                        break;
                    }
            }
        }
    }
}
