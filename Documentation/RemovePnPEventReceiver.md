# Remove-PnPEventReceiver
Removes/unregisters a specific event receiver
## Syntax
```powershell
Remove-PnPEventReceiver -Identity <GuidPipeBind>
                        [-List <ListPipeBind>]
                        [-Force [<SwitchParameter>]]
                        [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|True|The Guid of the event receiver on the list|
|Force|SwitchParameter|False|Specifying the Force parameter will skip the confirmation question.|
|List|ListPipeBind|False|The list object from where to get the event receiver object|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPEventReceiver -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```
This will remove an event receiver with id fb689d0e-eb99-4f13-beb3-86692fd39f22 from the current web

### Example 2
```powershell
PS:> Remove-PnPEventReceiver -List ProjectList -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```
This will remove an event receiver with id fb689d0e-eb99-4f13-beb3-86692fd39f22 from the list with name "ProjectList"
