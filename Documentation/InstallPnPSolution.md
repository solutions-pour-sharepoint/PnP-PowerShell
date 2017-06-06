# Install-PnPSolution
Installs a sandboxed solution to a site collection. WARNING! This method can delete your composed look gallery due to the method used to activate the solution. We recommend you to only to use this cmdlet if you are okay with that.
## Syntax
```powershell
Install-PnPSolution -PackageId <GuidPipeBind>
                    -SourceFilePath <String>
                    [-MajorVersion <Int>]
                    [-MinorVersion <Int>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|PackageId|GuidPipeBind|True|ID of the solution, from the solution manifest|
|SourceFilePath|String|True|Path to the sandbox solution package (.WSP) file|
|MajorVersion|Int|False|Optional major version of the solution, defaults to 1|
|MinorVersion|Int|False|Optional minor version of the solution, defaults to 0|
