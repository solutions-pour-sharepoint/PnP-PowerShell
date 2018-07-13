using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.Commands.Provisioning;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using PnPFileLevel = OfficeDevPnP.Core.Framework.Provisioning.Model.FileLevel;
using SPFile = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands
{
    /// <summary>
    /// Base class for commands related to adding file to template
    /// </summary>
    public class BaseFileProvisioningCmdlet : PnPWebCmdlet
    {
        protected const string PSNAME_LOCAL_SOURCE = "LocalSourceFile";
        protected const string PSNAME_REMOTE_SOURCE = "RemoteSourceFile";

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML provisioning template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = false, Position = 3, HelpMessage = "The target Container for the file to add to the in-memory template, optional argument.")]
        public string Container;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "The level of the files to add. Defaults to Published")]
        public PnPFileLevel FileLevel = PnPFileLevel.Published;

        [Parameter(Mandatory = false, Position = 5, HelpMessage = "Set to overwrite in site, Defaults to true")]
        public SwitchParameter FileOverwrite = true;

        [Parameter(Mandatory = false, Position = 6, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected ProvisioningTemplate LoadTemplate()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            // Load the template
            var template = ReadProvisioningTemplate
                .LoadProvisioningTemplateFromFile(Path,
                TemplateProviderExtensions);

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }

            return template;
        }

        /// <summary>
        /// Add a file to the template
        /// </summary>
        /// <param name="template">The provisioning template to add the file to</param>
        /// <param name="fs">Stream to read the file content</param>
        /// <param name="folder">target folder in the provisioning template</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="container">Container path within the template (pnp file) or related to the xml templage</param>
        protected void AddFileToTemplate(ProvisioningTemplate template, Stream fs, string folder, string fileName, string container)
        {
            var source = !string.IsNullOrEmpty(container) ? (container + "/" + fileName) : fileName;

            template.Connector.SaveFileStream(fileName, container, fs);

            if (template.Connector is ICommitableFileConnector)
            {
                ((ICommitableFileConnector)template.Connector).Commit();
            }

            var existing = template.Files.FirstOrDefault(f => f.Src == $"{container}/{fileName}" && f.Folder == folder);

            if (existing != null)
                template.Files.Remove(existing);

            var newFile = new OfficeDevPnP.Core.Framework.Provisioning.Model.File
            {
                Src = source,
                Folder = folder,
                Level = FileLevel,
                Overwrite = FileOverwrite,
            };

            template.Files.Add(newFile);

            // Determine the output file name and path
            var outFileName = System.IO.Path.GetFileName(Path);
            var outPath = new FileInfo(Path).DirectoryName;

            var fileSystemConnector = new FileSystemConnector(outPath, "");
            var formatter = XMLPnPSchemaFormatter.LatestFormatter;
            var extension = new FileInfo(Path).Extension.ToLowerInvariant();
            if (extension == ".pnp")
            {
                var provider = new XMLOpenXMLTemplateProvider(template.Connector as OpenXMLConnector);
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
            }
            else
            {
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(Path, "");
                provider.SaveAs(template, Path, formatter, TemplateProviderExtensions);
            }
        }

        /// <summary>
        /// Adds a remote file to a template
        /// </summary>
        /// <param name="template">Template to add the file to</param>
        /// <param name="file">The SharePoint file to retrieve and add</param>
        protected void AddSPFileToTemplate(ProvisioningTemplate template, SPFile file)
        {
            file.EnsureProperties(f => f.Name, f => f.ServerRelativeUrl);
            var folderRelativeUrl = file.ServerRelativeUrl.Substring(0, file.ServerRelativeUrl.Length - file.Name.Length - 1);
            var folderWebRelativeUrl = HttpUtility.UrlKeyValueDecode(folderRelativeUrl.Substring(SelectedWeb.ServerRelativeUrl.TrimEnd('/').Length + 1));
            if (ClientContext.HasPendingRequest) ClientContext.ExecuteQuery();
            try
            {
                using (var fi = SPFile.OpenBinaryDirect(ClientContext, file.ServerRelativeUrl))
                using (var ms = new MemoryStream())
                {
                    // We are using a temporary memory stream because the file connector is seeking in the stream
                    // and the stream provided by OpenBinaryDirect does not allow it
                    fi.Stream.CopyTo(ms);
                    ms.Position = 0;
                    AddFileToTemplate(template, ms, folderWebRelativeUrl, file.Name, folderWebRelativeUrl);
                }
            }
            catch (WebException exc)
            {
                WriteWarning($"Can't add file from url {file.ServerRelativeUrl} : {exc}");
            }
        }

        /// <summary>
        /// Adds a local file to a template
        /// </summary>
        /// <param name="template">Template to add the file to</param>
        /// <param name="file">Full path to a local file</param>
        /// <param name="folder">Destination folder of the added file</param>
        protected void AddLocalFileToTemplate(ProvisioningTemplate template, string file, string folder)
        {
            var fileName = System.IO.Path.GetFileName(file);
            var container = !string.IsNullOrEmpty(Container) ? Container : folder.Replace("\\", "/");
            using (var fs = System.IO.File.OpenRead(file))
            {
                AddFileToTemplate(template, fs, folder.Replace("\\", "/"), fileName, container);
            }
        }
    }
}