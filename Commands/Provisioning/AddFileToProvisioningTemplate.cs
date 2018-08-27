using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.Add, "PnPFileToProvisioningTemplate")]
    [CmdletHelp("Adds a file to a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder",
       Remarks = "Adds a file to a PnP Provisioning Template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.xml -Source $sourceFilePath -Folder $targetFolder",
       Remarks = "Adds a file reference to a PnP Provisioning XML Template",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source ""./myfile.png"" -Folder ""folderinsite"" -FileLevel Published -FileOverwrite:$false",
       Remarks = "Adds a file to a PnP Provisioning Template, specifies the level as Published and defines to not overwrite the file if it exists in the site.",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -Source $sourceFilePath -Folder $targetFolder -Container $container",
       Remarks = "Adds a file to a PnP Provisioning Template with a custom container for the file",
       SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Add-PnPFileToProvisioningTemplate -Path template.pnp -SourceUrl $urlOfFile",
        Remarks = "Adds a file to a PnP Provisioning Template retrieved from the currently connected web. The url can be either full, server relative or Web relative url.",
        SortOrder = 4)]
    public class AddFileToProvisioningTemplate : BaseFileProvisioningCmdlet
    {
        /*
* Path, FileLevel, FileOverwrite and TemplateProviderExtensions fields are in the base class
* */

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PSNAME_LOCAL_SOURCE, HelpMessage = "The file to add to the in-memory template, optionally including full path.")]
        public string Source;

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PSNAME_REMOTE_SOURCE, HelpMessage = "The file to add to the in-memory template, specifying its url in the current connected Web.")]
        public string SourceUrl;

        [Parameter(Mandatory = true, Position = 2, ParameterSetName = PSNAME_LOCAL_SOURCE, HelpMessage = "The target Folder for the file to add to the in-memory template.")]
        public string Folder;

        protected override void ProcessRecord()
        {
            var template = LoadTemplate();
            if (this.ParameterSetName == PSNAME_REMOTE_SOURCE)
            {
                SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                var sourceUri = new Uri(SourceUrl, UriKind.RelativeOrAbsolute);

                // Get the server relative url of the file, whatever the input url is (absolute, server relative or web relative form)
                var serverRelativeUrl =
                    sourceUri.IsAbsoluteUri ? sourceUri.AbsolutePath : // The url is absolute, extract the absolute path (http://server/sites/web/folder/file)
                    SourceUrl.StartsWith("/", StringComparison.Ordinal) ? SourceUrl : // The url is server relative. Take it as is (/sites/web/folder/file)
                    SelectedWeb.ServerRelativeUrl.TrimEnd('/') + "/" + SourceUrl; // The url is web relative, prepend by the web url (folder/file)

                var file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);

                AddSPFileToTemplate(template, file);
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(Source))
                {
                    Source = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Source);
                }

                // Load the file and add it to the .PNP file
                Folder = Folder.Replace("\\", "/");

                AddLocalFileToTemplate(template, Source, Folder);
            }
        }
    }
}