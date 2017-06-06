﻿using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace SharePointPnP.PowerShell.Commands.Principals
{
    [Cmdlet(VerbsCommon.Set, "PnPGroupPermissions")]
    [CmdletAlias("Set-SPOGroupPermissions")]
    [CmdletHelp("Adds and/or removes permissions of a specific SharePoint group",
        Category = CmdletHelpCategory.Principals)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -AddRole Contribute",
        Remarks = "Adds the 'Contribute' permission to the SharePoint group with the name 'My Site Members'",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -RemoveRole 'Full Control' -AddRole 'Read'",
        Remarks = "Removes the 'Full Control' from and adds the 'Contribute' permissions to the SharePoint group with the name 'My Site Members'",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -AddRole @('Contribute', 'Design')",
        Remarks = "Adds the 'Contribute' and 'Design' permissions to the SharePoint group with the name 'My Site Members'",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -RemoveRole @('Contribute', 'Design')",
        Remarks = "Removes the 'Contribute' and 'Design' permissions from the SharePoint group with the name 'My Site Members'",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Set-PnPGroupPermissions -Identity 'My Site Members' -List 'MyList' -RemoveRole @('Contribute')",
        Remarks = "Removes the 'Contribute' permissions from the list 'MyList' for the group with the name 'My Site Members'",
        SortOrder = 5)]
    public class SetGroupPermissions : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = "ByName", HelpMessage = "Get the permissions of a specific group by name")]
        [Alias("Name")]
        public GroupPipeBind Identity = new GroupPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "The list to apply the command to.")]
        public ListPipeBind List = new ListPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "Name of the permission set to add to this SharePoint group")]
        public string[] AddRole = null;

        [Parameter(Mandatory = false, HelpMessage = "Name of the permission set to remove from this SharePoint group")]
        public string[] RemoveRole = null;

        protected override void ExecuteCmdlet()
        {
            var group = Identity.GetGroup(SelectedWeb);
            
            List list = List.GetList(SelectedWeb);
            if (list == null && !string.IsNullOrEmpty(List.Title))
            {
                throw new Exception($"List with Title {List.Title} not found");
            }
            else if (list == null && List.Id != Guid.Empty )
            {
                throw new Exception($"List with Id {List.Id} not found");
            }

            if (AddRole != null)
            {
                foreach (var role in AddRole)
                {
                    var roleDefinition = SelectedWeb.RoleDefinitions.GetByName(role);
                    var roleDefinitionBindings = new RoleDefinitionBindingCollection(ClientContext) { roleDefinition };

                    RoleAssignmentCollection roleAssignments;
                    if (list != null)
                    {
                        roleAssignments = list.RoleAssignments;
                    }
                    else
                    {
                        roleAssignments = SelectedWeb.RoleAssignments;
                    }

                    roleAssignments.Add(group, roleDefinitionBindings);
                    ClientContext.Load(roleAssignments);
                    ClientContext.ExecuteQueryRetry();
                }
            }
            if (RemoveRole != null)
            {
                foreach (var role in RemoveRole)
                {
                    RoleAssignment roleAssignment;
                    if (list != null)
                    {
                        roleAssignment = list.RoleAssignments.GetByPrincipal(group);
                    }
                    else
                    {
                        roleAssignment = SelectedWeb.RoleAssignments.GetByPrincipal(group);
                    }
                    var roleDefinitionBindings = roleAssignment.RoleDefinitionBindings;
                    ClientContext.Load(roleDefinitionBindings);
                    ClientContext.ExecuteQueryRetry();
                    foreach (var roleDefinition in roleDefinitionBindings.Where(roleDefinition => roleDefinition.Name == role))
                    {
                        roleDefinitionBindings.Remove(roleDefinition);
                        roleAssignment.Update();
                        ClientContext.ExecuteQueryRetry();
                        break;
                    }
                }
            }
        }
    }
}
