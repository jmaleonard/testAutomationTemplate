#region Directives

using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using TestAutomationTemplate.Helpers;
using TestAutomationTemplate.ReportGeneration;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Test Result object to capture the state of each test
    /// </summary>
    public class TestResult
    {
        #region Fields
        private static GenerateReport _generateReport = new GenerateReport();
        private bool status;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResult" /> class.
        /// </summary>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="FeatureName">Name of the feature.</param>
        /// <param name="ScenarioName">Name of the scenario.</param>
        /// <param name="testStatus">The test status.</param>
        /// <param name="currentData">The current data.</param>
        public TestResult(string ApplicationName, string FeatureName, string ScenarioName, Enums.TestStatus testStatus, List<TestData> currentData)
        {
            this.FeatureName = FeatureName;
            this.ApplicationName = ApplicationName;
            this.ScenarioName = ScenarioName;
            this.TestParameters = DateTime.Now.ToLongTimeString();
            this.TestStatus = testStatus;
            this.ErrorMessage = string.Empty;
            this.StartTime = DateTime.Now;
            this.EndTime = DateTime.Now;
            this.ImagePath = string.Empty;
            currentData = new List<TestData>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResult" /> class.
        /// </summary>
        public TestResult()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the driver.
        /// </summary>
        /// <value>
        /// The driver.
        /// </value>
        private static GenerateReport GenerateReport
        {
            get
            {
                return _generateReport;
            }

            set
            {
                _generateReport = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the name of the feature.
        /// </summary>
        /// <value>
        /// The name of the feature.
        /// </value>
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the name of the scenario.
        /// </summary>
        /// <value>
        /// The name of the scenario.
        /// </value>
        public string ScenarioName { get; set; }

        /// <summary>
        /// Gets or sets the test parameters.
        /// </summary>
        /// <value>
        /// The test parameters.
        /// </value>
        public string TestParameters { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Enums.TestStatus TestStatus { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>
        /// The image path.
        /// </value>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        /// <value>
        /// The current data.
        /// </value>
        public List<TestData> CurrentData { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the result.
        /// </summary>
        private void AddResult()
        {
            if (this != null)
            {
                GenerateReport.AddItem(this);
            }
        }

        /// <summary>
        /// Adds all the testData for the current Test to a list
        /// </summary>
        /// <param name="Parameter">The parameter.</param>
        /// <param name="Value">The value.</param>
        public void AddTestData(string Parameter, string Value)
        {
            if (this.CurrentData == null)
            {
                this.CurrentData = new List<TestData>();
            }

            TestData cur = new TestData(Parameter, Value);
            this.CurrentData.Add(cur);
            cur = null;
        }

        /// <summary>
        /// Passed the test result.
        /// </summary>
        public void PassedTestResult()
        {
            this.EndTime = DateTime.Now;
            this.TestStatus = Enums.TestStatus.Completed;
            this.AddResult();
            this.SetStatusToTrue();
            Assert.IsTrue(this.status);
            TestAutomationTemplate.Core.BaseClass.Driver.Close();
        }

        /// <summary>
        /// Passed the test result.
        /// </summary>
        public void PassedTestResultWithoutClosing()
        {
            this.EndTime = DateTime.Now;
            this.TestStatus = Enums.TestStatus.Completed;
            this.AddResult();
            this.SetStatusToTrue();
            Assert.IsTrue(this.status);
        }

        /// <summary>
        /// Gets the image path.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private string GetImagePath(string parameters)
        {
            string format = "(yyyy-dd-MM)(HH-mm-ss)";
            string newImagePath = parameters.ToString().Replace(":", string.Empty).Replace("|", string.Empty).Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace(" ", string.Empty);
            return newImagePath + "_" + DateTime.Now.ToString(format) + ".jpg";
        }

        /// <summary>
        /// Gets the failed image path.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private string GetFailedImagePath(string parameters)
        {
            string format = "(yyyy-dd-MM)(HH-mm-ss)";
            string newImagePath = parameters.ToString().Replace(":", string.Empty).Replace("|", string.Empty).Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace(" ", string.Empty);
            return newImagePath + "_" + DateTime.Now.ToString(format) + "failed.jpg";
        }

        /// <summary>
        /// Failed the result.
        /// </summary>
        /// <param name="error">The error.</param>
        public void FailedResult(string error)
        {
            this.ImagePath = this.GetFailedImagePath(this.TestParameters);
            this.TakeFailedScreenShot(this.ImagePath);
            this.ErrorMessage = error;
            this.EndTime = DateTime.Now;
            this.TestStatus = Enums.TestStatus.Failed;
            this.AddResult();
            Navigation cur = new Navigation();
            cur.CloseWithError();
            this.SetStatusToFalse();
            Assert.IsTrue(this.status);
        }

        /// <summary>
        /// Takes the screen shot.
        /// </summary>
        public void TakeScreenShot()
        {
            Thread.Sleep(1000);
            ScreenShotImage screenshot = new ScreenShotImage();
            this.ImagePath = this.GetImagePath(this.TestParameters);
            Console.WriteLine("Taking screenshot...");
            screenshot.CaptureScreenShot(this.ImagePath);
            Console.WriteLine("Screenshot taken - " + this.ImagePath);
        }

        /// <summary>
        /// Takes the failed screen shot.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void TakeFailedScreenShot(string fileName)
        {
            Thread.Sleep(1000);
            ScreenShotImage screenshot = new ScreenShotImage();
            Console.WriteLine("Taking failure screenshot...");
            screenshot.CaptureScreenShot(fileName);
            Console.WriteLine("Failure screenshot taken - " + fileName);
        }

        /// <summary>
        /// Sets the status to false.
        /// </summary>
        private void SetStatusToFalse()
        {
            this.status = false;
        }

        /// <summary>
        /// Sets the status to true.
        /// </summary>
        private void SetStatusToTrue()
        {
            this.status = true;
        }

        #endregion

        #region TestData
        /// <summary>
        /// A list of the test parameters for each test run is stored here
        /// </summary>
        public class TestData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestData" /> class.
            /// </summary>
            /// <param name="Name">The name.</param>
            /// <param name="Value">The value.</param>
            public TestData(string Name, string Value)
            {
                this.Name = Name;
                this.Value = Value;
            }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public string Value { get; set; }
        }

        #endregion
    }
}
