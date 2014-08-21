#region Directives

using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestAutomationTemplate.Core;

#endregion

namespace TestAutomationTemplate.Helpers
{
    /// <summary>
    /// Custom Wait for Web driver
    /// </summary>
    public class Wait : BaseClass
    {
        /// <summary>
        /// Waits the until element appears.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void WaitUntilElementIdAppears(string id)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(CustomConfiguration.WaitForResponse));
            IWebElement MyDynamicElement = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.Id(id));
            });
        }

        /// <summary>
        /// Waits the until element class name appears.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        public static void WaitUntilElementClassNameAppears(string className)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(CustomConfiguration.WaitForResponse));
            IWebElement MyDynamicElement = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.ClassName(className));
            });
        }

        /// <summary>
        /// Waits the until element appears.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="secondsToWait">The seconds to wait.</param>
        public static void WaitUntilElementAppears(string id, int secondsToWait)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(secondsToWait));
            IWebElement MyDynamicElement = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.Id(id));
            });
        }

        /// <summary>
        /// Waits the until element fill with content using xpath.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void WaitUntilElementFillWithContentUsingXpath(string id)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(CustomConfiguration.WaitForResponse));
            IWebElement MyDynamicElement = wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.Id(id));
            });
            bool foundContent = wait.Until<bool>((b) =>
            {
                return MyDynamicElement.Text.Length > 0;
            });
        }

        /// <summary>
        /// Waits the until popup window and close.
        /// </summary>
        /// <param name="windowId">The window id.</param>
        /// <param name="id">The id.</param>
        public static void WaitUntilPopupWindowAndClose(string windowId, string id)
        {
            foreach (string locationPopUpHandle in Driver.WindowHandles)
            {
                Driver.SwitchTo().Window(locationPopUpHandle);
                try
                {
                    Driver.FindElement(By.Id(id)).Click();
                    return;
                }
                catch (NoSuchElementException)
                {
                    EventLogger.LogEvent("Unable to find popup");
                }
            }
        }

        /// <summary>
        /// Waits the until for element present.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        public static void WaitUntilForElementPresent(string elementName)
        {
            DateTime datetime = DateTime.Now;

            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - datetime.Ticks);

            while (ts.TotalSeconds <= CustomConfiguration.WaitForResponse)
            {
                if (IsElementPresent(elementName, CustomConfiguration.WaitForResponse))
                {
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Pauses the specified seconds.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public static void Pause(int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Determines whether element present with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="secondsToWait">The seconds to wait.</param>
        /// <returns>
        ///   <c>true</c> if element is present with the specified id; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsElementPresent(string id, int secondsToWait)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(secondsToWait));
                IWebElement MyDynamicElement = wait.Until<IWebElement>((d) =>
                {
                    return d.FindElement(By.Id(id));
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is link present] [the specified table].
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="linkText">The link text.</param>
        /// <returns>
        ///   <c>true</c> if [is link present] [the specified table]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLinkPresent(string table, string linkText)
        {
            if (table == string.Empty)
            {
                try
                {
                    Driver.FindElement(By.LinkText(linkText));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    IWebElement Table = Driver.FindElement(By.Id(table));

                    Table.FindElement(By.LinkText(linkText));
                    return true;
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Determines whether [is web element tag present] [the specified tag name].
        /// </summary>
        /// <param name="Tagname">The tag name.</param>
        /// <returns>
        ///   <c>true</c> if [is web element tag present] [the specified tag name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWebElementTagPresent(string Tagname)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 2));
                Driver.FindElement(By.TagName(Tagname));
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
                return true;
            }
            catch (Exception e)
            {
                EventLogger.LogEvent(e.Message.ToString());
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
                return false;
            }
        }
    }
}
