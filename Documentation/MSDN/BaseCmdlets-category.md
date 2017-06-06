# Base Cmdlets 
Cmdlet|Description
:-----|:----------
**[Get&#8209;PnPAuthenticationRealm](GetPnPAuthenticationRealm.md)** |Gets the authentication realm for the current web
**[Get&#8209;PnPAzureADManifestKeyCredentials](GetPnPAzureADManifestKeyCredentials.md)** |Creates the JSON snippet that is required for the manifest JSON file for Azure WebApplication / WebAPI apps
**[Get&#8209;PnPContext](GetPnPContext.md)** |Returns a Client Side Object Model context
**[Set&#8209;PnPContext](SetPnPContext.md)** |Sets the Client Context to use by the cmdlets
**[Get&#8209;PnPHealthScore](GetPnPHealthScore.md)** |Retrieves the current health score value of the server
**[Connect&#8209;PnPOnline](ConnectPnPOnline.md)** |Connects to a SharePoint site and creates a context that is required for the other PnP Cmdlets
**[Disconnect&#8209;PnPOnline](DisconnectPnPOnline.md)** |Disconnects the context
**[Get&#8209;PnPProperty](GetPnPProperty.md)** |Will populate properties of an object and optionally, if needed, load the value from the server. If one property is specified its value will be returned to the output.
**[Execute&#8209;PnPQuery](ExecutePnPQuery.md)** |Executes any queued actions / changes on the SharePoint Client Side Object Model Context
**[Get&#8209;PnPStoredCredential](GetPnPStoredCredential.md)** |Returns a stored credential from the Windows Credential Manager
**[Set&#8209;PnPTraceLog](SetPnPTraceLog.md)** |Defines if tracing should be turned on. PnP Core, which is the foundation of these cmdlets uses the standard Trace functionality of .NET. With this cmdlet you can turn capturing of this trace to a log file on or off.
