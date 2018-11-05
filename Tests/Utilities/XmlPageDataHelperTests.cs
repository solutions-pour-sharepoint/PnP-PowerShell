using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointPnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Utilities.Tests
{
    [TestClass()]
    public class XmlPageDataHelperTests
    {
        [TestMethod()]
        public void ExtractPropertiesTest()
        {
            var sampleContent = @"<%@ Page Inherits=""Microsoft.SharePoint.Publishing.TemplateRedirectionPage,Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"" %> <%@ Reference VirtualPath=""~TemplatePageUrl"" %> <%@ Reference VirtualPath=""~masterurl/custom.master"" %>
<%@ Register Tagprefix=""SharePoint"" Namespace=""Microsoft.SharePoint.WebControls"" Assembly=""Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"" %>
<html xmlns:mso=""urn:schemas-microsoft-com:office:office"" xmlns:msdt=""uuid:C2F41010-65B3-11d1-A29F-00AA00C14882""><head>
<!--[if gte mso 9]><SharePoint:CTFieldRefs runat=server Prefix=""mso:"" FieldList=""FileLeafRef,Comments,PublishingStartDate,PublishingExpirationDate,PublishingContactEmail,PublishingContactName,PublishingContactPicture,PublishingPageLayout,PublishingVariationGroupID,PublishingVariationRelationshipLinkFieldID,PublishingRollupImage,Audience,PublishingIsFurlPage,PublishingPageImage,PublishingPageContent,SummaryLinks,SummaryLinks2,SeoBrowserTitle,SeoMetaDescription,SeoKeywords,RobotsNoIndex""><xml>
<mso:CustomDocumentProperties>
<mso:PublishingPageContent msdt:dt=""string""></mso:PublishingPageContent>
<mso:ContentType msdt:dt=""string"">Page d&#39;accueil</mso:ContentType>
<mso:PublishingPageLayout msdt:dt=""string"">/sites/somesite/_catalogs/masterpage/BlankWebPartPage.aspx, Page de composant WebPart vierge</mso:PublishingPageLayout>
<mso:RequiresRouting msdt:dt=""string"">False</mso:RequiresRouting>
<mso:PublishingIsFurlPage msdt:dt=""string"">1</mso:PublishingIsFurlPage>
<mso:SummaryLinks msdt:dt=""string"">&lt;div title=&quot;_schemaversion&quot; id=&quot;_3&quot;&gt;
  &lt;div title=&quot;_view&quot;&gt;
    &lt;span title=&quot;_columns&quot;&gt;1&lt;/span&gt;
    &lt;span title=&quot;_linkstyle&quot;&gt;&lt;/span&gt;
    &lt;span title=&quot;_groupstyle&quot;&gt;&lt;/span&gt;
  &lt;/div&gt;
&lt;/div&gt;</mso:SummaryLinks>
<mso:PublishingPageImage msdt:dt=""string""></mso:PublishingPageImage>
<mso:PublishingRollupImage msdt:dt=""string""></mso:PublishingRollupImage>
<mso:Audience msdt:dt=""string""></mso:Audience>
<mso:PublishingContactPicture msdt:dt=""string""></mso:PublishingContactPicture>
<mso:SummaryLinks2 msdt:dt=""string"">&lt;div title=&quot;_schemaversion&quot; id=&quot;_3&quot;&gt;
  &lt;div title=&quot;_view&quot;&gt;
    &lt;span title=&quot;_columns&quot;&gt;1&lt;/span&gt;
    &lt;span title=&quot;_linkstyle&quot;&gt;&lt;/span&gt;
    &lt;span title=&quot;_groupstyle&quot;&gt;&lt;/span&gt;
  &lt;/div&gt;
&lt;/div&gt;</mso:SummaryLinks2>
<mso:PublishingContactName msdt:dt=""string""></mso:PublishingContactName>
<mso:Comments msdt:dt=""string""></mso:Comments>
<mso:PublishingContactEmail msdt:dt=""string""></mso:PublishingContactEmail>
</mso:CustomDocumentProperties>
</xml></SharePoint:CTFieldRefs><![endif]-->
<title>Recherche globale</title></head>";

            var result = XmlPageDataHelper.ExtractProperties(sampleContent);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ContainsKey("PublishingPageLayout"));
            Assert.AreEqual(result["PublishingPageLayout"], "/sites/somesite/_catalogs/masterpage/BlankWebPartPage.aspx, Page de composant WebPart vierge");

        }
    }
}