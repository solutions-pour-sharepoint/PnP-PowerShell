# Get-PnPUnifiedGroup
Gets one Office 365 Group (aka Unified Group) or a list of Office 365 Groups
## Syntax
```powershell
Get-PnPUnifiedGroup [-Identity <UnifiedGroupPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|UnifiedGroupPipeBind|False|The Identity of the Office 365 Group.|
## Examples

### Example 1
```powershell
PS:> Get-PnPUnifiedGroup
```
Retrieves all the Office 365 Groups

### Example 2
```powershell
PS:> Get-PnPUnifiedGroup -Identity $groupId
```
Retrieves a specific Office 365 Group based on its ID

### Example 3
```powershell
PS:> Get-PnPUnifiedGroup -Identity $groupDisplayName
```
Retrieves a specific Office 365 Group based on its DisplayName

### Example 4
```powershell
PS:> Get-PnPUnifiedGroup -Identity $groupSiteUrl
```
Retrieves a specific Office 365 Group based on the URL of its Modern SharePoint site

### Example 5
```powershell
PS:> Get-PnPUnifiedGroup -Identity $group
```
Retrieves a specific Office 365 Group based on its object instance
