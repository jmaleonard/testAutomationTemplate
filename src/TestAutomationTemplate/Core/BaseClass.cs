#region Directives

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using Selenium;
using TestAutomationTemplate.Enums;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// BaseClass to start all the drivers
    /// </summary>
    public class BaseClass : EventLogger
    {
        #region Fields

        private static string BaseUrl;
        private static WebBrowser WebBrowser = WebBrowser.None;
        private static ISelenium Selenium;
        private static IWebDriver __driver;

        #endregion

        #region Contructors
        /// <summary>
        /// Initializes static members of the BaseClass class.
        /// </summary>
        static BaseClass()
        {
            BaseUrl = CustomConfiguration.BaseSiteUrl;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the driver.
        /// </summary>
        /// <value>
        /// The driver.
        /// </value>
        public static IWebDriver Driver
        {
            get
            {
                return __driver;
            }

            set
            {
                __driver = value;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Navigates to a given URL
        /// </summary>
        /// <param name="url">The URL.</param>
        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Gets the browser name and version.
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserNameAndVersion()
        {
            string browsername = string.Empty;
            string version = string.Empty;

            if (Driver.GetType().ToString().Equals("TestAutomationTemplate.Core.ScreenShotRemoteWebDriver"))
            {
                browsername = ((ScreenShotRemoteWebDriver)Driver).Capabilities.BrowserName;
                browsername = char.ToUpper(browsername[0]) + browsername.Substring(1);
                version = ((ScreenShotRemoteWebDriver)Driver).Capabilities.Version;
            }
            else if (Driver.GetType().ToString().Equals("OpenQA.Selenium.PhantomJS.PhantomJSDriver"))
            {
                browsername = ((PhantomJSDriver)Driver).Capabilities.BrowserName;
                browsername = char.ToUpper(browsername[0]) + browsername.Substring(1);
                version = ((PhantomJSDriver)Driver).Capabilities.Version;
            }
            else if (Driver.GetType().ToString().Equals("OpenQA.Selenium.Remote.RemoteWebDriver"))
            {
                browsername = ((RemoteWebDriver)Driver).Capabilities.BrowserName;
                browsername = char.ToUpper(browsername[0]) + browsername.Substring(1);
                version = ((RemoteWebDriver)Driver).Capabilities.Version;
            }
            else if (Driver.GetType().ToString().Equals("OpenQA.Selenium.Chrome.ChromeDriver"))
            {
                browsername = ((ChromeDriver)Driver).Capabilities.BrowserName;
                browsername = char.ToUpper(browsername[0]) + browsername.Substring(1);
                version = ((ChromeDriver)Driver).Capabilities.Version;
            }
            else if (Driver.GetType().ToString().Equals("OpenQA.Selenium.Firefox.FirefoxDriver"))
            {
                browsername = ((FirefoxDriver)Driver).Capabilities.BrowserName;
                browsername = char.ToUpper(browsername[0]) + browsername.Substring(1);
                version = ((FirefoxDriver)Driver).Capabilities.Version;
            }
            else
            {
                browsername = ((InternetExplorerDriver)Driver).Capabilities.BrowserName;
                browsername = char.ToUpper(browsername[0]) + browsername.Substring(1);
                version = ((InternetExplorerDriver)Driver).Capabilities.Version;
            }

           return browsername + " " + version;
        }

        /// <summary>
        /// Starts a selected web browser
        /// </summary>
        /// <param name="webBrowser">The web browser.</param>
        /// <exception cref="OpenQA.Selenium.WebDriverException">No Web Browser selected</exception>
        public static void StartDriver(WebBrowser webBrowser)
        {
            WebBrowser = webBrowser;
            if (webBrowser == WebBrowser.Ie)
            {
                var capabilitiesInternet = new InternetExplorerOptions { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
                DesiredCapabilities capability = DesiredCapabilities.InternetExplorer();
                Uri remoteServerUrl = new Uri(CustomConfiguration.PathToSeleniumGridServer);
                Driver = new ScreenShotRemoteWebDriver(remoteServerUrl, capability);
            }
            else if (webBrowser == WebBrowser.DesktopIe)
            {
                Driver = new InternetExplorerDriver(CustomConfiguration.PathToWebDriverDrivers);
            }
            else if (webBrowser == WebBrowser.FireFox)
            {
                Driver = new FirefoxDriver();
            }
            else if (webBrowser == WebBrowser.Chrome)
            {   
                Driver = new ChromeDriver(CustomConfiguration.PathToWebDriverDrivers);
            }
            else
            {
                if (webBrowser == WebBrowser.PhantomJS)
                {
                    Driver = new PhantomJSDriver(CustomConfiguration.PathToWebDriverDrivers);
                }
                else
                {
                    throw new WebDriverException();
                }
            }

            Selenium = new WebDriverBackedSelenium(Driver, BaseUrl);
        }

        #endregion
    }
}
