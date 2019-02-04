# SharePointPnP.PowerShell Changelog
*Please do not commit changes to this file, it is maintained by the repo owner.*

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [3.6.1902.0]

### Added
- Added Clear-PnPDefaultColumnValues cmdlet
- Added Remove-PnPSearchConfiguration cmdlet
- Added Export-PnPClientSidePage to export a page to a Provisioning Template
- Added Add-PnPSiteDesignTask to apply a site design to a site. Intended as a replacement for Invoke-PnPSiteDesign as it the task can handle more than 30 actions.
- Added Get-PnPSiteDesignRun to retrieve the list of site designs applied to a site collection
- Added Get-PnPSiteDesignRunStatus to retrieve a list of all site script actions executed for a specified site design applied to a site
- Added Get-PnPSiteDesignTask to retrieve a list of all currently scheduled site design tasks.
- Added Remove-PnPSiteDesignTask to remove a previously scheduled site design task.

### Changed
- Set-PnPDefaultColumnValues: Fixed character encoding issue on folders #1706
- Fixed import of search configuration to tenant via string
- Set-PnPTenantSite: Added support for setting default sharing and sharing permissions
- ConvertTo-PnPClientSidePage: Added support for copying page metadata to the modern version of the page + parameter to clear the transformation cache

## [3.5.1901.0]

### Added
- Added Reset-PnPFileVersion cmdlet

### Changed

- Add-PnPClientSidePageSection: Added support for section the section background of a client side page
- Updated file and folder cmdlets to support special characters

### Deprecated

### Contributors
- Eric Skaggs (skaggej)
- Gautam Sheth (gautamdsheth)

## [3.4.1812.0]

### Added

- ConvertTo-PnPClientSidePage: creates a modern client side page from a classic wiki or web part page

### Changed
- Added support for setting the page header type in Set-PnPClientSidePage

### Deprecated

### Contributors

## [3.3.1811.0]
### Added

### Changed
- Copy-PnPFile now supports special characters like '&' in file names
- Updated New-PnPSite to support language/locale for new sites.
- Updated documentation for New-PnPTenantSite
- Fixed documentation for Measure-PnPWeb, Set-PnPSite
- Updated samples
- Fixes issue with Set-PnPUnifiedGroup where if you only change for instance the displayname a private group would be marked as public.
- Renamed (and created aliases for the old cmdlet name) Apply-PnPProvisioningHierarchy to Apply-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Add-PnPProvisioningSequence to Add-PnPTenantSequence
- Renamed (and created aliases for the old cmdlet name) Add-PnPProvisioningSite to Add-PnPTenantSequenceSite
- Renamed (and created aliases for the old cmdlet name) Add-PnPPnPProvisioningSubSite to Add-PnPTenantSequenceSubSite
- Renamed (and created aliases for the old cmdlet name) Get-PnPProvisioningSequence to Get-PnPTenantSequence
- Renamed (and created aliases for the old cmdlet name) Get-PnPProvisioningSite to Get-PnPTenantSequenceSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningSequence to New-PnPTenantSequence
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningTeamSite to New-PnPTenantSequenceTeamSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningCommunicationSite to New-PnPTenantSequenceCommunicationSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningTeamNoGroupSite to New-PnPTenantSequenceTeamNoGroupSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningTeamNoGroupSubSite to New-PnPTenantSequenceTeamNoGroupSubSite
- Renamed (and created aliases for the old cmdlet name) New-PnPProvisioningHierarchy to New-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Read-PnPProvisioningHierarchy to Read-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Save-PnPProvisioningHierarchy to Save-PnPTenantTemplate
- Renamed (and created aliases for the old cmdlet name) Test-PnPProvisioningHierarchy to Test-PnPTenantTemplate

### Deprecated
- Marked Get-PnPProvisioningTemplateFromGallery as deprecated as the PnP Template Gallery has been shut down.

### Contributors
- Paul Bullock (pkbullock)
- Fran�ois-Xavier Cat (lazywinadmin)
- Koen Zomers (KoenZomers)
- Kevin McDonnell (kevmcdonk)

## [3.2.1810.0] Released
### Added
- Add-PnPProvisioningSequence : Adds an in-memory sequence to an in-memory provisioning hierarchy
- Add-PnPProvisioningSite : Adds an in-memory site definition to a in-memory sequence
- Add-PnPProvisioningSubSite : Adds an in-memory sub site defintion to an in-memory site
- Apply-PnPProvisioningHierarchy : Applies a provisioninghierarchy with a site sequence to a tenant
- Get-PnPProvisioningSite : Returns a site as an in-memory object from a given provisioning hierarchy
- New-PnPProvisioningHierarchy : Creates a new in-memory provisioning hierarchy
- New-PnPProvisioningSequence : Creates a new in-memory provisioning sequence
- New-PnPProvisioningCommunicationSite : Creates a new in-memory communication site definition
- New-PnPProvisioningTeamNoGroupSite : Creates a new in-memory team site definition which has no associated office365 group
- New-PnPProvisioningTeamNoGroupSubSite : Creates a new in-memory team sub site definition which has no associated office365 group
- New-PnPProvisioningTeamSite : Creates a new in-memory team site definition
- Read-PnPProvisioningHierarchy : Reads an existing (file based) provisioning hierarchy into an in-memory instance
- Save-PnPProvisioningHierarchy : Saves an in-memory provisioning hierarchy to a pnp file
- Test-PnPProvisioningHierarchy : Tests an in-memory hierarchy if all template references are correct in the site sequence
- Get-PnPException : Returns the last occured exception that occured while using PowerShell.

### Changed
- Updated Set-PnPSite to allow for setting of a logo on modern team site
- Updated Get-PnPTerm to allow for -IncludeChildTerms parameter, which will load, if available all child terms
- Updated Get-PnPTerm to allow for only specifying the id of a termset, without needing to require to specify the termset and termgroup.

### Deprecated

### Contributors

## [3.1.1809.0]

### Changed
- Minor bugfixes
- Updated core library

## [3.0.1808.0]
### Added
- Added Get-PnPLabel and Set-PnPLabel to get and set compliancy tags/labels on a list or library. Only available for SharePoint Online.

### Changed
- Fixed Get-PnPSearchCrawlLog where listing user profile crawl entries failed for some tenants
- Added default pipebind to Get-PnPListitem 
- Add-PnPDocumentSet now adds the content type to the document library.
- Updated documentation for Clear-PnPRecycleBinItem and Restore-PnPRecycleBinItem
- Updated documentation for New-PnPSite

### Contributors
- KoenZomers
- robinmeure

## [2.28.1807.0]
### Changed
- Added IncludeClassification to Get-PnPUnifiedGroup
- Updated documentation for Get-PnPSearchCrawlLog
- Added -NewFileName to Add-PnPFile cmdlet

### Contributors
- vipulkelkar
- wobba
- koenzomers

## [2.27.1806.0]
### Added
- Added Grant-PnPTenantServicePrincipalPermission to explicitely grant a permission on a resource for the tenant.

### Changed
- Fixed edge cases where progress sent to PowerShell would be null, causing the provisioning of a template to end prematurely.
- Fixed Unregister-PnPHubSite where you could not unregister a hub site if the site was deleted before unregistering

### Deprecated

### Contributors

## [2.26.1805.1]
### Added

- Added -Timeout option to Add-PnPApp
- Added -CollapseSpecification option to Submit-PnPSearchQuery
- Added -InSiteHierarchy to Get-PnPField to search for fields in the site collection
- Added Get-PnPSearchCrawlLog

### Changed
- Fix for issue where using Add-PnPFile and setting Created and Modified did not update values

## [2.26.1805.0]
### Added
- Added Enable-PnPPowerShellTelemetry, Disable-PnPPowerShellTelemetry, Get-PnPPowershellTelemetryEnabled
- Added Enable-PnPTenantServicePrincipal
- Added Disable-PnPTenantServicePrincipal
- Added Get-PnPTenantServicePrincipal
- Added Get-PnPTenantServicePermissionRequests
- Added Get-PnPTenantServicePermissionGrants
- Added Approve-PnPTenantServicePrincipalPermissionRequest
- Added Deny-PnPTenantServicePrincipalPermissionRequest
- Added Revoke-PnPTenantServicePrincipalPermission
- Added -Scope parameter to Get-PnPStorageEntity, Set-PnPStorageEntity and Remove-PnPStorageEntity to allow for handling storage entity on site collection scope. This only works on site collections which have a site collection app catalog available.
- Added -CertificatePassword option to New-PnPAzureCertificate
- Added output of thumbprint for New-PnPAzureCertificate and Get-PnPAzureCertificat

### Changed
- Added -NoTelemetry switch to Connect-PnPOnline
- Updated Connect-PnPOnline to allow for -LoginProviderName when using -UseAdfs to authenticate
- Fixed issue where Add-PnPApp would fail where -Publish parameter was specified and -Scope was set to Site
- Fixed issue where New-PnPUnifiedGroup prompted for creation even though mail alias did not exist

### Deprecated

### Contributors
- Martin Duceb [cebud]
- Kev Maitland [kevmaitland]
- Martin Loitzl [mloitzl]

## [2.25.1804.1]
### Changed
- Now using signed core library assembly
- Updated Set-PnPTenantSite to handle changing the Site Lock State correctly. You cannot use both -LockState and set other properties at the same time due to possible delays in making the lockstate effective.


## [2.25.1804.0]
### Added
- Added -Tree parameter to Get-PnPNavigationNode which will return a tree representation of the selected navigation structure
- Added -Parent parameter which takes an ID to Add-PnPNavigationNode instead of using the -Header parameter
- Added -Scope parameter to Add-PnPApp, Get-PnPApp, Install-PnPApp, Publish-PnPApp, Remove-PnPApp, Uninstall-PnPApp, Unpublish-PnPApp, Update-PnPApp to support site collection app catalog
- Added -Wait parameter to Install-PnPApp which will wait for the installation to finish
- Added Get-PnPHideDefaultThemes cmdlet
- Added Set-PnPHideDefaultThemes cmdlet
- Added Get-PnPListRecordDeclaration cmdlet
- Added Set-PnPListRecordDeclaration cmdlet
- Added Get-PnPInPlaceRecordsManagement cmdlet
- Added Get-PnPInformationRightsManagement cmdlet
- Added Set-PnPInformationRightsManagement cmdlet
- Added New-PnPUPABulkImportJob cmdlet
- Added Get-PnPUPABulkImportStatus cmdlet

### Changed

- Added additional properties to Set-PnPList: Description, EnableFolderCreation, ForceCheckout, ListExperience
- ALM Cmdlets (Add-PnPApp, etc.) now allow for specifying the app title instead of only an id.
- Updated Set-PnPInPlaceRecordsManagement cmdlet to use a -Enabled parameter instead of -On and -Off
- Add-PnPClientSideWebPart and Add-PnPClientSideText now return the client side component added
- Fixed issue with Set-PnPTenantTheme not recognizing a parameter value accordingly
- Added -HideDefaultThemes parameter to Set-PnPTenant
- Get-PnPTenant now returns if default themes are hidden or not
- Added ability to cancel Device Login requests with CTRL+C
- Renamed Connect-PnPHubSite to Add-PnPHubSiteAssociation and added alias for Connect-PnPHubSite
- Renamed Disconnect-PnPHubSite to Remove-PnPHubSiteAssociation and added alias for Disconnect-PnPHubSite
- Fixed output of File/Folder objects which caused the creation of an error message that was not thrown to the output but was available in the $error built-in variable
- Fixed Set-PnPUserProfileProperty cmdlet to accept $null values to clear properties
- Fixed Invoke-PnPSiteDesign where you connected to the -admin URL, and it ignored the WebUrl parameter when applying the site design
- Added WebUrl parameter to Set-PnPWebTheme to support connection via -admin URL needed by app-only connections
- Fixed issue with 

### Deprecated
- Deprecated -Header parameter on Add-PnPNavigationNode in favor or -Parent [Id]
- Deprecated Disable-PnPInPlaceRecordsManagementForSite in favor of Set-PnPInPlaceRecordsManagement -Enabled $true
- Deprecated Enabled-PnPInPlaceRecordsManagementForSite in favor of Set-PnPInPlaceRecordsManagement -Disabled $true
- Deprecated Connect-PnPHubSite. Use Add-PnPHubSiteAssociation
- Deprecated Disconnect-PnPHubSite. Use Remove-PnPHubSiteAssociation

### Contributors
casselc
stevebeauge
velingeorgiev
cebud
jensotto


## [2.24.1803.0] - 2018-03-06
### Added
- Added Get-PnPTenant cmdlet
- Added Set-PnPTenant cmdlet
- Added Set-PnPWebTheme cmdlet
- Added Invoke-PnPSiteDesign cmdlet
- Added Read-PnPProvisioningTemplate cmdlet [Rename: see deprecated section]
- Added Invoke-PnPQuery cmdlet [Rename: see deprecated section]
- Added Resolve-PnPFolder cmdlet [Rename: see deprecated section]
- Added New-PnPAzureCertificate cmdlet
- Added Get-PnPAzureCertificate cmdlet
- Added Test-PnPOffice365GroupAliasIsUsed cmdlet
- Added Remove-PnPStoredCredential
- Added Add-PnPStoredCredential
- Added Get-PnPHubSite cmdlet
- Added Set-PnPHubSite cmdlet
- Added Grant-PnPHubSiteRights cmdlet
- Added Register-PnPHubSite cmdlet
- Added Unregister-PnPHubSite cmdlet
- Added Connect-PnPHubSite cmdlet
- Added Disconnect-PnPHubSite cmdlet
- Added Add-PnPTenantTheme cmdlet
- Added Get-PnPTenantTheme cmdlet
- Added Remove-PnPTenantTheme cmdlet
- Added Set-PnPTenantCdnEnabled cmdlet
- Added Get-PnPTenantCdnEnabled cmdlet
- Added Get-PnPTenantCdnOrigin cmdlet
- Added Add-PnPTenantCdnOrigin cmdlet
- Added Remove-PnPTenantCdnOrigin cmdlet
- Added Get-PnPTenantCdnPolicies cmdlet
- Added Set-PnPTenantCdnPolicy cmdlet
- Added Add-PnPSiteCollectionAppCatalog cmdlet
- Added Remove-PnPSiteCollectionAppCatalog cmdlet
- Added Get-PnPNavigationNode cmdlet
- Added Get-PnPRoleDefinition cmdlet
- Added Add-PnPRoleDefinition cmdlet
- Added Remove-PnPRoleDefinition cmdlet
- Implemented .NET 2.0 Standard project to allow for cross-platform use with PowerShell 6.0

### Changed
- Added "Formula" dynamic parameter to Add-PnPField to allow creating calculated fields.
- Updated Set-PnPClientSidePage to support setting the page title
- Added -Graph [and -LaunchBrowser] option to authenticate with Connect-PnPOnline to the Graph using the PnP O365 Management Shell Azure AD Application 
- Updated the UnifiedGroup cmdlets to also take an Alias of group as a value for the -Identity parameter
- Minor documentations updates [thechriskent]
- Updated Connect-PnPOnline to support connecting using PEM encoded certificate strings
- Updated Connect-PnPOnline for On-Premises to allow for additional HighTrustCertificate parameters [fowl2]
- Added -EnableAttachment parameter for Set-PnPList [Laskewitz]
- Added -Approve parameter for Set-PnPFileCheckedIn [Aproxmiation]
- Added -EnableModeration for Set-PnPList [Apromixation]
- Fixed issue where it was not possible to use New-PnPSite when using Connect-PnPOnline with the -UseWebLogin parameter
- Fixed issue with Copy-PnPFile when copying to a location within the current web where metadata was not being retained
- Fixed issue with Add-PnPFile when a new file was uploaded and using the cmdlet also field values where set, the version would increase to 2.0 instead of the expected 1.0
- Fixed issues with Set-PnPTheme cmdlet not accepting site relative urls
- Move-PnPFolder now returns the folder that has been moved
- Updated Get-PnPStoredCredentials to support .NET Standard
- Updated/fixed documentation on various cmdlets
- Fixed issue with Get-PnPTenantSite not returning all sites in large tenants
- Added -PnPO365ManagementShell [and -LaunchBrowser] login option to Connect-PnPOnline
- Changed changelog format
- Updated Remove-PnPNavigationNode cmdlet to support removal by Id
- Updated Remove-PnPNavigationNode cmdlet to support the -All parameter
- Updated Set-PnPList cmdlet to change moderation setting
- Updated Set-PnPFileCheckedIn to approve the file

### Deprecated
- [SharePoint Online Only] Deprecated Get-PnPAppInstance, Import-PnPAppPackage, Uninstall-PnPAppPackage. Use Add-PnPApp, Install-PnPApp, Publish-PnPApp, Uninstall-PnPApp, Remove-PnPApp instead where applicable.
- Deprecated Load-PnPProvisioningTemplate, renaming it to Read-PnPProvisioningTemplate which follows the PowerShell approved verb standard. Load-PnPProvisioningTemplate has been added as an alias for Read-PnPProvisioningTemplate.
- Deprecated Execute-PnPQuery, renaming it to Invoke-PnPQuery which follows the PowerShell approved verb standard. Execute-PnPQuery has been added as an alias for Invoke-PnPQuery.
- Deprecated Ensure-PnPFolder, moving functionality to Resolve-PnPFolder which follows the PowerShell approved verb standard. Ensure-PnPFolder has been added as an alias for Resolve-PnPFolder.
- Documentation/Markdown generation has been removed from build, now points to https://docs.microsoft.com/en-us/powershell/sharepoint/sharepoint-pnp/sharepoint-pnp-cmdlets?view=sharepoint-ps
- Deprecated Remove-PnPNavigationNode -Title and -Header parameters. Use the Identity parameter instead.
- Marked -WebTemplate parameter on Get-PnPTenantSite as obsolete. Use -Template instead.
- Deprecated Get-PnPAzureADManifestKeyCredentials. Use Get-PnPAzureCertificate instead.

## [2.23.1802.0] - 2018-02-05
### Added
- Added Set-PnPSiteDesign and Set-PnPSiteScript cmdlets

## [2.22.1801.0]
### Added
- Added Get-PnPTenantAppCatalogUrl
- Start-PnPWorkflowInstance
- Get-PnPWorkflowInstance

**2017-12-06**
* Added cmdlets for Site Designs: Add-PnPSiteDesign, Add-PnPSiteScript, Get-PnPSiteDesign, Get-PnPSiteScript, Get-PnPSiteDesignRights, Grant-PnPSiteDesignRights, Remove-PnPSiteDesign, Remove-PnPSiteScript, Revoke-PnPSiteDesignRights

**2017-12-02**
* Added additional authentication option with Connect-PnPOnline allowing you use an existing Access Token for authentication
* Added Get-PnPClientSideComponent, Move-PnPClientSideComponent, Remove-PnPClientSideComponent, Set-PnPClientSideText, Set-PnPClientSideWebPart

**2017-11-20**
* Added Measure-PnPWeb, Measure-PnPList and Measure-PnPResponseTime cmdlets
* Added Set-PnPStorageEntity, Get-PnPStorageEntity and Remove-PnPStorageEntity cmdlets to manage storage entities / farm properties

**2017-11-19**
* Fixed issues with Set-PnPListItem -Values, Add-PnPListItem -Values and Add-PnPFile -Values, updated documentation for these cmdlets
* Added confirmation prompt to Get-PnPFile to ask if local file should be overwritten. Use -Force to overwrite this

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
* Renamed PageUrl parameter of web part cmdlets to ServerRelativePageUrl. Set PageUrl as parameter alias to not break existing scripts.

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

* Added Get-SPOWebPartXml cmdlet to export web part XML from a page.

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

* Introduced new Cmdlet: Get-SPOWebPartProperty to return web part properties
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
