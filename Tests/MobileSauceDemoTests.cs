using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace BrowserStackMobileTests.Tests;

/// <summary>
/// Standard Playwright/NUnit tests — no BrowserStack-specific code required.
/// The BrowserStack SDK (BrowserStack.TestAdapter) intercepts test execution
/// and runs each test against every device/browser combination listed in
/// browserstack.yml. Locally, without the SDK's setup step configured, these
/// would just run in a local Chromium instance instead.
///
/// Target site: saucedemo.com — a public demo store built for test
/// automation practice, with a responsive layout and a mobile hamburger menu.
/// </summary>
[TestFixture]
public class MobileSauceDemoTests : PageTest
{
    [SetUp]
    public async Task GotoSite()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");
    }

    [Test]
    public async Task LoginPage_RendersCorrectly_OnMobile()
    {
        await Expect(Page.Locator("#user-name")).ToBeVisibleAsync();
        await Expect(Page.Locator("#password")).ToBeVisibleAsync();
        await Expect(Page.Locator("#login-button")).ToBeVisibleAsync();
    }

    [Test]
    public async Task StandardUser_CanLogin_OnMobile()
    {
        await Page.FillAsync("#user-name", "standard_user");
        await Page.FillAsync("#password", "secret_sauce");
        await Page.ClickAsync("#login-button");

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        await Expect(Page.Locator(".title")).ToHaveTextAsync("Products");
    }

    [Test]
    public async Task HamburgerMenu_OpensAndShowsLogout_OnMobile()
    {
        await Page.FillAsync("#user-name", "standard_user");
        await Page.FillAsync("#password", "secret_sauce");
        await Page.ClickAsync("#login-button");

        await Page.ClickAsync("#react-burger-menu-btn");

        await Expect(Page.Locator("#logout_sidebar_link")).ToBeVisibleAsync();
    }
}
