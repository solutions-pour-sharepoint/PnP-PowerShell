# Add-PnPEventReceiver
Adds a new event receiver
## Syntax
```powershell
Add-PnPEventReceiver -Name <String>
                     -Url <String>
                     -EventReceiverType <EventReceiverType>
                     -Synchronization <EventReceiverSynchronization>
                     [-List <ListPipeBind>]
                     [-SequenceNumber <Int>]
                     [-Force [<SwitchParameter>]]
                     [-Web <WebPipeBind>]
```


## Returns
>[Microsoft.SharePoint.Client.EventReceiverDefinition](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceiverdefinition.aspx)

## Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|EventReceiverType|EventReceiverType|True|The type of the event receiver like ItemAdded, ItemAdding|
|Name|String|True|The name of the event receiver|
|Synchronization|EventReceiverSynchronization|True|The Synchronization type, Asynchronous or Synchronous|
|Url|String|True|The URL of the event receiver web service|
|Force|SwitchParameter|False|Overwrites the output file if it exists.|
|List|ListPipeBind|False|The list object or name where the event receiver needs to be added|
|SequenceNumber|Int|False|The sequence number where this event receiver should be placed|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
## Examples

### Example 1
```powershell
PS:> Add-PnPEventReceiver -List "ProjectList" -Name "TestEventReceiver" -Url https://yourserver.azurewebsites.net/eventreceiver.svc -EventReceiverType ItemAdded -Synchronization Asynchronous
```
This will add a new event receiver that is executed after an item has been added to the ProjectList list
