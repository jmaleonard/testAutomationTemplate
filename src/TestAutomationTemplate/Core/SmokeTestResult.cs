#region Directives

using System;
using TestAutomationTemplate.ReportGeneration;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// This result is used when creating smoke tests
    /// </summary>
    public class SmokeTestResult
    {
         #region Fields

        /// <summary>
        /// Gets all the test results
        /// </summary>
        private SmokeTestGenerateReport smokeGenerateReport = new SmokeTestGenerateReport();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SmokeTestResult" /> class.
        /// </summary>
        /// <param name="RootTab">Name of the application.</param>
        /// <param name="TestName">Name of the test.</param>
        /// <param name="ParentTab">The parent tab.</param>
        /// <param name="ChildTab">The child tab.</param>
        /// <param name="testStatus">The test status.</param>
        /// <param name="ErrorMessage">The error message.</param>
        /// <param name="StartTime">The start time.</param>
        /// <param name="EndTime">The end time.</param>
        public SmokeTestResult(string RootTab, string TestName, string ParentTab, string ChildTab, Enums.TestStatus testStatus, string ErrorMessage, DateTime StartTime, DateTime EndTime)
        {
            this.TestName = TestName;
            this.RootTab = RootTab;
            this.ParentTab = ParentTab;
            this.ChildTab = ChildTab;
            this.TestStatus = testStatus;
            this.ErrorMessage = ErrorMessage;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string RootTab { get; set; }

        /// <summary>
        /// Gets or sets the parent tab.
        /// </summary>
        /// <value>
        /// The parent tab.
        /// </value>
        public string ParentTab { get; set; }

        /// <summary>
        /// Gets or sets the child tab.
        /// </summary>
        /// <value>
        /// The child tab.
        /// </value>
        public string ChildTab { get; set; }

        /// <summary>
        /// Gets or sets the name of the test.
        /// </summary>
        /// <value>
        /// The name of the test.
        /// </value>
        public string TestName { get; set; }

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

        #endregion

        #region Methods

        /// <summary>
        /// Passed the test.
        /// </summary>
        public void PassedTest()
        {
            this.TestStatus = Enums.TestStatus.Completed;
            this.ErrorMessage = "None";
            this.EndTime = DateTime.Now;
            this.smokeGenerateReport.AddItem(this);
        }

        /// <summary>
        /// Failed the test.
        /// </summary>
        /// <param name="error">The error.</param>
        public void FailedTest(string error)
        {
            this.TestStatus = Enums.TestStatus.Failed;
            this.ErrorMessage = error;
            this.EndTime = DateTime.Now;
            this.smokeGenerateReport.AddItem(this);
        }

        #endregion
    }
}
