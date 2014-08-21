#region Directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestAutomationTemplate.Core;
using TestAutomationTemplate.ReportGeneration;

#endregion

namespace TestAutomationTemplate.SmokeTest
{
    /// <summary>
    /// Test Controller for the TestAutomationTemplate.Tests namespace
    /// </summary>
    [SetUpFixture]
    public class TestController : BaseClass
    {
        #region TestSetup

        private SmokeTestGenerateReport GenerateReport;

        /// <summary>
        /// Commons the setup.
        /// </summary>
        [SetUp]
        public void CommonSetup()
        {
            Directory.CreateDirectory(CustomConfiguration.TempDirectory);
            this.GenerateReport = new SmokeTestGenerateReport();
            this.GenerateReport.ReportHeader = "Automation Smoke Tests";

            if (CustomConfiguration.RecordVideo)
            {
                ScreenRecordingControls.StartVideoRecordingWithResolution();
                this.CleanDirectory();
            }

            this.GenerateReport.SetstartTime();
            this.OpenBrowser();
        }

        /// <summary>
        /// Commons the tear down.
        /// </summary>
        [TearDown]
        public void CommonTearDown()
        {
            Driver.Quit();
            if (CustomConfiguration.RecordVideo)
            {
                ScreenRecordingControls.EndRecording();
            }

            this.GenerateReport.SetStopTime();
            this.GenerateReport.SendEmail();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Opens the browser.
        /// </summary>
        public void OpenBrowser()
        {
            BaseClass.StartDriver(CustomConfiguration.BrowserType);
            Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
            Driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 0, CustomConfiguration.WaitForResponse));
            Driver.Navigate().GoToUrl(CustomConfiguration.BaseSiteUrl);
        }

        /// <summary>
        /// Cleans the directory.
        /// </summary>
        private void CleanDirectory()
        {
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(CustomConfiguration.TempDirectory);

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                BaseClass.LogEvent("Deleting file for cleanup - " + file.Name);
                file.Delete();
            }

            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                BaseClass.LogEvent("Deleting directory for cleanup - " + dir.Name);
                dir.Delete(true);
            }
        }

        #endregion
    }
}