#region Directives

using System;
using System.Text;

#endregion

namespace TestAutomationTemplate.ReportGeneration
{
    /// <summary>
    /// Generate the Javascript that is used in the reports
    /// </summary>
    public class ReportJavaScript
    {
        #region Methods
        /// <summary>
        /// Generates the java script code.
        /// </summary>
        /// <param name="testDataRowCount">The test data row count.</param>
        /// <returns></returns>
        public StringBuilder GenerateJavaScriptCode(int testDataRowCount)
        {
            StringBuilder javaScriptString = new StringBuilder();
            javaScriptString.AppendLine(@"<script language=""javascript"">");
            javaScriptString.AppendLine(@"function rowsHideShow(e)");
            javaScriptString.AppendLine(@"{");
            javaScriptString.AppendLine(@"   var clickedRowIndex = parseInt(e.id,10);");
            javaScriptString.AppendLine(@"   var testDataRow = document.getElementById(clickedRowIndex + 1);");
            javaScriptString.AppendLine(@"   if (testDataRow.style.display == ""table-row"")");
            javaScriptString.AppendLine(@"   {");
            javaScriptString.AppendLine(@"       testDataRow.style.display = ""none"";");
            javaScriptString.AppendLine(@"   }");
            javaScriptString.AppendLine(@"   else");
            javaScriptString.AppendLine(@"   {");
            javaScriptString.AppendLine(@"      testDataRow.style.display = ""table-row"";");
            javaScriptString.AppendLine(@"   }");
            javaScriptString.AppendLine(@"}");
            javaScriptString.AppendLine(@"</script>");
            javaScriptString.AppendLine(@"</body>");
            javaScriptString.AppendLine(@"</html>");

            return javaScriptString;
        }

        /// <summary>
        /// Writes the HTML header information.
        /// </summary>
        /// <returns></returns>
        public StringBuilder WriteHTMLHeaderInformation()
        {
            StringBuilder headerScriptString = new StringBuilder();
            headerScriptString.AppendLine(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">");
            headerScriptString.AppendLine(@"<html>");
            headerScriptString.AppendLine(@"<head>");
            headerScriptString.AppendLine(@"<style>");
            headerScriptString.AppendLine(@"#bannerDiv");
            headerScriptString.AppendLine(@"{");
            headerScriptString.AppendLine(@"  background-image: url(""img_TopBackground.jpg"");");
            headerScriptString.AppendLine(@"  background-repeat: repeat-x;");
            headerScriptString.AppendLine(@"  background-size:80px 60px;");
            headerScriptString.AppendLine(@"  background-position: 0 0;");
            headerScriptString.AppendLine(@"}");
            headerScriptString.AppendLine(@"p");
            headerScriptString.AppendLine(@"{");
            headerScriptString.AppendLine(@"  text-align: right;");
            headerScriptString.AppendLine(@"  font-family: Verdana, Helvetica, sans-serif;");
            headerScriptString.AppendLine(@"  font-size: 11px;");
            headerScriptString.AppendLine(@"  vertical-align: top;");
            headerScriptString.AppendLine(@"  float: right;");
            headerScriptString.AppendLine(@"}");
            headerScriptString.AppendLine(@"</style>");
            headerScriptString.AppendLine(@"</head>");
            headerScriptString.AppendLine(@"<body>");
            headerScriptString.AppendLine(@"<div id=""bannerDiv"">");
            headerScriptString.AppendLine(@"<table width=""100%"">");
            headerScriptString.AppendLine(@"<tr width=""100%"">");
            headerScriptString.AppendLine(@"    <td height=""65"" width=""100%"" colSpan=""2"">");
            headerScriptString.AppendLine(@"      <img src=""sitelogo.gif"" alt=""Wonga.com"" width=""auto"" height=""auto"" style=""border-style: none""/>");
            headerScriptString.AppendLine(@"       <p>" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "</p>");
            headerScriptString.AppendLine(@"    </td>");
            headerScriptString.AppendLine(@"</tr>");
            headerScriptString.AppendLine(@"</table>");
            headerScriptString.AppendLine(@"</div>");
            return headerScriptString;
        }

        #endregion
    }
}
