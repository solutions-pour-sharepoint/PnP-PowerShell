﻿#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsCommon.New, "PnPTenantSequence", SupportsShouldProcess = true)]
    [Alias("New-PnPProvisioningSequence")]
    [CmdletHelp("Creates a new tenant sequence object",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> $sequence = New-PnPTenantSequence",
       Remarks = "Creates a new instance of a tenant sequence object.",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> $sequence = New-PnPTenantSequence -Id ""MySequence""",
       Remarks = "Creates a new instance of a tenant sequence object and sets the Id to the value specified.",
       SortOrder = 2)]
    public class NewTenantSequence : PSCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Optional Id of the sequence", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Id;
        protected override void ProcessRecord()
        {
            var result = new ProvisioningSequence();
            if (this.MyInvocation.BoundParameters.ContainsKey("Id"))
            {
                result.ID = Id;
            } else
            {
                result.ID = $"sequence-{Guid.NewGuid()}";
            }
            WriteObject(result);
        }
    }
}
#endif