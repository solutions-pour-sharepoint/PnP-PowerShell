using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using PnPFileLevel = OfficeDevPnP.Core.Framework.Provisioning.Model.FileLevel;
using SPFile = Microsoft.SharePoint.Client.File;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.Add, "PnPFilesToProvisioningTemplate")]
    [CmdletHelp("Adds files to a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFilesToProvisioningTemplate -Path template.pnp -SourceFolder $sourceFolder -Folder $targetFolder",
       Remarks = "Adds files to a PnP Provisioning Template from a local folder",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.xml -SourceFolder $sourceFolder -Folder $targetFolder",
       Remarks = "Adds files reference to a PnP Provisioning XML Template",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolder ""./myfolder"" -Folder ""folderinsite"" -FileLevel Published -FileOverwrite:$false",
       Remarks = "Adds files to a PnP Provisioning Template, specifies the level as Published and defines to not overwrite the files if it exists in the site.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolder $sourceFolder -Folder $targetFolder -Container $container",
       Remarks = "Adds files to a PnP Provisioning Template with a custom container for the file",
       SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceFolderUrl $urlOfFolder",
        Remarks = "Adds files to a PnP Provisioning Template retrieved from the currently connected web. The url can be either full, server relative or Web relative url.",
        SortOrder = 4)]
    public class AddFilesToProvisioningTemplate : PnPWebCmdlet
    {
        private const string PSNAME_REMOTE_SOURCE_FOLDER = "RemoteSourceFolder";
        private const string PSNAME_LOCAL_SOURCE_FOLDER = "LocalSourceFolder";

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML provisioning template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PSNAME_LOCAL_SOURCE_FOLDER, HelpMessage = "The source folder to add to the in-memory template, optionally including full path.")]
        public string SourceFolder;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PSNAME_REMOTE_SOURCE_FOLDER, HelpMessage = "The source folder to add to the in-memory template, specifying its url in the current connected Web.")]
        public string SourceFolderUrl;

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = PSNAME_LOCAL_SOURCE_FOLDER, HelpMessage = "The target Folder for the source folder to add to the in-memory template.")]
        public string Folder;

        [Parameter(Mandatory = false, Position = 3, HelpMessage = "The target Container for the file to add to the in-memory template, optional argument.")]
        public string Container;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "The level of the files to add. Defaults to Published")]
        public PnPFileLevel FileLevel = PnPFileLevel.Published;

        [Parameter(Mandatory = false, Position = 5, HelpMessage = "Set to overwrite in site, Defaults to true")]
        public SwitchParameter FileOverwrite = true;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        protected override void ProcessRecord()
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
            if (this.ParameterSetName == PSNAME_REMOTE_SOURCE_FOLDER)
            {
                SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                var sourceUri = new Uri(SourceFolderUrl, UriKind.RelativeOrAbsolute);
                var serverRelativeUrl =
                    sourceUri.IsAbsoluteUri ? sourceUri.AbsolutePath :
                    SourceFolderUrl.StartsWith("/", StringComparison.Ordinal) ? SourceFolderUrl :
                    SelectedWeb.ServerRelativeUrl.TrimEnd('/') + "/" + SourceFolderUrl;

                var folder = SelectedWeb.GetFolderByServerRelativeUrl(serverRelativeUrl);

                var files = EnumRemoteFiles(folder, true).OrderBy(f => f.ServerRelativeUrl);
                foreach (var file in files)
                {
                    AddSPFileToTemplate(template, file);
                }
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(SourceFolder))
                {
                    SourceFolder = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, SourceFolder);
                }

                var files = System.IO.Directory.GetFiles(SourceFolder, "*", SearchOption.AllDirectories).OrderBy(f => f);

                foreach (var file in files)
                {
                    var localFileFolder = System.IO.Path.GetDirectoryName(file);
                    var relativeFolder = Folder + localFileFolder.Substring(SourceFolder.Length);
                    // Load the file and add it to the .PNP file

                    var fileName = System.IO.Path.GetFileName(file);
                    var container = !string.IsNullOrEmpty(Container) ? Container : relativeFolder;
                    using (var fs = System.IO.File.OpenRead(file))
                    {
                        AddFileToTemplate(template, fs, relativeFolder.Replace("\\", "/"), fileName, container);
                    }
                }
            }
        }

        private void AddSPFileToTemplate(ProvisioningTemplate template, SPFile file)
        {
            var fileName = file.EnsureProperty(f => f.Name);
            var folderRelativeUrl = file.ServerRelativeUrl.Substring(0, file.ServerRelativeUrl.Length - fileName.Length - 1);
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
                    AddFileToTemplate(template, ms, folderWebRelativeUrl, fileName, folderWebRelativeUrl);
                }
            }
            catch (WebException exc)
            {
                WriteWarning($"Can't add file from url {file.ServerRelativeUrl} : {exc}");
            }
        }

        /// <summary>
        /// Add a file to the template
        /// </summary>
        /// <param name="template">The provisioning template to add the file to</param>
        /// <param name="fs">Stream to read the file content</param>
        /// <param name="folder">target folder in the provisioning template</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="container">Container of the file (PnP file or folder where the template is located)</param>
        private void AddFileToTemplate(ProvisioningTemplate template, Stream fs, string folder, string fileName, string container)
        {
            var source = !string.IsNullOrEmpty(container) ? (container + "/" + fileName) : fileName;

            template.Connector.SaveFileStream(fileName, container, fs);

            if (template.Connector is ICommitableFileConnector)
            {
                ((ICommitableFileConnector)template.Connector).Commit();
            }

            var existing = template.Files.FirstOrDefault(f =>
              f.Src == $"{container}/{fileName}"
              && f.Folder == folder);

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

        private IEnumerable<SPFile> EnumRemoteFiles(Microsoft.SharePoint.Client.Folder folder, bool recurse)
        {
            var ctx = folder.Context;

            ctx.Load(folder.Files, files => files.Include(f => f.ServerRelativeUrl, f => f.Name));
            ctx.ExecuteQueryRetry();

            foreach (var file in folder.Files)
            {
                yield return file;
            }

            if (recurse)
            {
                ctx.Load(folder.Folders);
                ctx.ExecuteQueryRetry();

                foreach (var subFolder in folder.Folders)
                {
                    foreach (var file in EnumRemoteFiles(subFolder, recurse))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}