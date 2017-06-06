# Remove-PnPCustomAction
Removes a custom action
## Syntax
```powershell
Remove-PnPCustomAction -Identity <GuidPipeBind>
                       [-Scope <CustomActionScope>]
                       [-Force [<SwitchParameter>]]
                       [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|GuidPipeBind|True|The identifier of the CustomAction that needs to be removed|
|Force|SwitchParameter|False|Use the -Force flag to bypass the confirmation question|
|Scope|CustomActionScope|False|Define if the CustomAction is to be found at the web or site collection scope. Specify All to allow deletion from either web or site collection.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2
```
Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2'.

### Example 2
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -scope web
```
Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' from the current web.

### Example 3
```powershell
PS:> Remove-PnPCustomAction -Identity aa66f67e-46c0-4474-8a82-42bf467d07f2 -force
```
Removes the custom action with the id 'aa66f67e-46c0-4474-8a82-42bf467d07f2' without asking for confirmation.
