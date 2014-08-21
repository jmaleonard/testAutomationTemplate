#region Directives

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using TestAutomationTemplate.Core;

#endregion 

namespace TestAutomationTemplate.ReportGeneration
{
    /// <summary>
    /// This class generates the HTML report for the test results
    /// </summary>
    public class GenerateReport
    {
        #region Fields
        private static List<TestResult> _ListResults = new List<TestResult>();
        private string emailBody;
        private StringBuilder emailError = null;
        private List<string> emaillist = new List<string>();
        private static DateTime timeStart;
        private static DateTime timeStop;
        private int testDataRowCount = 0;
        private int testParameterRowCount = 0;
        private ReportJavaScript _reportJavaScript = new ReportJavaScript();
        private string reportHeader = string.Empty;
        #endregion

        #region Results to Email

        #region Properties

        /// <summary>
        /// Gets or sets the list results.
        /// </summary>
        /// <value>
        /// The list results.
        /// </value>
        public static List<TestResult> ListResults
        {
            get
            {
                return _ListResults;
            }

            set
            {
                _ListResults = value;
            }
        }

        /// <summary>
        /// Gets or sets the _ report header.
        /// </summary>
        /// <value>
        /// The _ report header.
        /// </value>
        public string ReportHeader
        {
            get
            {
                return this.reportHeader;
            }

            set
            {
                this.reportHeader = value;
            }
        }
        #endregion

        /// <summary>
        /// Writes the results to email.
        /// </summary>
        /// <returns></returns>
        private string WriteResultsToEmail()
        {
            if (ListResults.Count == 0)
            {
                return this.emailBody = this.NoResults();
            }
            else
            {
                return this.emailBody = this.PrintResults();
           } 
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="currentResult">The current result.</param>
        public void AddItem(TestResult currentResult)
        {
            if (currentResult != null)
            {
                ListResults.Add(currentResult);
            }
        }
        #endregion

        #region SendEmail

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <returns></returns>
        public bool SendEmail()
        {
            this.WriteResultsToEmail();
            //MailMessage message = new MailMessage();

            //message.Sender = message.From;
            //message.From = new MailAddress("", "");
            //message.Sender = new MailAddress("", "");
            //message.Subject = this.ReportHeader + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            //message.Body = this.WriteResultsToEmail();
            //message.IsBodyHtml = true;
            //foreach (string email in this.GetEmailAddresses())
            //{
            //    message.To.Add(email);

            //    try
            //    {
            //        SmtpClient smtp = new SmtpClient("");
            //        smtp.Credentials = new NetworkCredential("", "");
            //        smtp.Send(message);
            //        message.To.Clear();
            //    }
            //    catch (Exception ex)
            //    {
            //        this.emailError.Append(ex.Message);
            //        return false;
            //    }
            //}

            this.emailBody = string.Empty;
            return true;
        }

        /// <summary>
        /// Gets the email addresses.
        /// </summary>
        /// <returns></returns>
        private List<string> GetEmailAddresses()
        {
            string emaillist = ConfigurationManager.AppSettings["EmailRecipients"].ToString();
            List<string> emails = new List<string>(emaillist.Split(','));
            return emails;
        }
        #endregion

        #region PrintResults

        /// <summary>
        /// Noes the results.
        /// </summary>
        /// <returns></returns>
        private string NoResults()
        {
            HtmlTable NoResultsTable = new HtmlTable();
            NoResultsTable.EnableTheming = true;
            NoResultsTable.Border = 1;
            NoResultsTable.BorderColor = "#000000";
            NoResultsTable.Width = "100%";
            NoResultsTable.CellSpacing = 0;

            HtmlTableRow NoResultsTableName = new HtmlTableRow();
            HtmlTableCell NoResultsTableNameCell = new HtmlTableCell("th");
            NoResultsTableNameCell.ColSpan = 4;
            NoResultsTableName.BorderColor = "#FFFFFF";
            NoResultsTableNameCell.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:9pt;color:#FFFFFF;";
            NoResultsTableNameCell.BgColor = "#808080";
            NoResultsTableNameCell.Align = "left";
            NoResultsTableNameCell.InnerText = "No test results available";
            NoResultsTableName.BgColor = "#808080";

            NoResultsTableName.Cells.Add(NoResultsTableNameCell);
            NoResultsTable.Rows.Add(NoResultsTableName);

            StringBuilder NoResultsStringBuilder = new StringBuilder();
            NoResultsTable.RenderControl(new HtmlTextWriter(new StringWriter(NoResultsStringBuilder)));
            this.emailBody = NoResultsStringBuilder.ToString();
            NoResultsStringBuilder.Clear();
            NoResultsTable.Dispose();

            return this.emailBody.ToString();
        }

        /// <summary>
        /// Prints the results.
        /// </summary>
        /// <returns></returns>
        private string PrintResults()
        {
            HtmlTable table = new HtmlTable();
            table.EnableTheming = true;
            table.Border = 1;
            table.BorderColor = "#000000";
            table.Width = "100%";
            table.CellSpacing = 0;

            foreach (var featureName in ListResults.GroupBy(item => item.FeatureName))
            {
                HtmlTableRow tableName = new HtmlTableRow();
                HtmlTableCell tableNameCell = new HtmlTableCell("th");
                tableNameCell.ColSpan = 4;
                tableName.BorderColor = "#FFFFFF";
                tableNameCell.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:13pt;color:#FFFFFF;";
                tableNameCell.BgColor = "#808080";
                tableNameCell.Align = "left";
                tableNameCell.InnerText = "Feature Name: " + featureName.Key;
                tableName.BgColor = "#808080";

                tableName.Cells.Add(tableNameCell);
                table.Rows.Add(tableName);

                foreach (var testScenario in featureName.GroupBy(item => item.ScenarioName))
                {
                    HtmlTableRow appheaderRow = new HtmlTableRow();
                    HtmlTableCell header = new HtmlTableCell("th");
                    header.ColSpan = 2;
                    appheaderRow.BgColor = "#808080";
                    appheaderRow.BorderColor = "#FFFFFF";
                    header.BgColor = "#808080";
                    header.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;color:#FFFFFF;";
                    header.Align = "Left";
                    header.InnerText = "Test Scenario: " + testScenario.Key;
                    appheaderRow.Cells.Add(header);

                    header = new HtmlTableCell("th");
                    header.ColSpan = 1;
                    appheaderRow.BgColor = "#808080";
                    appheaderRow.BorderColor = "#FFFFFF";
                    header.BgColor = "#808080";
                    header.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;color:#FFFFFF;";
                    header.Align = "Left";
                    header.InnerText = "Test Duration";
                    appheaderRow.Cells.Add(header);

                    header = new HtmlTableCell("th");
                    header.ColSpan = 2;
                    appheaderRow.BgColor = "#808080";
                    appheaderRow.BorderColor = "#FFFFFF";
                    header.BgColor = "#808080";
                    header.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;color:#FFFFFF;";
                    header.Align = "Left";
                    header.InnerText = "Status";
                    appheaderRow.Cells.Add(header);
                    table.Rows.Add(appheaderRow);
                    HtmlTableRow tableHeadings = new HtmlTableRow();

                    appheaderRow.Cells.Add(header);
                    table.Rows.Add(appheaderRow);
                    int x = 1;
                    foreach (var testParameters in testScenario.GroupBy(item => item.TestParameters))
                    {
                        HtmlTableRow testparamets = new HtmlTableRow();
                        testparamets.ID = this.testParameterRowCount.ToString();
                        testparamets.Attributes.Add("onclick", "rowsHideShow(this);");
                        HtmlTableCell testparametersName = new HtmlTableCell();
                        testparametersName.ColSpan = 2;

                        testparametersName.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:10pt;padding:5px 5px 5px 5px; cursor: pointer;";
                        if (x % 2 == 0)
                        {
                            testparamets.BgColor = "#FFFFFF";
                        }
                        else
                        {
                            testparamets.BgColor = "#F0F0F0";
                        }

                        testparametersName.Align = "Left";
                        testparametersName.InnerText = "Test Run:" + x++;
                        testparamets.Cells.Add(testparametersName);

                        foreach (TestResult result in testParameters)
                        {
                            HtmlTableCell cell;
                            cell = new HtmlTableCell();
                            cell.ColSpan = 1;
                            cell.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:10pt;padding:5px 5px 5px 5px;";
                            cell.Align = "Left";
                            cell.InnerText = this.GetTotalTimeResult(result.StartTime, result.EndTime);
                            testparamets.Cells.Add(cell);

                            if (x % 2 == 0)
                            {
                                testparamets.BgColor = "#FFFFFF";
                            }
                            else
                            {
                                testparamets.BgColor = "#F0F0F0";
                            }

                            cell = new HtmlTableCell();
                            cell.ColSpan = 1;
                            if (result.TestStatus == Enums.TestStatus.Failed)
                            {
                                if (result.ErrorMessage.Equals("Failed to execute tests due to insufficient GACS access."))
                                {
                                    cell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;font-weight:bold;";
                                    cell.InnerText = result.ErrorMessage;
                                }
                                else
                                {
                                    cell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;font-weight:bold;color:#FFFFFF;vertical-align:middle;position:relative;";
                                    cell.InnerHtml = "<a href =" + result.ImagePath + "> " + result.ErrorMessage + "</a> &nbsp;" + @"<div style=""background:#ac2925; width: 20px; height:20px; border-radius:50%;display:inline-block;position:absolute;bottom:10%""></div>";
                                }
                            }
                            else if (result.TestStatus == Enums.TestStatus.Completed)
                            {
                                cell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;font-weight:bold;color:#FFFFFF;font-family:Verdana;font-size:9pt; vertical-align:middle;position:relative;";
                                cell.InnerHtml = "<a href =" + testParameters.First().ImagePath.ToString() + "> " + result.TestStatus.ToString() + "</a> &nbsp;" + @"<div style=""background:#397439; width: 20px; height:20px; border-radius:50%;display:inline-block;position:absolute;bottom:10%""></div>"; 
                            }

                            testparamets.Cells.Add(cell);

                            table.Rows.Add(testparamets);
                            this.testParameterRowCount++;
                            if (result.CurrentData != null)
                            {
                                HtmlTableRow testDataRow = new HtmlTableRow();
                                testDataRow.ID = this.testParameterRowCount.ToString();
                                testDataRow.Style.Add(HtmlTextWriterStyle.Display, "none");
                                testDataRow.Style.Add(HtmlTextWriterStyle.Width, "100%");

                                HtmlTableCell testDataCell = new HtmlTableCell();
                                testDataCell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;";
                                testDataCell.ColSpan = 4;
                                testDataCell.Width = "100%";

                                HtmlTable testDataTable = new HtmlTable();
                                testDataTable.Width = "100%";
                                testDataTable.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;";

                                foreach (TestAutomationTemplate.Core.TestResult.TestData td in result.CurrentData)
                                {
                                    HtmlTableRow testDataTableRow = new HtmlTableRow();
                                    testDataTableRow.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;";
                                    if (x % 2 == 0)
                                    {
                                        testDataTable.BgColor = "#FFFFFF";
                                    }
                                    else
                                    {
                                        testDataTable.BgColor = "#F0F0F0";
                                    }

                                    HtmlTableCell testDataTableCell = new HtmlTableCell();
                                    testDataTableCell.InnerHtml = "<li><b>" + td.Name + "</b>: " + td.Value + "</li>";
                                    testDataTableCell.ColSpan = 4;
                                    testDataTableCell.Width = "100%";
                                    testDataTableCell.Attributes.CssStyle.Value = "border-bottom:None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:8pt;padding:5px 5px 5px 5px;text-indent:50px;";
                                    testDataTableRow.Cells.Add(testDataTableCell);
                                    testDataTable.Rows.Add(testDataTableRow);
                                }

                                StringBuilder innerTable = new StringBuilder();
                                testDataTable.RenderControl(new HtmlTextWriter(new StringWriter(innerTable)));
                                testDataCell.InnerHtml = innerTable.ToString();
                                testDataRow.Cells.Add(testDataCell);
                                table.Rows.Add(testDataRow);
                                this.testParameterRowCount++;
                            }
                            else
                            {
                                HtmlTableRow testDataRow = new HtmlTableRow();
                                testDataRow.ID = this.testParameterRowCount.ToString();
                                testDataRow.Style.Add(HtmlTextWriterStyle.Display, "none");
                                testDataRow.Style.Add(HtmlTextWriterStyle.Width, "100%");

                                HtmlTableCell testDataCell = new HtmlTableCell();
                                testDataCell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;font-family:Verdana; font-size:8pt;";
                                testDataCell.ColSpan = 4;
                                testDataCell.Width = "100%";
                                testDataCell.InnerHtml = "<b>Test Parameters not available</b>";
                                testDataRow.Cells.Add(testDataCell);
                                table.Rows.Add(testDataRow);
                                this.testParameterRowCount++;
                            }
                        }
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            table.RenderControl(new HtmlTextWriter(new StringWriter(stringBuilder)));
            this.emailBody = this._reportJavaScript.WriteHTMLHeaderInformation().ToString() + this.PrintOverallStats().ToString() + stringBuilder.ToString() + this._reportJavaScript.GenerateJavaScriptCode(this.testDataRowCount).ToString();
            string reportDirectory = CustomConfiguration.TempDirectory;
            File.WriteAllText(reportDirectory + "Results.html", this.emailBody);
            this.CopyImagestoReportFolder(reportDirectory);
            stringBuilder.Clear();
            table.Dispose();

            return this.emailBody.ToString();
        }
        #endregion

        #region OverallStats

        /// <summary>
        /// Prints the overall stats.
        /// </summary>
        /// <returns></returns>
        public StringBuilder PrintOverallStats()
        {
            HtmlTable table = new HtmlTable();
            table.BorderColor = "#FFFFFF";
            table.EnableTheming = true;
            table.Border = 0;
            table.BorderColor = "#000000";
            table.Width = "100%";
            table.CellSpacing = 0;

            HtmlTableRow appheaderRow = new HtmlTableRow();
            HtmlTableCell header = new HtmlTableCell("th");
            header.ColSpan = 1;
            appheaderRow.BgColor = "#FFFFFF";
            appheaderRow.BorderColor = "#FFFFFF";
            header.BgColor = "#FFFFFF";
            header.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:8pt;color:#FFFFFF; word-spacing:normal";
            header.Align = "Left";
            header.InnerHtml = "<br>";
            appheaderRow.Cells.Add(header);
            table.Rows.Add(appheaderRow);

            appheaderRow = new HtmlTableRow();
            header = new HtmlTableCell("th");
            header.ColSpan = 1;
            appheaderRow.BgColor = "#FFFFFF";
            appheaderRow.BorderColor = "#FFFFFF";
            header.BgColor = "#FFFFFF";
            header.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:13pt;color:#666; word-spacing:normal";
            header.Align = "Left";
            header.InnerText = this.reportHeader;
            appheaderRow.Cells.Add(header);
            table.Rows.Add(appheaderRow);

            HtmlTableRow tableHeadings = new HtmlTableRow();
            HtmlTableCell headingCell;
            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();
            tableHeadings.BgColor = "#FFFFFF";
            headingCell.InnerHtml = "<br>";
            headingCell.Attributes.CssStyle.Value = "border-bottom: None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:5pt;font-weight:bold;padding:5px 5px 5px 5px;color:#FFFFFF;";
            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();

            headingCell.InnerHtml = @"<span style=""color:#666""><b> Browser: </b></span>" + BaseClass.GetBrowserNameAndVersion();
            tableHeadings.BgColor = "#FFFFFF";
            headingCell.ColSpan = 1;
            headingCell.Attributes.CssStyle.Value = "border-bottom:None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:10pt;padding:5px 5px 5px 5px;";
            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();
            headingCell.InnerHtml = @"<span style=""color:#666""><b>No. of Test(s): </b></span>" + this.NoTests();
            tableHeadings.BgColor = "#FFFFFF";
            headingCell.ColSpan = 1;
            headingCell.Attributes.CssStyle.Value = "border-bottom:None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:10pt;padding:5px 5px 5px 5px;";
            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();
            if (this.NoTestsCompleted() == 0)
            {
                headingCell.BgColor = "#FFFFFF";
                headingCell.InnerHtml = @"<b><span style=""color:#666""> No. of Passed Test(s)</span></b>: None";
                headingCell.Attributes.CssStyle.Value = "border-bottom: None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:10pt;padding:5px 5px 5px 5px;position:relative;";
            }
            else
            {
                headingCell.InnerHtml = @"<span style=""color:#666""><b>No. of Passed Test(s): </b></span>" + this.NoTestsCompleted().ToString() + " of " + this.NoTests().ToString() + "&nbsp;&nbsp;" + @"<div style=""background:#397439; width: 20px; height:20px; border-radius:50%;display:inline-block;position:absolute;bottom:10%""></div>";
                headingCell.Attributes.CssStyle.Value = "border-bottom: None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:10pt;padding:5px 5px 5px 5px;position:relative;";
            }

            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();
            if (this.NoTestsFailed() == 0)
            {
                headingCell.BgColor = "#FFFFFF";
                headingCell.InnerHtml = @"<span style=""color:#666""><b>No. of Failed Test(s):</b></span> None";
                headingCell.Attributes.CssStyle.Value = "border-bottom: None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:10pt;padding:5px 5px 5px 5px;position:relative;";
            }
            else
            {
                headingCell.InnerHtml = @"<span style=""color:#666""><b>No. of Failed Test(s): </b></span>" + this.NoTestsFailed().ToString() + " of " + this.NoTests().ToString() + " &nbsp;&nbsp;" + @"<div style=""background:#ac2925; width: 20px; height:20px; border-radius:50%;display:inline-block;position:absolute;bottom:10%""></div>";
                headingCell.Attributes.CssStyle.Value = "border-bottom: None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:10pt;padding:5px 5px 5px 5px;position:relative;";
            }

            tableHeadings.Cells.Add(headingCell);

            table.Rows.Add(tableHeadings);

            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();
            tableHeadings.BgColor = "#FFFFFF";
            headingCell.InnerHtml = @"<span style=""color:#666""><b>Total Time: </b></span>" + this.GetTotalTime(timeStop, timeStart);
            headingCell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:10pt;padding:5px 5px 5px 5px;";
            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            if (CustomConfiguration.RecordVideo)
            {
                tableHeadings = new HtmlTableRow();
                headingCell = new HtmlTableCell();
                tableHeadings.BgColor = "#FFFFFF";
                headingCell.InnerHtml = @"<span style=""color:#666""><b>Full Test Run: </b></span><a href =""TestReportVideo.mp4"">Video</a> (To View Video Right-Click Save As)";
                headingCell.Attributes.CssStyle.Value = "border-bottom:None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:10pt;padding:5px 5px 5px 5px;";
                tableHeadings.Cells.Add(headingCell);
                table.Rows.Add(tableHeadings);
            }

            tableHeadings = new HtmlTableRow();
            headingCell = new HtmlTableCell();
            tableHeadings.BgColor = "#FFFFFF";
            headingCell.InnerHtml = "<br>";
            headingCell.Attributes.CssStyle.Value = "border-bottom: None; border-right:None; border-top:None; border-left:None;font-family:Verdana;font-size:3pt;padding:5px 5px 5px 5px;color:#FFFFFF;";
            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            StringBuilder stringBuilder = new StringBuilder();
            table.RenderControl(new HtmlTextWriter(new StringWriter(stringBuilder)));

            StringBuilder stats = new StringBuilder();

            stats = stringBuilder;

            return stats;
        }

        #region Methods

        /// <summary>
        /// Copies the images to report folder.
        /// </summary>
        /// <param name="reportsDirectory">The reports directory.</param>
        private void CopyImagestoReportFolder(string reportsDirectory)
        {
            Assembly assemb = Assembly.GetExecutingAssembly();
            string path = assemb.ManifestModule.ScopeName.Replace(".dll", string.Empty).Trim();
            var image1 = Image.FromStream(assemb.GetManifestResourceStream(path + ".Images.img_TopBackground.jpg"));
            image1.Save(reportsDirectory + "img_TopBackground.jpg");

            var image2 = Image.FromStream(assemb.GetManifestResourceStream(path + ".Images.sitelogo.gif"));
            image2.Save(reportsDirectory + "sitelogo.gif");

            var image3 = Image.FromStream(assemb.GetManifestResourceStream(path + ".Images.green.gif"));
            image3.Save(reportsDirectory + "green.gif");

            var image5 = Image.FromStream(assemb.GetManifestResourceStream(path + ".Images.red.gif"));
            image5.Save(reportsDirectory + "red.gif");
        }

        /// <summary>
        /// None of the tests passed.
        /// </summary>
        /// <returns></returns>
        public int NoTestsCompleted()
        {
            int count = 0;

            if (ListResults.Count > 0)
            {
                return count = ListResults.Where(x => x.TestStatus.Equals(Enums.TestStatus.Completed)).Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// None of the tests failed.
        /// </summary>
        /// <returns></returns>
        public int NoTestsFailed()
        {
            int count = 0;

            if (ListResults.Count > 0)
            {
                return count = ListResults.Where(x => x.TestStatus.Equals(Enums.TestStatus.Failed)).Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// None of the tests are incomplete.
        /// </summary>
        /// <returns></returns>
        public int NoTestsIncomplete()
        {
            int count = 0;

            if (ListResults.Count > 0)
            {
                return count = ListResults.Where(x => x.TestStatus.Equals(Enums.TestStatus.Incompleted)).Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// No tests found.
        /// </summary>
        /// <returns></returns>
        public int NoTests()
        {
            int count = 0;

            if (ListResults.Count > 0)
            {
                return count = ListResults.Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the total time.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        private string GetTotalTime(DateTime start, DateTime end)
        {
            TimeSpan t = TimeSpan.FromSeconds(end.Subtract(start).Duration().TotalSeconds);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
            return answer;
        }

        /// <summary>
        /// Sets start time.
        /// </summary>
        public void SetstartTime()
        {
            timeStart = DateTime.Now;
        }

        /// <summary>
        /// Sets the stop time.
        /// </summary>
        public void SetStopTime()
        {
            timeStop = DateTime.Now;
        }

        /// <summary>
        /// Currents the user Of N unit.
        /// </summary>
        /// <returns></returns>
        private string CurrentUserofNUnit()
        {
            string currentUser;
            currentUser = Environment.UserDomainName + "/" + Environment.UserName;
            return currentUser;
        }

        /// <summary>
        /// Gets the total time result.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        private string GetTotalTimeResult(DateTime start, DateTime end)
        {
            TimeSpan t = TimeSpan.FromSeconds(end.Subtract(start).Duration().TotalSeconds);

            string answer = string.Format("{0:D2}m:{1:D2}s", t.Minutes, t.Seconds);

            return answer;
        }

        #endregion

        #endregion

        #region Summary Report

        /// <summary>
        /// Sends the summary report to email
        /// </summary>
        public void SendSummaryReport()
        {
            MailMessage message = new MailMessage();

            message.Sender = message.From;
            message.From = new MailAddress("automation@openboxsoftware.com", "open box Automation");
            message.Sender = new MailAddress("automation@openboxsoftware.com", "open box Automation");
            message.Subject = "Summary Report" + this.ReportHeader + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            message.Body = this.PrintOverallStats().ToString();
            message.IsBodyHtml = true;
            foreach (string email in this.GetEmailAddresses())
            {
                message.To.Add(email);

                try
                {
                    SmtpClient smtp = new SmtpClient("mail1.openboxsoftware.com");
                    smtp.Credentials = new NetworkCredential("openbox\\automation", "P@ssw0rd");
                    smtp.Send(message);
                    message.To.Clear();
                }
                catch (Exception ex)
                {
                    this.emailError.Append(ex.Message);
                }
            }
        }

        #endregion
    }
}
