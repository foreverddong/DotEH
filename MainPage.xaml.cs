#if ANDROID
using Android.Webkit;
#endif

using DotEH.Services;
using System.Net;

namespace DotEH;

public partial class MainPage : ContentPage
{

    private OptionsStorageService storage;
	public MainPage()
	{
		InitializeComponent();
        blazorWebView.BlazorWebViewInitialized += BlazorWebViewBlazorWebViewInitialized;
    }

    private void BlazorWebViewBlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        this.storage = this.Handler.MauiContext.Services.GetServices<OptionsStorageService>().First();
        var wv = e.WebView;
#if WINDOWS
		var membercookie = wv.CoreWebView2.CookieManager.CreateCookie("ipb_member_id", storage.EhMemberId, "ExHentai.org", "/");
		var passcookie = wv.CoreWebView2.CookieManager.CreateCookie("ipb_pass_hash", storage.EhPassHash, "ExHentai.org", "/");
		var igneouscookie = wv.CoreWebView2.CookieManager.CreateCookie("igneous", storage.EhIgneous, "ExHentai.org", "/");
        membercookie.SameSite = Microsoft.Web.WebView2.Core.CoreWebView2CookieSameSiteKind.None;
		membercookie.IsSecure = true;
        passcookie.SameSite = Microsoft.Web.WebView2.Core.CoreWebView2CookieSameSiteKind.None;
        passcookie.IsSecure = true;
        igneouscookie.SameSite = Microsoft.Web.WebView2.Core.CoreWebView2CookieSameSiteKind.None;
        igneouscookie.IsSecure = true;
        wv.CoreWebView2.CookieManager.AddOrUpdateCookie(membercookie);
        wv.CoreWebView2.CookieManager.AddOrUpdateCookie(passcookie);
        wv.CoreWebView2.CookieManager.AddOrUpdateCookie(igneouscookie);
#elif ANDROID
		var cm = CookieManager.Instance;
        cm.SetAcceptThirdPartyCookies(wv, true);
        cm.SetAcceptCookie(true);
        cm.SetCookie("https://exhentai.org", "abc=123;");
		cm.SetCookie("https://exhentai.org", $"ipb_member_id={storage.EhMemberId}; SameSite=None; Secure;");
        cm.SetCookie("https://exhentai.org", $"ipb_pass_hash={storage.EhPassHash}; SameSite=None; Secure;");
        cm.SetCookie("https://exhentai.org", $"igneous={storage.EhIgneous}; SameSite=None; Secure;") ;
#elif IOS
#endif
    }
}
