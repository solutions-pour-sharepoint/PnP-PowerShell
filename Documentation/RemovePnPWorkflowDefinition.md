# Remove-PnPWorkflowDefinition
Removes a workflow definition
## Syntax
```powershell
Remove-PnPWorkflowDefinition -Identity <WorkflowDefinitionPipeBind>
                             [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|WorkflowDefinitionPipeBind|True|The definition to remove|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
