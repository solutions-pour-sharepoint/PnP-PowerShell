﻿#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsData.Save, "PnPTenantTemplate")]
    [Alias("Save-PnPProvisioningHierarchy")]
    [CmdletHelp("Saves a PnP provisioning hierarchy to the file system",
        Category = CmdletHelpCategory.Provisioning, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
       Code = @"PS:> Save-PnPTenantTemplate -Template $template -Out .\hierarchy.pnp",
       Remarks = "Saves a PnP tenant template to the file system",
       SortOrder = 1)]
    public class SaveTenantTemplate : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Allows you to provide an in-memory instance of a Tenant Template. When using this parameter, the -Out parameter refers to the path for saving the template and storing any supporting file for the template.")]
        [Alias("Hierarchy")]
        public ProvisioningHierarchy Template;

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename to write to, optionally including full path.")]
        public string Out;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            // Determine the output file name and path
            string outFileName = Path.GetFileName(Out);

            if (!Path.IsPathRooted(Out))
            {
                Out = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Out);
            }

            bool proceed = false;

            if (System.IO.File.Exists(Out))
            {
                if (Force || ShouldContinue(string.Format(Properties.Resources.File0ExistsOverwrite, Out),
                    Properties.Resources.Confirm))
                {
                    System.IO.File.Delete(Out);
                    proceed = true;
                }
            }
            else
            {
                proceed = true;
            }

            string outPath = new FileInfo(Out).DirectoryName;

            // Determine if it is an .XML or a .PNP file
            var extension = "";
            if (proceed && outFileName != null)
            {
                if (outFileName.IndexOf(".", StringComparison.Ordinal) > -1)
                {
                    extension = outFileName.Substring(outFileName.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
                }
                else
                {
                    extension = ".pnp";
                }
            }

            var fileSystemConnector = new FileSystemConnector(outPath, "");

            ITemplateFormatter formatter = XMLPnPSchemaFormatter.LatestFormatter;

            if (extension == ".pnp")
            {
                // var connector = new OpenXMLConnector(Out, fileSystemConnector);
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(
                      Out, fileSystemConnector, templateFileName: templateFileName);
                WriteObject("Processing template");
                provider.SaveAs(Template, templateFileName);
                ProcessFiles(Out, fileSystemConnector, provider.Connector);

                //provider.SaveAs(Hierarchy, templateFileName);
                //connector.Commit();
            }
            else
            {
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(outPath, "");
                provider.SaveAs(Template, Out);
            }
        }

        private void ProcessFiles(string templateFileName, FileConnectorBase fileSystemConnector, FileConnectorBase connector)
        {
            var hierarchy = ReadTenantTemplate.LoadProvisioningHierarchyFromFile(templateFileName, null);
            if (Template.Tenant?.AppCatalog != null)
            {
                foreach (var app in Template.Tenant.AppCatalog.Packages)
                {
                    WriteObject($"Processing {app.Src}");
                    AddFile(app.Src, hierarchy, fileSystemConnector, connector);
                }
            }
            if (Template.Tenant?.SiteScripts != null)
            {
                foreach (var siteScript in Template.Tenant.SiteScripts)
                {
                    WriteObject($"Processing {siteScript.JsonFilePath}");
                    AddFile(siteScript.JsonFilePath, hierarchy, fileSystemConnector, connector);
                }
            }
            if (Template.Localizations != null && Template.Localizations.Any())
            {
                foreach (var location in Template.Localizations)
                {
                    WriteObject($"Processing {location.ResourceFile}");
                    AddFile(location.ResourceFile, hierarchy, fileSystemConnector, connector);
                }
            }
            foreach (var template in Template.Templates)
            {
                if(template.WebSettings != null && template.WebSettings.SiteLogo != null)
                {
                    // is it a file?
                    var isFile = false;
                    using (var fileStream = fileSystemConnector.GetFileStream(template.WebSettings.SiteLogo))
                    {
                        isFile = fileStream != null;
                    }
                    if (isFile)
                    {
                        WriteObject($"Processing {template.WebSettings.SiteLogo}");
                        AddFile(template.WebSettings.SiteLogo, hierarchy, fileSystemConnector, connector);
                    }
                }
                if (template.Files.Any())
                {
                    foreach (var file in template.Files)
                    {
                        AddFile(file.Src, hierarchy, fileSystemConnector, connector);
                    }
                }
            }

        }

        private void AddFile(string sourceName, ProvisioningHierarchy hierarchy, FileConnectorBase fileSystemConnector, FileConnectorBase connector)
        {
            using (var fs = fileSystemConnector.GetFileStream(sourceName))
            {
                var fileName = sourceName.IndexOf("\\") > 0 ? sourceName.Substring(sourceName.LastIndexOf("\\") + 1) : sourceName;
                var folderName = sourceName.IndexOf("\\") > 0 ? sourceName.Substring(0, sourceName.LastIndexOf("\\")) : "";
                hierarchy.Connector.SaveFileStream(fileName, folderName, fs);

                if (hierarchy.Connector is ICommitableFileConnector)
                {
                    ((ICommitableFileConnector)hierarchy.Connector).Commit();
                }
            }
        }
    }
}
#endif