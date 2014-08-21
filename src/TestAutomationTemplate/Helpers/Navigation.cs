#region Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TestAutomationTemplate.Core;
using TestAutomationTemplate.ReportGeneration;

#endregion

namespace TestAutomationTemplate.Helpers
{
    /// <summary>
    /// Handles the navigation for page objects
    /// </summary>
   public class Navigation : BaseClass
    {
        #region Fields

       private SmokeTestGenerateReport _smokeTestGenerateReport = new SmokeTestGenerateReport();

       #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Navigation" /> class.
        /// </summary>
        /// <param name="BrowserWindowHandle">The browser window handle.</param>
        public Navigation(string BrowserWindowHandle)
        {
            this.BrowserWindowHandle = BrowserWindowHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Navigation" /> class.
        /// </summary>
        public Navigation()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the browser window handle.
        /// </summary>
        /// <value>
        /// The browser window handle.
        /// </value>
        public string BrowserWindowHandle { get; set; }

        #endregion

        #region ForceClosing Browsers 

        /// <summary>
        /// Closes driver the with error.
        /// </summary>
        public void CloseWithError()
        {
            try
            {
                foreach (Process IEInstance in Process.GetProcessesByName("iexplore"))
                {
                    IEInstance.Kill();
                }
            }
            catch (Exception e)
            {
                BaseClass.LogEvent(e.Message.ToString());
            }

            if (BaseClass.Driver != null)
            {
                Driver.Quit();
            }
        }

        #endregion

        #region Interaction Methods

        /// <summary>
        /// Gets a web Element.
        /// </summary>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="locator">The locator.</param>
        /// <returns></returns>
        public IWebElement GetWebElement(string locatorString, Enums.Locator locator)
        {
            switch (locator)
            {
                case Enums.Locator.ID:
                    {
                        return Driver.FindElement(By.Id(locatorString)); 
                    }

                case Enums.Locator.ClassName:
                    {
                        return Driver.FindElement(By.ClassName(locatorString)); 
                    }

                case Enums.Locator.LinkText:
                    {
                        return Driver.FindElement(By.LinkText(locatorString));
                    }

                case Enums.Locator.XPath:
                    {
                        return Driver.FindElement(By.XPath(locatorString));
                    }

                case Enums.Locator.TagName:
                    {
                        return Driver.FindElement(By.TagName(locatorString));
                    }
            }

            EventLogger.LogEvent("Please provide a locator");
            return (IWebElement)null;
        }

        /// <summary>
        /// Clicks the select element.
        /// </summary>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="locator">The locator.</param>
        public void ClickElement(string locatorString, Enums.Locator locator)
        {
            this.GetWebElement(locatorString, locator).Click();
        }

        /// <summary>
        /// Clicks the element and ignore exceptions. This method is used when we click export to excel, the page will timeout.
        /// </summary>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="locator">The locator.</param>
        public void ClickElementAndIgnoreExceptions(string locatorString, Enums.Locator locator)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 10));
                this.GetWebElement(locatorString, locator).Click();
            }
            catch (Exception e)
            {
                EventLogger.LogEvent(e.Message.ToString());
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
            }
        }

        /// <summary>
        /// Drops down list selection method.
        /// The selection parameter can either be the dropdown list value, text or Index
        /// The index needs be provided, which will be parsed as an int
        /// </summary>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="selectionType">Type of the selection.</param>
        /// <param name="selectionParameter">The selection parameter.</param>
        public void DropDownListSelection<T>(string locatorString, Enums.Locator locator, Enums.DropDownSelectionType selectionType, T selectionParameter)
        {
            SelectElement element = new SelectElement(this.GetWebElement(locatorString, locator));

            switch (selectionType)
            {
                case Enums.DropDownSelectionType.Value:
                    {
                        element.SelectByValue(selectionParameter.ToString());
                        break;
                    }

                case Enums.DropDownSelectionType.Text:
                    {
                        element.SelectByText(selectionParameter.ToString());
                        break;
                    }

                case Enums.DropDownSelectionType.Index:
                    {
                        element.SelectByIndex(int.Parse(selectionParameter.ToString()));
                        break;
                    }
            }
        }

        /// <summary>
        /// Finds the element.
        /// </summary>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="locator">The locator.</param>
        public void FindElement(string locatorString, Enums.Locator locator)
        {
            Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 10));
            this.GetWebElement(locatorString, locator);
        }

        /// <summary>
        /// Enter text to an Web Element
        /// </summary>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string locatorString, Enums.Locator locator, string text)
        {
            this.GetWebElement(locatorString, locator).SendKeys(text);
        }

        /// <summary>
        /// Retrieve selected Drop Down List value.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        /// <returns></returns>
        public string FindDropDownListSelectionTextById(string elementId)
        {
            IWebElement dropdownlist = Driver.FindElement(By.Id(elementId));
            SelectElement element = new SelectElement(dropdownlist);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until<IWebElement>((d) =>
            {
                return element.SelectedOption;
            });
            IWebElement selectedElement = element.SelectedOption;
            return selectedElement.Text.ToString();
        }

        /// <summary>
        /// Clicks the name of the element with in table by tag.
        /// </summary>
        /// <param name="TableClassName">Name of the table class.</param>
        /// <param name="tagName">Name of the tag.</param>
        public void ClickElementWithInTableClassByTagName(string TableClassName, string tagName)
        {
            Driver.FindElement(By.ClassName(TableClassName)).FindElement(By.TagName(tagName)).Click();
        }

        /// <summary>
        /// Clicks the name of the element within table id by tag.
        /// </summary>
        /// <param name="TableId">The table id.</param>
        /// <param name="tagName">Name of the tag.</param>
        public void ClickElementWithinTableIdByTagName(string TableId, string tagName)
        {
            Driver.FindElement(By.Id(TableId)).FindElement(By.TagName(tagName)).Click();
        }

        /// <summary>
        /// Clicks the element by partial link text.
        /// </summary>
        /// <param name="elementText">The element text.</param>
        public void ClickElementByPartialLinkText(string elementText)
        {
            Driver.FindElement(By.PartialLinkText(elementText.Substring(0, 6))).Click();
        }

        /// <summary>
        /// Doubles the click element by id.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        public void DoubleClickElementById(string elementId)
        {
            Actions action = new Actions(Driver);
            IWebElement current = Driver.FindElement(By.Id(elementId));
            action.MoveToElement(current).DoubleClick().Perform();
        }

        /// <summary>
        /// Checks the check box selection by id.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        public void CheckCheckBoxSelectionById(string elementId)
        {
            IWebElement checkbox = Driver.FindElement(By.Id(elementId));
            if (!checkbox.Selected)
            {
                checkbox.Click();
            }
        }

        /// <summary>
        /// Unchecks the check box selection by id.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        public void UncheckCheckBoxSelectionById(string elementId)
        {
            IWebElement checkbox = Driver.FindElement(By.Id(elementId));
            if (checkbox.Selected)
            {
                checkbox.Click();
            }
        }

        /// <summary>
        /// Selects the frame.
        /// </summary>
        /// <param name="frameId">The frame id.</param>
        public void SelectFrame(string frameId)
        {
            Driver.SwitchTo().Frame(frameId);
        }

        /// <summary>
        /// Selects the default frame.
        /// </summary>
        public void SelectDefaultFrame()
        {
            Driver.SwitchTo().DefaultContent();
        }

       #region XpathLookUp

        /// <summary>
        /// Finds the text using xpath look up.
        /// </summary>
        /// <param name="elementText">The element text.</param>
        public bool FindTextUsingXpathLookUp(string elementText)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 2));
                Driver.FindElement(By.XPath(@"//*[contains(text(),'" + elementText + @"')]"));
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

        /// <summary>
        /// Finds the element by alt attribute.
        /// </summary>
        /// <param name="attributeText">The attribute text.</param>
        public void FindElementByAltAttribute(string attributeText)
        {
            Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, 2));
            Driver.FindElement(By.XPath(@"//img[@alt ='" + attributeText + @"']"));
            Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
        }

        /// <summary>
        /// Clicks the element by alt attribute.
        /// </summary>
        /// <param name="attributeText">The attribute text.</param>
        public void ClickElementByAltAttribute(string attributeText)
        {   
            Driver.FindElement(By.XPath(@"//img[@alt ='" + attributeText + @"']")).Click();
        }
        #endregion

        #endregion

        #region PopupSwitching

        /// <summary>
        /// Gets the pop window handle.
        /// </summary>
        /// <param name="BrowserWindowHandle">The browser window handle.</param>
        /// <returns>The  Popup WindowHandle</returns>
        public string GetPopWindowHandle(string BrowserWindowHandle)
        {
            string foundHandle = null;
            IList<string> existingWindowHandles = new List<string>();
            existingWindowHandles.Add(BrowserWindowHandle);
            Wait.Pause(5);
            while (Driver.WindowHandles.Count < 1)
            {
                Thread.Sleep(200);
            }

            List<string> currentHandles = Driver.WindowHandles.ToList();
            List<string> differentHandles = currentHandles.Except(existingWindowHandles).ToList();

            if (differentHandles.Count > 0)
            {
                foundHandle = differentHandles[0];

                return foundHandle;
            }
            else
            {
                return foundHandle;
            }
        }

        /// <summary>
        /// Gets the second window handle.
        /// </summary>
        /// <param name="BrowserWindowHandle">The browser window handle.</param>
        /// <returns></returns>
        public string GetSecondWindowHandle(string BrowserWindowHandle)
        {
            string foundHandle = null;
            IList<string> existingWindowHandles = new List<string>();
            existingWindowHandles.Add(BrowserWindowHandle);
            Wait.Pause(5);

            while (Driver.WindowHandles.Count < 1)
            {
                Thread.Sleep(200);
            }

            List<string> currentHandles = Driver.WindowHandles.ToList();
            List<string> differentHandles = currentHandles.Except(existingWindowHandles).ToList();

            if (differentHandles.Count == 2)
            {
                foundHandle = differentHandles[1];

                return foundHandle;
            }
            else if (differentHandles.Count < 2)
            {
                foundHandle = differentHandles[0];

                return foundHandle;
            }
            else
            {
                return foundHandle;
            }
        }

        #endregion

        #region Download File IE

        /// <summary>
        /// Downloads the file internet explorer.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void DownloadFileInternetExplorer(string filePath)
        {
            string defaultAddin = " - Microsoft Internet Explorer provided by open box software";
            DownloadFile download = new DownloadFile();
            download.SaveFile(Driver.Title + defaultAddin, filePath);
        }

        #endregion

         #region Iterate Tabs

        /// <summary>
        /// Iterates the tabs.
        /// </summary>
        /// <param name="Tabs">The tabs.</param>
        /// <param name="ParentTab">The parent tab.</param>
        /// <param name="RootTab">The root tab.</param>
        public void IterateTabs(List<TabPage> Tabs, string ParentTab, string RootTab)
        {
            for (int x = 0; x < Tabs.Count(); x++)
            {
                if (Tabs.ElementAt(x) != null)
                {
                    TabPage cur = (TabPage)Tabs.ElementAt(x);
                    string testname = "Load " + cur.TabName + " Tab";
                    SmokeTestResult result = new SmokeTestResult(RootTab, testname, ParentTab, cur.TabName, Enums.TestStatus.Incompleted, string.Empty, DateTime.Now, DateTime.Now);

                    try
                    {
                        this.ClickElement(cur.LocatorString, cur.Locator);
                        this.FindTextUsingXpathLookUp(cur.PageElement);
                        result.TestStatus = Enums.TestStatus.Completed;
                        result.ErrorMessage = "None";
                        result.EndTime = DateTime.Now;
                        this._smokeTestGenerateReport.AddItem(result);
                    }
                    catch (Exception e)
                    {
                        e.Message.ToString();
                        result.TestStatus = Enums.TestStatus.Failed;
                        result.ErrorMessage = "Failed to load " + cur.TabName + " Tab";
                        result.EndTime = DateTime.Now;
                        this._smokeTestGenerateReport.AddItem(result);
                    }
                }
            }
        }
#endregion
    }
}
