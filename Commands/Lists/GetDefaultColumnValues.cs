﻿using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPDefaultColumnValues")]
    [CmdletAlias("Get-SPODefaultColumnValues")]
    [CmdletHelp("Gets the default column values for all folders in document library",
        DetailedDescription = "Gets the default column values for a document library, per folder. Supports both text, people and taxonomy fields.",
        Category = CmdletHelpCategory.Lists)]
    public class GetDefaultColumnValues : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Name or Url of the list.")]
        public ListPipeBind List;

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                if (list.BaseTemplate == (int)ListTemplateType.DocumentLibrary || list.BaseTemplate == (int)ListTemplateType.WebPageLibrary || list.BaseTemplate == (int)ListTemplateType.PictureLibrary)
                {
                    var defaultValues = list.GetDefaultColumnValues();
                    var dynamicList = new List<dynamic>();
                    foreach (var dict in defaultValues)
                    {
                        dynamicList.Add(
                            new
                            {
                                Path = dict["Path"],
                                Field = dict["Field"],
                                Value = dict["Value"]
                            });

                    }
                    WriteObject(dynamicList, true);
                }
            }
            else
            {
                WriteWarning("List is not a document library");
            }
        }
    }
}
