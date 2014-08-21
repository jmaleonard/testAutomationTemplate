using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestAutomationTemplate.Core;
using TestAutomationTemplate.ReportGeneration;

namespace TestAutomationTemplate.Steps
{
    /// <summary>
    /// This class will act as a controller for all UI tests. This will execute once before each test
    /// </summary>
    [Binding]
    public class BeforeAndAfter
    {
        #region Fields

        /// <summary>
        /// CurrentList of Data
        /// </summary>
        private static List<TestResult.TestData> currentTestDataList = new List<TestResult.TestData>();

        /// <summary>
        /// Gets or sets the generate report.
        /// </summary>
        /// <value>
        /// The generate report.
        /// </value>
        private static GenerateReport _generateReport { get; set; }

        /// <summary>
        /// Gets or sets the generate report.
        /// </summary>
        /// <value>
        /// The generate report.
        /// </value>
        public static GenerateReport GenerateReport
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

        #endregion

        #region TestSetup

        /// <summary>
        /// Commons the setup.
        /// </summary>
        [BeforeTestRun]
        public static void CommonSetup()
        {
            Directory.CreateDirectory(CustomConfiguration.TempDirectory);
            CleanDirectory();
            GenerateReport = new GenerateReport();
            GenerateReport.ReportHeader = "Demo UI Tests";

            if (CustomConfiguration.RecordVideo)
            {
                ScreenRecordingControls.StartVideoRecordingWithResolution();
            }

            GenerateReport.SetstartTime();
        }

        /// <summary>
        /// Commons the tear down.
        /// </summary>
        [AfterTestRun]
        public static void CommonTearDown()
        {
            if (CustomConfiguration.RecordVideo)
            {
                ScreenRecordingControls.EndRecording();
            }

            GenerateReport.SetStopTime();
            GenerateReport.SendEmail();
            GenerateReport.SendSummaryReport();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Cleans the directory.
        /// </summary>
        private static void CleanDirectory()
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
