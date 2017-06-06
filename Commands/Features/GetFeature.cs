﻿using Microsoft.SharePoint.Client;
using System.Collections.Generic;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using System.Linq.Expressions;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Features
{
    [Cmdlet(VerbsCommon.Get, "PnPFeature")]
    [CmdletAlias("Get-SPOFeature")]
    [CmdletHelp("Returns all activated or a specific activated feature",
        Category = CmdletHelpCategory.Features,
        OutputType = typeof(IEnumerable<Feature>),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.feature.aspx")]
    [CmdletExample(
     Code = @"PS:> Get-PnPFeature",
     Remarks = @"This will return all activated web scoped features", SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Get-PnPFeature -Scope Site",
     Remarks = @"This will return all activated site scoped features", SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Get-PnPFeature -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22",
     Remarks = @"This will return a specific activated web scoped feature", SortOrder = 3)]
    [CmdletExample(
     Code = @"PS:> Get-PnPFeature -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22 -Scope Site",
     Remarks = @"This will return a specific activated site scoped feature", SortOrder = 4)]
    public class GetFeature : PnPWebRetrievalsCmdlet<Feature>
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "The feature ID or name to query for, Querying by name is not supported in version 15 of the Client Side Object Model")]
        public FeaturePipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "The scope of the feature. Defaults to Web.")]
        public FeatureScope Scope = FeatureScope.Web;

        protected override void ExecuteCmdlet()
        {
#if !SP2013
            DefaultRetrievalExpressions = new Expression<Func<Feature, object>>[] { f => f.DisplayName };
#else
            DefaultRetrievalExpressions = new Expression<Func<Feature, object>>[] { f => f.DefinitionId };
#endif
            FeatureCollection featureCollection;
            if (Scope == FeatureScope.Site)
            {
                featureCollection = ClientContext.Site.Features;
            }
            else
            {
                featureCollection = SelectedWeb.Features;
            }
            IEnumerable<Feature> query = ClientContext.LoadQuery(featureCollection.IncludeWithDefaultProperties(RetrievalExpressions));
            ClientContext.ExecuteQueryRetry();
            if (Identity == null)
            {
                WriteObject(query, true);
            }
            else
            {
                if (Identity.Id != Guid.Empty)
                {
                    WriteObject(query.Where(f => f.DefinitionId == Identity.Id));
                }
                else if (!string.IsNullOrEmpty(Identity.Name))
                {
#if !SP2013
                    WriteObject(query.Where(f => f.DisplayName.Equals(Identity.Name, StringComparison.OrdinalIgnoreCase)));
#else
                    throw new Exception("Querying by name is not supported in version 15 of the Client Side Object Model");
#endif
                }
            }
        }

    }
}
