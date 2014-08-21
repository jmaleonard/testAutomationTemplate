#region Directives

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using TestAutomationTemplate.Core;

#endregion

namespace TestAutomationTemplate.ReportGeneration
{
    /// <summary>
    /// This is the report used to generate the Smoke Tests reports
    /// </summary>
    public class SmokeTestGenerateReport
    {
        #region Fields
        private static List<SmokeTestResult> _ListResults = new List<SmokeTestResult>();
        private string emailBody;
        private StringBuilder emailError = null;
        private List<string> emailList = new List<string>();
        private static DateTime timeStart;
        private static DateTime timeStop;
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
        public static List<SmokeTestResult> ListResults
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
        public string WriteResultsToEmail()
        {
            if (ListResults.Count == 0)
            {
                return this.emailBody = this.NoResults();
            }
            else if (ListResults.Count != 0)
            {
                return this.emailBody = this.PrintResults();
            }

            return "HOW DID THIS HAPPEN";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="currentResult">The current result.</param>
        public void AddItem(SmokeTestResult currentResult)
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
            MailMessage message = new MailMessage();
            message.Sender = message.From;
            message.From = new MailAddress("automation@openboxsoftware.com", "open box Automation");
            message.Sender = new MailAddress("automation@openboxsoftware.com", "open box Automation");
            message.Subject = this.ReportHeader + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            message.Body = this.WriteResultsToEmail();
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
                    return false;
                }
            }

            this.emailBody = string.Empty;
            return true;
        }

        /// <summary>
        /// Gets the email addresses.
        /// </summary>
        /// <returns></returns>
        public List<string> GetEmailAddresses()
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
        public string NoResults()
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
        public string PrintResults()
        {
           HtmlTable table = new HtmlTable();
            table.EnableTheming = true;
            table.Border = 1;
            table.BorderColor = "#000000";
            table.Width = "100%";
            table.CellSpacing = 0;

            HtmlTableRow tableHeadings = new HtmlTableRow();
            HtmlTableCell headingCell;
            tableHeadings.BgColor = "#1F497D";

            headingCell = new HtmlTableCell();
            headingCell.InnerText = "Test Description";
            headingCell.Attributes.CssStyle.Value = "border-bottom: 1px solid;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;font-weight:700;color:#FFFFFF;text-indent:100px;";
            tableHeadings.Cells.Add(headingCell);

            headingCell = new HtmlTableCell();
            headingCell.InnerText = "Test Status";
            headingCell.Width = "30px";
            headingCell.Attributes.CssStyle.Value = "border-bottom: 1px solid;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;font-weight:700;color:#FFFFFF;text-indent:0px;";
            tableHeadings.Cells.Add(headingCell);

            headingCell = new HtmlTableCell();
            headingCell.InnerText = "Test Duration";
            headingCell.Width = "30px";
            headingCell.Attributes.CssStyle.Value = "border-bottom: 1px solid;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;font-weight:700;color:#FFFFFF;text-indent:0px;";
            tableHeadings.Cells.Add(headingCell);

            headingCell = new HtmlTableCell();
            headingCell.InnerText = "Comments";
            headingCell.Attributes.CssStyle.Value = "border-bottom: 1px solid;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:9pt;font-weight:700;color:#FFFFFF;text-indent:0px;";
            tableHeadings.Cells.Add(headingCell);
            table.Rows.Add(tableHeadings);

            foreach (var RootTab in ListResults.GroupBy(item => item.RootTab))
            {
                HtmlTableRow tableName = new HtmlTableRow();
                HtmlTableCell tableNameCell = new HtmlTableCell("th");
                tableNameCell.ColSpan = 4;
                tableName.BorderColor = "#FFFFFF";
                tableNameCell.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:13pt;color:#FFFFFF;";
                tableNameCell.BgColor = "#808080";
                tableNameCell.Align = "left";
                tableNameCell.InnerText = RootTab.Key;
                tableName.BgColor = "#808080";
                tableName.Cells.Add(tableNameCell);
                table.Rows.Add(tableName);

                foreach (var parentTab in RootTab.GroupBy(item => item.ParentTab))
                {
                    HtmlTableRow appheaderRow = new HtmlTableRow();
                    HtmlTableCell header = new HtmlTableCell("th");
                    header.ColSpan = 4;
                    appheaderRow.BgColor = "#808080";
                    appheaderRow.BorderColor = "#FFFFFF";
                    header.BgColor = "#808080";
                    header.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:11pt;color:#FFFFFF;text-indent:100px;";
                    header.Align = "Left";
                    header.InnerText = parentTab.Key;
                    appheaderRow.Cells.Add(header);
                    table.Rows.Add(appheaderRow);

                    foreach (var childTab in parentTab.GroupBy(item => item.ChildTab))
                    {
                        HtmlTableRow childRow = new HtmlTableRow();
                        HtmlTableCell childheader = new HtmlTableCell("th");
                        childheader.ColSpan = 4;
                        childRow.BgColor = "#808080";
                        childRow.BorderColor = "#FFFFFF";
                        childheader.BgColor = "#a6a6a6";
                        childheader.Attributes.CssStyle.Value = "border-bottom: None;border-right:None;border-top:1px solid;border-left:None; font-family:Verdana; font-size:10pt;color:#FFFFFF;text-indent:150px;";
                        childheader.Align = "Left";
                        childheader.InnerText = childTab.Key;
                        childRow.Cells.Add(childheader);
                        table.Rows.Add(childRow);

                        foreach (SmokeTestResult result in childTab)
                        {
                            HtmlTableRow resultRow = new HtmlTableRow();
                            HtmlTableCell cell;
                            resultRow.Attributes.CssStyle.Value = "font-family:Verdana;text-indent:200px;";
                            cell = new HtmlTableCell();
                            cell.InnerText = result.TestName;
                            if (result.TestStatus == Enums.TestStatus.Failed)
                            {
                                cell.BgColor = "#C0504D";
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;font-weight:bold;color:#FFFFFF;text-indent:200px;";
                            }
                            else
                            {
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;text-indent:200px;";
                            }

                            resultRow.Cells.Add(cell);
                            cell = new HtmlTableCell();
                            cell.InnerText = result.TestStatus.ToString();
                            if (result.TestStatus == Enums.TestStatus.Failed)
                            {
                                cell.BgColor = "#C0504D";
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;font-weight:bold;color:#FFFFFF;text-indent:0px;";
                            }
                            else if (result.TestStatus == Enums.TestStatus.Completed)
                            {
                                cell.BgColor = "#9BBB59";
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-weight:bold;color:#FFFFFF;font-family:Verdana;font-size:9pt;text-indent:0px;";
                            }
                            else if (result.TestStatus == Enums.TestStatus.Incompleted)
                            {
                                cell.BgColor = "#F59B00";
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-weight:bold;color:#FFFFFF;font-family:Verdana;font-size:9pt;text-indent:0px;";
                            }

                            resultRow.Cells.Add(cell);
                            cell = new HtmlTableCell();
                            cell.InnerText = this.GetTotalTimeResult(result.StartTime, result.EndTime);
                            if (result.TestStatus == Enums.TestStatus.Failed)
                            {
                                cell.BgColor = "#C0504D";
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;font-weight:bold;color:#FFFFFF;text-indent:0px;";
                            }
                            else
                            {
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;text-indent:0px;";
                            }

                            resultRow.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.InnerText = result.ErrorMessage;
                            if (result.TestStatus == Enums.TestStatus.Failed)
                            {
                                cell.BgColor = "#C0504D";
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;font-weight:bold;color:#FFFFFF;text-indent:0px;";
                            }
                            else
                            {
                                cell.Attributes.CssStyle.Value = "border-bottom: None; border-right:1px solid; border-top:None; border-left:None;font-family:Verdana;font-size:9pt;text-indent:0px;";
                            }

                            resultRow.Cells.Add(cell);
                            table.Rows.Add(resultRow);
                        }
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            table.RenderControl(new HtmlTextWriter(new StringWriter(stringBuilder)));
            this.emailBody = this.PrintOverallStats() + stringBuilder.ToString();
            string reportDirectory = CustomConfiguration.TempDirectory;
            File.WriteAllText(reportDirectory + "Results.html", this.emailBody);
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
            header.InnerText = this.reportHeader + " (" + this.CurrentUserofNUnit() + ")";
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
            headingCell.InnerHtml = @"<span style=""color:#666""><b>Environment: </b></span>" + CustomConfiguration.Environment;
            tableHeadings.BgColor = "#FFFFFF";
            headingCell.ColSpan = 1;
            headingCell.Attributes.CssStyle.Value = "border-bottom:None;border-right:None;border-top:None;border-left:None; font-family:Verdana; font-size:10pt;padding:5px 5px 5px 5px;";
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
                headingCell.InnerHtml = @"<span style=""color:#666""><b>No. of Passed Test(s): </b></span>" + this.NoTestsCompleted().ToString() + " of " + this.NoTests().ToString();
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
                headingCell.InnerHtml = @"<span style=""color:#666""><b>No. of Failed Test(s): </b></span>" + this.NoTestsFailed().ToString() + " of " + this.NoTests().ToString();
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
        /// Noes the tests passed.
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
        /// Noes the tests failed.
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
        /// Noes the tests incomplete.
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
        /// Noes the tests.
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
        public string GetTotalTime(DateTime start, DateTime end)
        {
            TimeSpan t = TimeSpan.FromSeconds(end.Subtract(start).Duration().TotalSeconds);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
            return answer;
        }

        /// <summary>
        /// Set start the time.
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
        /// Currents the user of N unit.
        /// </summary>
        /// <returns></returns>
        public string CurrentUserofNUnit()
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
        public string GetTotalTimeResult(DateTime start, DateTime end)
        {
            TimeSpan t = TimeSpan.FromSeconds(end.Subtract(start).Duration().TotalSeconds);
            string answer = string.Format("{0:D2}s:{1:D2}ms", t.Seconds, t.Milliseconds);
            return answer;
        }

        #endregion

        #endregion
    }
}
