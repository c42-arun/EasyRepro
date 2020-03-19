// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using OpenQA.Selenium;

namespace Microsoft.Dynamics365.UIAutomation.Sample.Web
{
    [TestClass]
    public class CreateAccount
    {

        private readonly SecureString _username = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername"].ToSecureString();
        private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());

        [TestMethod]
        public void WEBTestCreateNewAccount()
        {
            using (var xrmBrowser = new Api.Browser(TestSettings.Options))
            {
                xrmBrowser.LoginPage.Login(_xrmUri, _username, _password);
                xrmBrowser.GuidedHelp.CloseGuidedHelp();
                xrmBrowser.Dialogs.CloseWarningDialog();
                
                xrmBrowser.ThinkTime(100);
                xrmBrowser.Navigation.OpenSubArea("Sales", "Accounts");

                xrmBrowser.ThinkTime(2000);
                xrmBrowser.Grid.SwitchView("Active Accounts");

                xrmBrowser.ThinkTime(1000);
                xrmBrowser.CommandBar.ClickCommand("New");

                xrmBrowser.ThinkTime(5000);
                xrmBrowser.Entity.SetValue("name", "Test API Account");
                xrmBrowser.Entity.SetValue("telephone1", "555-555-5555");
                xrmBrowser.Entity.SetValue("websiteurl", "https://easyrepro.crm.dynamics.com");

                xrmBrowser.CommandBar.ClickCommand("Save & Close");
                xrmBrowser.ThinkTime(2000);
            }
        }

        private void RedirectAction(LoginRedirectEventArgs obj)
        {
            var d = obj.Driver;

            //if (d.IsVisible(By.Id("i0116")))
            //{
            //    d.FindElement(By.Id("i0116")).SendKeys(obj.Username.ToUnsecureString());
            //}

            //if (d.IsVisible(By.Id("i0118")))
            //{
            //    d.FindElement(By.Id("i0118")).SendKeys(obj.Password.ToUnsecureString());
            //}

            // Wait for the "StaySignedIn"-Page and disagree
            d.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])
                , new TimeSpan(0, 0, 60),
                $"Could not find element {Reference.Login.StaySignedIn}"
                );

            if (d.IsVisible(By.Id("idBtn_Back")))
            {
                d.FindElement(By.Id("idBtn_Back")).Click(true);
            }

            //Wait for CRM Page to load
            d.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.CrmMainPage])
                , new TimeSpan(0, 0, 60),
                f => throw new Exception("Login page failed."));
        }
    }
}
