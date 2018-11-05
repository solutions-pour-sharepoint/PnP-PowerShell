﻿#if !ONPREMISES
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet("Apply", "PnPTenantTemplate", SupportsShouldProcess = true)]
    [Alias("Apply-PnPProvisioningHierarchy")]
    [CmdletHelp("Applies a tenant template to the current tenant.",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Apply-PnPTenantTemplate -Path myfile.pnp",
       Remarks = "Will read the tenant template from the filesystem and will apply the sequences in the template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Apply-PnPTenantTemplate -Path myfile.pnp -SequenceId ""mysequence""",
       Remarks = "Will read the tenant template from the filesystem and will apply the specified sequence in the template",
       SortOrder = 1)]
    [CmdletExample(
     Code = @"PS:> Apply-PnPTenantTemplate -Path myfile.pnp -Parameters @{""ListTitle""=""Projects"";""parameter2""=""a second value""}",
     Remarks = @"Applies a tenant template to the current tenant. It will populate the parameter in the template the values as specified and in the template you can refer to those values with the {parameter:<key>} token.

For instance with the example above, specifying {parameter:ListTitle} in your template will translate to 'Projects' when applying the template. These tokens can be used in most string values in a template.",
     SortOrder = 3)]
    public class ApplyTenantTemplate : PnPAdminCmdlet
    {
        private const string ParameterSet_PATH = "By Path";
        private const string ParameterSet_OBJECT = "By Object";

        private ProgressRecord progressRecord = new ProgressRecord(0, "Activity", "Status");
        private ProgressRecord subProgressRecord = new ProgressRecord(1, "Activity", "Status");

        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path to the xml or pnp file containing the provisioning hierarchy.", ParameterSetName = ParameterSet_PATH)]
        public string Path;

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = ParameterSet_OBJECT)]
        [Alias("Hierarchy")]
        public ProvisioningHierarchy Template;

        [Parameter(Mandatory = false)]
        public string SequenceId;

        [Parameter(Mandatory = false, HelpMessage = "Root folder where resources/files that are being referenced in the template are located. If not specified the same folder as where the tenant template is located will be used.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string ResourceFolder;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to only process a specific part of the template. Notice that this might fail, as some of the handlers require other artifacts in place if they are not part of what your applying.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public Handlers Handlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to run all handlers, excluding the ones specified.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public Handlers ExcludeHandlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ExtensbilityHandlers to execute while applying a template", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ExtensibilityHandler[] ExtensibilityHandlers;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while applying a template.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify parameters that can be referred to in the hierarchy by means of the {parameter:<Key>} token. See examples on how to use this parameter.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public Hashtable Parameters;

        [Parameter(Mandatory = false, HelpMessage = "Specify this parameter if you want to overwrite and/or create properties that are known to be system entries (starting with vti_, dlc_, etc.)", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter OverwriteSystemPropertyBagValues;

        [Parameter(Mandatory = false, HelpMessage = "Ignore duplicate data row errors when the data row in the template already exists.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter IgnoreDuplicateDataRowErrors;

        [Parameter(Mandatory = false, HelpMessage = "If set content types will be provisioned if the target web is a subweb.")]
        public SwitchParameter ProvisionContentTypesToSubWebs;

        [Parameter(Mandatory = false, HelpMessage = "If set fields will be provisioned if the target web is a subweb.")]
        public SwitchParameter ProvisionFieldsToSubWebs;

        [Parameter(Mandatory = false, HelpMessage = "Override the RemoveExistingNodes attribute in the Navigation elements of the template. If you specify this value the navigation nodes will always be removed before adding the nodes in the template")]
        public SwitchParameter ClearNavigation;

        protected override void ExecuteCmdlet()
        {
            var applyingInformation = new ProvisioningTemplateApplyingInformation();

            if (MyInvocation.BoundParameters.ContainsKey("Handlers"))
            {
                applyingInformation.HandlersToProcess = Handlers;
            }
            if (MyInvocation.BoundParameters.ContainsKey("ExcludeHandlers"))
            {
                foreach (var handler in (Handlers[])Enum.GetValues(typeof(Handlers)))
                {
                    if (!ExcludeHandlers.Has(handler) && handler != Handlers.All)
                    {
                        Handlers = Handlers | handler;
                    }
                }
                applyingInformation.HandlersToProcess = Handlers;
            }

            if (ExtensibilityHandlers != null)
            {
                applyingInformation.ExtensibilityHandlers = ExtensibilityHandlers.ToList();
            }

            applyingInformation.ProgressDelegate = (message, step, total) =>
            {
                if (message != null)
                {
                    var percentage = Convert.ToInt32((100 / Convert.ToDouble(total)) * Convert.ToDouble(step));
                    progressRecord.Activity = $"Applying template to tenant";
                    progressRecord.StatusDescription = message;
                    progressRecord.PercentComplete = percentage;
                    progressRecord.RecordType = ProgressRecordType.Processing;
                    WriteProgress(progressRecord);
                }
            };

            var warningsShown = new List<string>();

            applyingInformation.MessagesDelegate = (message, type) =>
            {
                switch (type)
                {
                    case ProvisioningMessageType.Warning:
                        {
                            if (!warningsShown.Contains(message))
                            {
                                WriteWarning(message);
                                warningsShown.Add(message);
                            }
                            break;
                        }
                    case ProvisioningMessageType.Progress:
                        {
                            if (message != null)
                            {
                                var activity = message;
                                if (message.IndexOf("|") > -1)
                                {
                                    var messageSplitted = message.Split('|');
                                    if (messageSplitted.Length == 4)
                                    {
                                        var current = double.Parse(messageSplitted[2]);
                                        var total = double.Parse(messageSplitted[3]);
                                        subProgressRecord.RecordType = ProgressRecordType.Processing;
                                        subProgressRecord.Activity = string.IsNullOrEmpty(messageSplitted[0]) ? "-" : messageSplitted[0];
                                        subProgressRecord.StatusDescription = string.IsNullOrEmpty(messageSplitted[1]) ? "-" : messageSplitted[1];
                                        subProgressRecord.PercentComplete = Convert.ToInt32((100 / total) * current);
                                        WriteProgress(subProgressRecord);
                                    }
                                    else
                                    {
                                        subProgressRecord.Activity = "Processing";
                                        subProgressRecord.RecordType = ProgressRecordType.Processing;
                                        subProgressRecord.StatusDescription = activity;
                                        subProgressRecord.PercentComplete = 0;
                                        WriteProgress(subProgressRecord);
                                    }
                                }
                                else
                                {
                                    subProgressRecord.Activity = "Processing";
                                    subProgressRecord.RecordType = ProgressRecordType.Processing;
                                    subProgressRecord.StatusDescription = activity;
                                    subProgressRecord.PercentComplete = 0;
                                    WriteProgress(subProgressRecord);
                                }
                            }
                            break;
                        }
                    case ProvisioningMessageType.Completed:
                        {

                            WriteProgress(new ProgressRecord(1, message, " ") { RecordType = ProgressRecordType.Completed });
                            break;
                        }
                }
            };

            applyingInformation.OverwriteSystemPropertyBagValues = OverwriteSystemPropertyBagValues;
            applyingInformation.IgnoreDuplicateDataRowErrors = IgnoreDuplicateDataRowErrors;
            applyingInformation.ClearNavigation = ClearNavigation;
            applyingInformation.ProvisionContentTypesToSubWebs = ProvisionContentTypesToSubWebs;
            applyingInformation.ProvisionFieldsToSubWebs = ProvisionFieldsToSubWebs;

            ProvisioningHierarchy hierarchyToApply = null;

            switch (ParameterSetName)
            {
                case ParameterSet_PATH:
                    {
                        hierarchyToApply = GetHierarchy();
                        break;
                    }
                case ParameterSet_OBJECT:
                    {
                        hierarchyToApply = Template;
                        if (ResourceFolder != null)
                        {
                            var fileSystemConnector = new FileSystemConnector(ResourceFolder, "");
                            hierarchyToApply.Connector = fileSystemConnector;
                        }
                        else
                        {
                            if (Path != null)
                            {
                                if (!System.IO.Path.IsPathRooted(Path))
                                {
                                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                                }
                            }
                            else
                            {
                                Path = SessionState.Path.CurrentFileSystemLocation.Path;
                            }
                            var fileInfo = new FileInfo(Path);
                            var fileConnector = new FileSystemConnector(fileInfo.DirectoryName, "");
                            hierarchyToApply.Connector = fileConnector;
                        }
                        break;
                    }
            }
            if (Parameters != null)
            {
                foreach (var parameter in Parameters.Keys)
                {
                    if (hierarchyToApply.Parameters.ContainsKey(parameter.ToString()))
                    {
                        hierarchyToApply.Parameters[parameter.ToString()] = Parameters[parameter].ToString();
                    }
                    else
                    {
                        hierarchyToApply.Parameters.Add(parameter.ToString(), Parameters[parameter].ToString());
                    }
                }
            }
            if (!string.IsNullOrEmpty(SequenceId))
            {
                Tenant.ApplyProvisionHierarchy(hierarchyToApply, SequenceId, applyingInformation);
            }
            else
            {
                foreach (var sequence in hierarchyToApply.Sequences)
                {
                    Tenant.ApplyProvisionHierarchy(hierarchyToApply, sequence.ID, applyingInformation);
                }
            }

        }

        private ProvisioningHierarchy GetHierarchy()
        {
            if(!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            return ReadTenantTemplate.LoadProvisioningHierarchyFromFile(Path, TemplateProviderExtensions);
        }
    }
}
#endif