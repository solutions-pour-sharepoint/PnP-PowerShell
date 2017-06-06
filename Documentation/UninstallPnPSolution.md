# Uninstall-PnPSolution
Uninstalls a sandboxed solution from a site collection
## Syntax
```powershell
Uninstall-PnPSolution -PackageId <GuidPipeBind>
                      -PackageName <String>
                      [-MajorVersion <Int>]
                      [-MinorVersion <Int>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|PackageId|GuidPipeBind|True|ID of the solution, from the solution manifest|
|PackageName|String|True|Filename of the WSP file to uninstall|
|MajorVersion|Int|False|Optional major version of the solution, defaults to 1|
|MinorVersion|Int|False|Optional minor version of the solution, defaults to 0|
