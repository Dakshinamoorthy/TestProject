<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmbedDomainSearch.ascx.cs" Inherits="EmbedDomainSearch" %>
<%@ Register Src="~/Custom/UserControls/Parts/NcButton.ascx" TagName="NcButton" TagPrefix="namecheap" %>
<%@ Register Src="~/Custom/Components/Process/Domains/SingleDomainSearchBox.ascx" TagPrefix="namecheap" TagName="SingleDomainSearchBox" %>
<%@ Register Assembly="Namecheap.Web.Controls.Assets" Namespace="Namecheap.Web.Controls.Assets.Controls" TagPrefix="Namecheap" %>
<%@ Register Src="~/Custom/Components/DataView/DomainSearch/RelatedDomains.ascx" TagPrefix="namecheap" TagName="RelatedDomains" %>

<%if( IsReCaptchaVisible) { %>

<script src='https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit' async defer></script>

<%} %>

<%-- Start: Filtration New Domains  --%>
<namecheap:CssInclude ID="select2Css" runat="server" EnableViewState="false" CssBundle="None" UseSpecifiedContent="true" CssFileUrl="/assets/css/select2.css" RenderCssIncludesInline="false">
    <link rel="stylesheet" type="text/css" href="/assets/css/select2.css" />
</namecheap:CssInclude>
<namecheap:JsInclude id="select2Js" runat="server" EnableViewState="false" JsBundle="None" RenderJsScriptsInline="true" JsSrcUrl="/assets/js/select2.min.js">
	<script type="text/javascript" src="/assets/js/select2.min.js"></script>
</namecheap:JsInclude>
<namecheap:JsInclude id="JsInclude1" runat="server" EnableViewState="false" JsBundle="None" RenderJsScriptsInline="true" JsSrcUrl="/assets/js/tldFilter.js">
	<script type="text/javascript" src="/assets/js/tldFilter.js"></script>
</namecheap:JsInclude>
<namecheap:JsInclude id="JsInclude4" runat="server" EnableViewState="false" JsBundle="None" RenderJsScriptsInline="true" JsSrcUrl="/assets/js/bootstrap.affix.js">
	<script type="text/javascript" src="/assets/js/bootstrap.affix.js"></script>
</namecheap:JsInclude>
<%--End: Filtration New Domains  --%>
<namecheap:JsInclude id="JsInclude2" runat="server" EnableViewState="false" JsBundle="None" RenderJsScriptsInline="true" JsSrcUrl="/assets/js/cart.js">
	<script type="text/javascript" src="/assets/js/cart.js"></script>
</namecheap:JsInclude>
<namecheap:JsInclude id="JsInclude3" runat="server" EnableViewState="false" JsBundle="None" RenderJsScriptsInline="true" JsSrcUrl="/assets/js/DomainRelation.js">
	<script type="text/javascript" src="/assets/js/DomainRelation.js"></script>
</namecheap:JsInclude>

<asp:PlaceHolder ID="editMode" runat="server" Visible="false">
    <div style="background-color: #ccc; padding: 15px;">
        <strong>Domain Search by Ajax: In Edit Mode</strong>. Will activate during regular page view.
    </div>
</asp:PlaceHolder>
<Namecheap:InlineCss runat="server">
</Namecheap:InlineCss>

<asp:Panel runat="server" ID="recaptchaPanel" CssClass="recaptchaPanel" Visible="false">
    <fieldset>

        <div class="module">
            <header>Are you a human?</header>
            <ul class="captcha">
                <li>

                    <p>In order to make this service available to everyone, we need to protect it from abuse. That's why we ask you to solve this CAPTCHA. We'll do our best not to bother you with this anymore during this session. We apologize for the inconvenience.</p>
                    <p>If you have any issues, please <a href="#">contact our support</a> for assistance.</p>

                    <asp:HiddenField runat="server" ID="hfCaptchaResponse"/>
                    <div class="g-recaptcha" data-sitekey="<%=Captcha2SiteKey %>">
                        <span id ="reCaptcha" style="display: block; margin: 0 auto; width: 304px"></span>
                    </div>

                    <div class="captchaError" id="captchaStatusLiteral" runat="server" visible="false"></div>

                </li>
            </ul>
        </div>

    </fieldset>
</asp:Panel>

<asp:Literal ID="gaEventLogScript" runat="server" Visible="false">
<script type="text/javascript">
    $(document).ready(function () {
        LogGAEvent("{EVENT_CATEGORY}", "{EVENT_ACTION}", "{EVENT_LABEL}");

    });
</script>
</asp:Literal>

<%if ( ConfigurationManager.AppSettings["EnvironmentType"] ==null || ConfigurationManager.AppSettings["EnvironmentType"] == "sandbox" ) { %>
<link href="https://s3-us-west-2.amazonaws.com/namecheap-search/sandbox/search.css" rel="stylesheet" />
<div id="react-nc-search"></div>
<script type="text/javascript" src="/search.js"></script>
<% } %>	
<%else { %>
<link href="https://d1dijnkjnmzy2z.cloudfront.net/search.css" rel="stylesheet" />
<div id="react-nc-search"></div>
<script type="text/javascript" src="https://d1dijnkjnmzy2z.cloudfront.net/search.js"></script>
<% } %>

<div id="hidding-html" style="display:none">
  <Namecheap:CssInclude RenderCssIncludesInline="false" ID="AjaxAutoCompleteCssInclude" runat="server" Visible="false"></Namecheap:CssInclude>
  <Namecheap:JsInclude RenderJsScriptsInline="false" ID="AjaxAutoCompleteJsInclude" runat="server" Visible="false"></Namecheap:JsInclude>
  <div id="embedDomainSearchResults" runat="server" visible="false">
    <div id="categoryTabs" runat="server" visible="false">
      <asp:Repeater ID="categoryTabsRepeater" runat="server"></asp:Repeater>
    </div>
  </div>
  <div id="embedAjaxDomainSearchResults" runat="server" visible="false">
    <namecheap:NcButton ID="searchButton" runat="server" visible="false"
              ButtonType="Button" ButtonStyle="None" ButtonText="Search"
              Method="GET"
              ControlScope="FieldSet" GAeventcategory="Domain search" GAeventaction="Search" GAeventlabel="Single Search"/>
  </div>
  <input type="text" id="inputSingleDomain" runat="server" visible="false" class="handlereturn"
          autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false"
          placeholder="Search again" required
          onfocus="" />
</div>

<div class="date-time-configTldFilter-wrap">
    <asp:HiddenField runat="server" ID="newTldsHF" />
</div>

<script type ="text/javascript">

	// used when domain is premium but price is unknown
	function getEmergencyPremiumPrice(domainName) {

		var price = 0;

		$.ajax({
			dataType : 'json',
			url: '/domains/PremiumPrice.ashx?domainName=' + domainName,
			async: false
		}).done(function (data) {
			price = data;
		});

		return price;
	}

	function getEnvironmentType() {
		return '<%= ConfigurationManager.AppSettings["EnvironmentType"] ?? "sandbox"%>';
	}

</script>





<asp:Literal ID="scriptLiteral" runat="server" Visible="false">
<style type="text/css">
@media only screen and (max-device-width: 420px) {
    .sr-domain-result > div p {
        display: block;
    }
}
.sr-list li.premium div.strong a, .sr-list li.premium div.strong {
    color: inherit;
}
.sr-loading-domains {
    min-height: 126px;
}
</style>
</asp:Literal>

<!-- Tip Content -->
<ol id="joyRideTipContent">
    <li data-class="breadcrumbs" data-text="Start" class="custom" data-options="tipLocation:top;nubPosition:force-hide;tipAdjustmentY:20;tipAdjustmentX:50">
        <h2>Domain Search</h2>
        <p>Welcome to our new domain search! Click "Start" for a quick tour of the new features.</p>
    </li>
    <li data-class="sr-tabs" data-text="Next" class="custom" data-options="tipLocation:bottom;tipAdjustmentY:10;">
        <h2>Tabs</h2>
        <p>Simply drag and drop to re-order the tabs and set their priority. We'll remember your choices for your next visit.</p>
    </li>
    <li data-class="sr-tabs-content > div.selected div.sr-list a.favorite" data-button="Next" data-options="tipLocation:right;tipAnimation:fade;nubPosition:left;tipAdjustmentY:-18;tipAdjustmentX:10">
        <h2>Favorites</h2>
        <p>Heart your favorite domain extensions e.g .com, .net, .org. We'll store them under your "Favorites" tab and you can do a quick, tailored search for your domains there.</p>
    </li>
    <!--<li data-class="sr-tabs a[data-tab='favorites-tab']" data-text="Next" class="custom" data-options="tipLocation:bottom;tipAdjustmentY:10;">
        <h2>Favorites Tab</h2>
        <p>This tab conveniently shows all the previously favorited TLDs</p>
      </li>-->
    <li data-class="sr-tabs a[data-tab='new-tab']" data-text="Next" class="custom" data-options="tipLocation:bottom;tipAdjustmentY:10;">
        <h2>New</h2>
        <p>Contains results for new, recently released domain extensions. .e.g .email, .online etc.</p>
    </li>
    <li data-class="sr-tabs a[data-tab='international-tab']" data-text="Next" class="custom" data-options="tipLocation:bottom;tipAdjustmentY:10;">
        <h2>International</h2>
        <p>Contains results for country specific domain extensions e.g. .de, .co.uk , .fr etc.</p>
    </li>
    <!--<li data-class="sr-tabs-content div.selected .sr-list.results p.more a" data-button="Next" data-options="tipLocation:right;nubPosition:left;tipAdjustmentY:-18;tipAdjustmentX:10;">
        <h2>More Options</h2>
        <p>Use this to show more domain options</p>
      </li>-->
    <li data-class="sr-tabs-content div.selected .sr-list.domain-suggestions:eq(0) h2" data-button="Done" data-options="tipLocation:top;nubPosition:bottom;tipAdjustmentY:-10;">
        <h2>Domain Suggestions</h2>
        <p>Shows other available domains that are relevant to your current domain search.</p>
    </li>
    <!--
      <li data-class="sr-tabs-content div.selected .sr-list.domain-suggestions:eq(1) h2" data-button="Next" data-options="tipLocation:top;nubPosition:bottom;tipAdjustmentY:-10;">
        <h2>Premium Domains</h2>
        <p>If the domain you are searching for is listed for sale on our marketplace, you will see the list of such domains here. The high price is for initial purchase only, renewals come at regular price.</p>
      </li>
      <li data-class="sr-search-form a.btn-bulk-options" data-button="Done" data-options="tipLocation:bottom;tipAnimation:fade;nubPosition:top;tipAdjustmentY:5;">
        <h2>Bulk Search</h2>
        <p>If you want to search for multiple domains, clicking this to show the option</p>
      </li>-->
</ol>

<script>
     var onloadCallback = function () {
        grecaptcha.render('reCaptcha', {
            'sitekey': '<%=Captcha2SiteKey%>',
            'callback': function(response) {
                $('#<%=hfCaptchaResponse.ClientID%>').val(response);
                __doPostBack();
            }
        });
    };
</script>
