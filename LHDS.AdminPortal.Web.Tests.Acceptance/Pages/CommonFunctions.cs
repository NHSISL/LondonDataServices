// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Playwright;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Pages
{
    public class CommonFunctions
    {
        public class Login
        {
            private readonly IPage page;

            public Login(IPage page)
            {
                this.page = page;
            }

            public async Task PerformLogin(string username, string password)
            {
                var loginBtnClass = "#root > header > div > div.nhsuk-header__content > div > div > button";

                await page.Locator(loginBtnClass).ClickAsync();
                await page.TypeAsync("input[name='loginfmt']", username);
                await page.Locator("#idSIButton9").ClickAsync();
                await page.WaitForSelectorAsync("input[type='password']");
                await page.FocusAsync("input[type='password']");
                await page.TypeAsync("input[type='password']", password);
                await page.Locator("#idSIButton9").ClickAsync();
                await page.Locator("input[type='submit']").ClickAsync();
            }
        }
    }
}
