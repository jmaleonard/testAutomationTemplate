using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestAutomationTemplate.Core;
using TestAutomationTemplate.Helpers;
using TestAutomationTemplate.PageObjects;

namespace TestAutomationTemplate.Steps
{
    /// <summary>
    /// Google Search Steps
    /// Add [Binding] to the top of the class this is like adding TestClass
    /// </summary>
   [Binding]
   public class GoogleSearchSteps
   {
       #region Fields

        private TestResult _currentResult;
        private List<TestResult.TestData> currentTestDataList = new List<TestResult.TestData>();
        private GooglePageObject _googlePageObject;

       #endregion

       #region Steps

        /// <summary>
        /// Givens the i am using the follwing.
        /// </summary>
        /// <param name="Device">The device.</param>
        [Given(@"I am using the follwing '(.*)'")]
        public void GivenIAmUsingTheFollwing(Enums.MobileDevice Device)
        {
            this.InitializeNewTestResult(Device.ToString());

            // See the BeforeAndAfter.cs class as this will execute all code before touching any steps. 
            
            //// Start the driver with the browser type being specified in the app.config
            BaseClass.StartDriver(Enums.WebBrowser.Chrome);
            //// Setting the time we want the browser to wait before we fail the tests
            BaseClass.Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
            BaseClass.Driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
            //// navigate to the website
            BaseClass.Driver.Navigate().GoToUrl(CustomConfiguration.BaseSiteUrl);
            //// Resize the browser to the mobile size we wish to use
            //BaseClass.Driver.Manage().Window.Size = MobileDeviceSizes.Select(Device);
        }

        /// <summary>
        /// Givens the i have searched for.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        [Given(@"I have searched for '(.*)'")]
        public void GivenIHaveSearchedFor(string searchTerm)
        {
            this._googlePageObject = new GooglePageObject();

            //// Add the test data so that it will be in the report 
            this._currentResult.AddTestData("Term Searched", searchTerm);

            Helpers.Steps.Invoke(new Action<string>(this._googlePageObject.GoogleSearchFor), this._currentResult, searchTerm);
        }

        /// <summary>
        /// Thens the search results should be returned.
        /// </summary>
        [Then(@"search results should be returned")]
        public void ThenSearchResultsShouldBeReturned()
        {
            //// Here we check that we are able to find the results div
            //// if the method from the page object return false we fail the test and pass in a test result object
            
            Helpers.Steps.Invoke(new Action(this._googlePageObject.SearchResultDivIsPresent), this._currentResult);
            this._currentResult.TakeScreenShot();
            this._currentResult.PassedTestResult();
            //// this passes the test and takes a screenshot
        }

        #endregion

       #region Private Methods

        /// <summary>
        /// Initializes the new test result.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        private void InitializeNewTestResult(string deviceName)
        {
            this._currentResult = new TestResult(
                                                "Google Search",
                                                FeatureContext.Current.FeatureInfo.Title,
                                                ScenarioContext.Current.ScenarioInfo.Title,
                                                Enums.TestStatus.Incompleted,
                                                this.currentTestDataList);
        }

        #endregion
   }
}
