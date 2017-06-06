﻿using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPCustomAction")]
    [CmdletAlias("Add-SPOCustomAction")]
    [CmdletHelp("Adds a custom action to a web", Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = @"$cUIExtn = ""<CommandUIExtension><CommandUIDefinitions><CommandUIDefinition Location=""""Ribbon.List.Share.Controls._children""""><Button Id=""""Ribbon.List.Share.GetItemsCountButton"""" Alt=""""Get list items count"""" Sequence=""""11"""" Command=""""Invoke_GetItemsCountButtonRequest"""" LabelText=""""Get Items Count"""" TemplateAlias=""""o1"""" Image32by32=""""_layouts/15/images/placeholder32x32.png"""" Image16by16=""""_layouts/15/images/placeholder16x16.png"""" /></CommandUIDefinition></CommandUIDefinitions><CommandUIHandlers><CommandUIHandler Command=""""Invoke_GetItemsCountButtonRequest"""" CommandAction=""""javascript: alert('Total items in this list: '+ ctx.TotalListItems);"""" EnabledScript=""""javascript: function checkEnable() { return (true);} checkEnable();""""/></CommandUIHandlers></CommandUIExtension>""

Add-PnPCustomAction -Name 'GetItemsCount' -Title 'Invoke GetItemsCount Action' -Description 'Adds custom action to custom list ribbon' -Group 'SiteActions' -Location 'CommandUI.Ribbon' -CommandUIExtension $cUIExtn",
    Remarks = @"Adds a new custom action to the custom list template, and sets the Title, Name and other fields with the specified values. On click it shows the number of items in that list. Notice: escape quotes in CommandUIExtension.",
    SortOrder = 1)]
    [CmdletRelatedLink(
        Text ="UserCustomAction", 
        Url = "https://msdn.microsoft.com/en-us/library/office/microsoft.sharepoint.client.usercustomaction.aspx")]
    [CmdletRelatedLink(
        Text ="BasePermissions",
        Url = "https://msdn.microsoft.com/en-us/library/office/microsoft.sharepoint.client.basepermissions.aspx")]
    public class AddCustomAction : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the custom action")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The title of the custom action")]
        public string Title = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The description of the custom action")]
        public string Description = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The group where this custom action needs to be added like 'SiteActions'")]
        public string Group = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The actual location where this custom action need to be added like 'CommandUI.Ribbon'")]
        public string Location = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Sequence of this CustomAction being injected. Use when you have a specific sequence with which to have multiple CustomActions being added to the page.")]
        public int Sequence = 0;

        [Parameter(Mandatory = false, HelpMessage = "The URL, URI or ECMAScript (JScript, JavaScript) function associated with the action")]
        public string Url = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The URL of the image associated with the custom action")]
        public string ImageUrl = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "XML fragment that determines user interface properties of the custom action")]
        public string CommandUIExtension = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "The identifier of the object associated with the custom action.")]
        public string RegistrationId = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "A string array that contain the permissions needed for the custom action")]
        public PermissionKind[] Rights;

        [Parameter(Mandatory = false, HelpMessage = "Specifies the type of object associated with the custom action")]
        public UserCustomActionRegistrationType RegistrationType;

        [Parameter(Mandatory = false, HelpMessage = "The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        protected override void ExecuteCmdlet()
        {
            var permissions = new BasePermissions();
            if (Rights != null)
            {
                foreach (var kind in Rights)
                {
                    permissions.Set(kind);
                }
            }

            var ca = new CustomActionEntity
            {
                Name = Name,
                ImageUrl = ImageUrl,
                CommandUIExtension = CommandUIExtension,
                RegistrationId = RegistrationId,
                RegistrationType = RegistrationType,
                Description = Description,
                Location = Location,
                Group = Group,
                Sequence = Sequence,
                Title = Title,
                Url = Url,
                Rights = permissions
            };

            switch (Scope)
            {
                case CustomActionScope.Web:
                    SelectedWeb.AddCustomAction(ca);
                    break;

                case CustomActionScope.Site:
                    ClientContext.Site.AddCustomAction(ca);
                    break;

                case CustomActionScope.All:
                    WriteWarning("CustomActionScope 'All' is not supported for adding CustomActions");
                    break;
            }           
        }
    }
}
