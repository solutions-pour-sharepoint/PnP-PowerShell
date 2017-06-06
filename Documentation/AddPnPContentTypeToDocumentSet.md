# Add-PnPContentTypeToDocumentSet
Adds a content type to a document set
## Syntax
```powershell
Add-PnPContentTypeToDocumentSet -ContentType <ContentTypePipeBind[]>
                                -DocumentSet <DocumentSetPipeBind>
                                [-Web <WebPipeBind>]
```


## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|ContentType|ContentTypePipeBind[]|True|The content type object, name or id to add. Either specify name, an id, or a content type object.|
|DocumentSet|DocumentSetPipeBind|True|The document set object or id to add the content type to. Either specify a name, a document set template object, an id, or a content type object|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPContentTypeToDocumentSet -ContentType "Test CT" -DocumentSet "Test Document Set"
```
This will add the content type called 'Test CT' to the document set called ''Test Document Set'

### Example 2
```powershell
PS:> $docset = Get-PnPDocumentSetTemplate -Identity "Test Document Set"
PS:> $ct = Get-SPOContentType -Identity "Test CT"
PS:> Add-PnPContentTypeToDocumentSet -ContentType $ct -DocumentSet $docset
```
This will add the content type called 'Test CT' to the document set called ''Test Document Set'

### Example 3
```powershell
PS:> Add-PnPContentTypeToDocumentSet -ContentType 0x0101001F1CEFF1D4126E4CAD10F00B6137E969 -DocumentSet 0x0120D520005DB65D094035A241BAC9AF083F825F3B
```
This will add the content type called 'Test CT' to the document set called ''Test Document Set'
