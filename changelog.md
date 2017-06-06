# SharePointPnP.PowerShell Changelog #

**2017-05-06**
* Added Set-PnPWebPermissions
* Updated Get-PnPListItem to execute scriptblocks
* Added Set-PnPListItemPermissions
* Added Get-PnPDefaultColumnValues
* Added Set-PnPListPermissions


**2017-01-27**
* Added Get-PnPTerm
* Added Get-PnPTermSet
* Added New-PnPTerm
* Added New-PnPTermSet
* Added New-PnPTermGroup
* Updated Get-PnPTermGroup to optionally return all termgroups in a TermStore

**2017-01-22**
* Introducing the -Includes parameter. The parameter will allow you (on selected cmdlets) to retrieve values for properties that are not being retrieved by default. The parameter exposes the possible values on pressing tab, and you can specify multiple values. The parameter is available on the following cmdlets: Get-PnPAppInstance, Get-PnPCustomAction, Get-PnPDocumentSetTemplate, Get-PnPEventReceiver, Get-PnPFeature, Ensure-PnPFolder, Get-PnPFolder, Get-PnPList, Get-PnPView, Get-PnPGroup, Get-PnPRecyclyBinItem, Get-PnPSite, Get-PnPTermGroup, Get-PnPWeb.
* Updated the output of a view cmdlets so return table formatted data.

**2017-01-14**
* Added Submit-PnPSearchQuery cmdlet
* Added Set-PnPSiteClosure and Get-PnPSiteClosure cmdlets
* Added Get-PnPContentTypePublishingHubUrl
* Added Get-PnPSiteCollectionTermStore which returns the Site Collection Term Store.

**2017-01-05**
* Added Get-PnPTenantRecyclyBinItem cmdlet to list all items in the tenant scoped recycle bin
* Added -Wait and -LockState properties to Set-PnPTenantSite
* The Tenant cmdlets now report progress if the -Wait parameter is specified (where applicable)

**2017-01-03**
* HAPPY NEW YEAR!
* Added Clear-PnPRecyclyBinItem, Clear-PnPTenantRecyclyBinItem, Get-PnPRecyclyBinItem, Move-PnPRecyclyBinItem, Restore-PnPRecyclyBinItem, Restore-PnPTenantRecyclyBinItem cmdlets
* Added Move-PnPFolder, Rename-PnPFolder cmdlets
* Added Add-PnPPublishingImageRendition, Get-PnPPublishingImageRendition and Remove-PnPPublishingImageRendition cmdlets
* Refactored Get-PnPFile. ServerRelativeUrl and SiteRelativeUrl are now obsolete (but will still work), use the Url parameter instead which takes either a server or site relative url.

**2016-11-21**
* Added support to enable versionining and set the maximum number of versions to keep on a list and library with Set-PnPList
* Updated Add-PnPUserToGroup to allow to send invites to external users

**2016-11-09**
* Added Set-PnPUnifiedGroup cmdlet

**2016-11-01**
* Exposed ResetSubwebsToInherit and UpdateRootwebOnly parameters to Set-PnPTheme.

**2016-10-29**
* Marked Get-SPOSite as deprecated. We will remove this cmdlet in the January 2017 release. Please switch as soon as possible to Get-PnPSite instead. A warning will be shown the moment Get-SPOSite is used.
* Renamed all cmdlet verbs from -SPO* to -PnP*. From now all cmdlets follow the *Verb*-PnP*Noun* pattern. There are corresponding aliases available now that allow existing scripts to continue to work.

**2016-10-19**
* Added Get-SPOProvisioningTemplateFromGallery cmdlet

**2016-10-13**
* Added Get-SPOFolder cmdlet
* Minor update to Set-SPOListItem
* Added attributes to Get-SPOFile
* Added return type to generated documentation for those cmdlets that return an object or value

**2016-10-01**
* Added Load-SPOPRovisioningTemplate
* Added Save-SPOProvisioningTemplate

**2016-09-29**
* Live from MS Ignite: Added Remove-SPOTaxonomyItem cmdlet
* Live from MS Ignite: Added Remove-SPOTermGroup cmdlet

**2016-06-03**
* Added Add-SPODocumentSet cmdlet

**2016-06-02**
* Added Enable-SPOResponsiveUI and Disable-SPOResponsiveUI cmdlets
* Added -CreateDrive parameter to Connect-SPOnline cmdlet, allowing to create a virtual drive into a SharePoint site
* Added Invoke-SPOWebAction cmdlet

**2016-05-09**
* Namespace, Assembly and Project rename from OfficeDevPnP.PowerShell to SharePointPnP.PowerShell

**2016-04-08**
* Added -ExtensibilityHandlers parameter to Get-SPOPRovisioningTemplate

**2016-03-11**
* Added List parameter to Get-SPOContentType, allowing to retrieve the ContentTypes added to a list.

**2016-03-08**
* Added Remove-SPOListItem
* Updated Get-SPOWeb and Get-SPOSubWebs to include ServerRelativeUrl
* Added Ensure-SPOFolder cmdlet

**2016-03-07**
* Added Remove-SPOFieldFromContentType cmdlet
* Added Get-SPOSiteSearchQueryResults cmdlet

**2016-02-04**
* Added -PersistPublishingFiles and -IncludeNativePublishingFiles parameters to Get-SPOProvisioningTemplate

**2016-02-03 **
* Added -ExcludedHandlers attribute to Apply-SPOProvisioningTemplate and Get-SPOPRovisioningTemplate
**2016-02-01**

* Added Convert-SPOProvisioningTemplate cmdlet

**2015-12-26**

* Added -AsIncludeFile parameter to New-SPOProvisioningTemplateFromFolder

**2015-12-21**

* Added a Set-SPOContext cmdlet

**2015-12-14**

* Added Set-SPOListItem cmdlet

**2015-11-21**

* Added, where applicable, Site Relative Url parameters, besides the existing Server Relative Url parameters on cmdlets.
* Implemented the use of PnP Monitored Scope. Turn on the trace log with Set-SPOTraceLog -On -Level Information -LogFile c:\pathtoyourlogfile.log to see the tracelog.
* Added a Get-SPOTheme cmdlet

**2015-10-26**

* Added New-SPOProvisioningTemplateFromFolder cmdlet

**2015-10-14**

* Added optional -Encoding parameter to Export-SPOTaxonomy

**2015-09-23**

* Update Get-SPOSearchConfiguration and Set-SPOSearchConfiguration to support Path parameter to export to or import from a file

**2015-09-21**

* Added -Parameters parameter to Apply-SPOProvisioningTemplate. See help for the cmdlet for more info.
* Renamed PageUrl parameter of Webpart cmdlets to ServerRelativePageUrl. Set PageUrl as parameter alias to not break existing scripts.

**2015-09-17**

* Added Get-SPOProperty to dynamically load specified properties from objects.

**2015-09-10**

* Renamed Path parameter of Set-SPOHomePage to RootFolderRelativeUrl. Set Path as parameter alias.

**2015-09-02**

* Started adding unit tests
* Added warning when using Install-SPOSolution to documentation. The cmdlet can potentially clear the composed look gallery.

**2015-08-18**

* Added Set-SPOTraceLog cmdlet

**2015-08-15**

* Added -Recurse parameter to Get-SPOSubWebs cmdlet to recursively retrieve all subwebs

**2015-08-14**

* Modified Connect-SPOnline to output version number when specifying -Verbose parameter

**2015-08-10**

* Added Get-SPOWebPartXml cmdlet to export webpart XML from a page.

**2015-08-07**

* Added Set-SPOUserProfileProperty (only available for SharePoint Online due to limitations of the On-Premises CSOM SDK)
**2015-07-22**

* Added Remove-SPOGroup cmdlet

**2015-07-14**

* Added additional attribute (-Key) to Get-SPOWebPartProperty cmdlet

**2015-07-13**

* Added additional functionality for connect-sponline in resolving credentials. If no credentials are specified throught the -Credentials parameter, a query is done against the Windows Credentials Manager to retrieve credentials. First is checked for the full URL of the connect request, e.g. https://yourserver/sites/yoursite. If no credential is found for that entry, a query is done for for https://yourserver/sites. If no credential is found that entry, a query is done for https://yourserver, if no credential is found for that entry a query is done for 'yourserver'. So:
```
Connect-SPOnline -Url https://yourtenant.sharepoint.com/sites/demosite
``` 
will mean that it will check your credential manager for entries in this order:

```
https://yourtenant.sharepoint.com/sites/demosite
https://yourtenant.sharepoint.com/sites
https://yourtenant.sharepoint.com
yourtenant.sharepoint.com
```

Notice that using
```
Connect-SPOnline -Url https://yourtenant.sharepoint.com/sites/demosite -Credentials <yourlabel>
```
still works as before.

**2015-07-08**

* Added Get-SPOSearchConfiguration and Set-SPOSearchConfiguration cmdlets
* Added support for folder property bags in Set-SPOPropertyBagValue, Get-SPOPropertyBag and Remove-SPOPropertyBagValue. See the help of the cmdlets for more details and examples.

**2015-07-01**

* Added Add-SPOIndexedProperty and Remove-SPOIndexedProperty to allow adding or removing single keys from a set of indexed properties.

**2015-06-29**

* Added OverwriteSystemPropertyBagValues parameter to Apply-SPOProvisioningTemplate cmdlet
* Updated installer to allow for setting advanced properties.

**2015-06-10**

* Changed installers from 64 bit to 32 bit.
* Added ResourceFolder parameter to Apply-SPOProvisioningTemplate cmdlet

**2015-06-03**

* Added OnQuickLaunch parameter to New-SPOList cmdlet

**2015-06-01**

* Added Add-SPOWorkflowDefinition cmdlet
* Updated Add-SPOField to allow for -Field parameter to add a site column to a list.

**2015-05-28**

* Added Set-SPOSitePolicy and Get-SPOSitePolicy cmdlets

**2015-05-22**

* Updated Add-SPOHtlPublishingPageLayout and Add-SPOPublishingPageLayout to support DestinationFolderHierarchy parameter
* Updated Add-SPOFile to create the target folder is not present
* Updated Remove-SPOUserFromGroup to accept either a login name or an email address of a user.

**2015-05-15**

* Updated Set-SPOList to switching if ContentTypes are enabled on the list

**2015-04-24**

* Updated Get-SPOProvisioningTemplate and Apply-SPOProvisioningTemplate to show a progress bar
* Updated GEt-SPOProvisioningTemplate with optional switches to export either Site Collection Term Group (if available) or all Term Groups in the default site collection termstore.
* Added Export-SPOTermGroup cmdlet that supports the provisioning engine XML format
* Added Import-SPOTermGroup cmdlet that supports the provisioning engine XML format

**2015-04-20**

* Admin cmdlets: Get-SPOTenantSite, New-SPOTenantSite, Remove-SPOTenantSite, Set-SPOTenantSite and Get-SPOWebTemplates now automatically switch context. This means that you don't have to connect to https://<tenant>-admin.sharepoint.com first in order to execute them.

**2015-04-08**

* Added Apply-SPOProvisioningTemplate cmdlet
* Added Get-SPOPRovisioningTemplate cmdlet
* Extended Enable-SPOFeature cmdlet to handle Sandboxed features

**2015-03-11**

* Added Get-SPOJavaScript link cmdlet
* Refactored JavaScript related cmdlets to use -Name parameter instead of -Key (-Key still works for backwards compatibility reasons)
* Refactored JavaScript related cmdlets to use -Scope [Web|Site] parameter instead of -FromSite, -SiteScoped and -AddToSite parameters. The old parameters still work for backwards compatibility reasons.
* Fixed an issue in cmdlet help generation where the syntax would not be shown for cmdlets with only one parameter set.

**2015-03-10**

* Added Sequence parameter to Add-SPOJavaScriptLink and Add-SPOJavaScriptBlock cmdlets
* Added Remove-SPOFile cmdlet

**2015-02-25**

* Updated Location parameter in Add-/Remove-SPONavigationNode

**2015-01-07**

* Introduced new Cmdlet: Get-SPOWebPartProperty to return webpart properties
* Updated Set-SPOWebPartProperty cmdlet to support int values

**2015-01-02**

* Removed SetAssociatedGroup parameter from new-spogroup cmdlet and moved it to a separate cmdlet: Set-SPOGroup
* Introduced new Cmdlet: Set-SPOGroup to set the group as an associated group and optionally add or remove role assignments
* Introduced new Cmdlet: Set-SPOList to set list properties
* Introduced new Cmdlet: Set-SPOListPermission to set list permissions

**2014-12-30**

* Changed New-SPOWeb to return the actual web as an object instead of a success message.
* Added -SetAssociatedGroup parameter to New-SPOGroup to set a group as a default associated visitors, members or owners group
* Updated New-SPOGroup to allow setting groups as owners

**2014-12-01**

* Added Get-SPOListItem cmdlet to retrieve list items by id, unique id, or CAML. Optionally you can define which fields to load.

**2014-11-05**

* Added Add-SPOFolder cmdlet

**2014-11-04**

* Added Get-SPOIndexedPropertyBagKeys cmdlet
* Updated Set-SPOPropertyBagValue to not remove a property from the indexed properties if it was already in the indexed properties.
* Updated Get-SPOTenantSite output formatting

**2014-11-03**

* Split up Add-SPOField into Add-SPOField and Add-SPOFieldFromXml. The latter only accepts XML input while the first takes parameters to create fields

**2014-10-15**

* Added Add-SPOWorkflowSubscription, Get-SPOWorkflowDefinition, Get-SPOWorkflowSubscription, Remove-SPOWorkflowDefinition, Remove-SPOWorkflowSubscription, Resume-SPOWorkflowInstance, Stop-SPOWorkflowInstance

**2014-10-14**

* Added Get-SPOUserProfileProperty cmdlet
* Added New-SPOPersonalSite cmdlet
* Fixed Get-SPOView cmdlet

**2014-10-08**

* Added Set-SPODefaultColumnValue 

**2014-09-19**

* Removed Url Parameters on Add-SPOFile and made Folder parameter mandatory.

**2014-09-06**

* Added new Set-SPOWeb cmdlet to set Title, SiteLogo, or AlternateCssUrl

**2014-09-03**

* Renamed Add-SPOApp to Import-SPOAppPackage to align with server cmdlet
* Renamed Remove-SPOApp to Uninstall-SPOAppInstance to align with server cmdlet

**2014-08-29**

* Removed OfficeDevPnP.PowerShell.Core project, not in use anymore as all cmdlets now make use of the OfficeDevPnP.Core project.

**2014-08-27**

* Split up Add-SPOWebPart in two cmdlets, Add-SPOWebPartToWikiPage and Add-SPOWebPartToWebPartPage, to reduce confusing parameter sets
* Changed parameters of Add-SPOCustomAction cmdlet
* Changed name of Add-SPONavigationLink to Add-SPONavigationNode, in sync with method name of OfficeDevPnP.Core. Changed parameters of cmdlet.


**2014-08-26**

* Updated several commands to use OfficeDevPnP.Core instead of OfficeDevPnP.PowerShell.Core
* Marked SPOSite and SPOTaxonomy as obsolete. Use OfficeDevPnP.Core extensions instead

**2014-08-23**

* Simplified connection code, added functionality to connect with add-in Id and add-in Secret. 
* Added connection samples in samples folder. 
* Added Get-SPORealm command.

**2014-08-22**

* Namespace change from OfficeDevPnP.SPOnline to OfficeDevPnP.PowerShell
