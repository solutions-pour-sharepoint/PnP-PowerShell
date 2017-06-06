﻿using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.IO;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet("Load", "PnPProvisioningTemplate")]
    [CmdletAlias("Load-SPOProvisioningTemplate")]
    [CmdletHelp("Loads a PnP file from the file systems",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Load-PnPProvisioningTemplate -Path template.pnp",
       Remarks = "Loads a PnP file from the file systems",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Load-PnPProvisioningTemplate -Path template.pnp -TemplateProviderExtensions $extensions",
       Remarks = "Loads a PnP file from the file systems using some custom template provider extenions while loading the file.",
       SortOrder = 2)]
    public class LoadProvisioningTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = false, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            WriteObject(LoadProvisioningTemplateFromFile(Path, TemplateProviderExtensions));
        }

        internal static ProvisioningTemplate LoadProvisioningTemplateFromFile(string templatePath, ITemplateProviderExtension[] templateProviderExtensions)
        {
            // Prepare the File Connector
            string templateFileName = System.IO.Path.GetFileName(templatePath);

            // Prepare the template path
            var fileInfo = new FileInfo(templatePath);
            FileConnectorBase fileConnector = new FileSystemConnector(fileInfo.DirectoryName, "");

            // Load the provisioning template file
            Stream stream = fileConnector.GetFileStream(templateFileName);
            var isOpenOfficeFile = ApplyProvisioningTemplate.IsOpenOfficeFile(stream);

            XMLTemplateProvider provider;
            if (isOpenOfficeFile)
            {
                provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(templateFileName, fileConnector));
                templateFileName = templateFileName.Substring(0, templateFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";
            }
            else
            {
                provider = new XMLFileSystemTemplateProvider(fileConnector.Parameters[FileConnectorBase.CONNECTIONSTRING] + "", "");
            }

            ProvisioningTemplate provisioningTemplate = provider.GetTemplate(templateFileName, templateProviderExtensions);
            provisioningTemplate.Connector = provider.Connector;

            // Return the result
            return provisioningTemplate;
        }
    }
}
