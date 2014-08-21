#region Directives

using System;
using OpenQA.Selenium;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// This class defines the way screenshots are captured
    /// </summary>
    internal class ScreenShotImage : BaseClass
    {
        /// <summary>
        /// Captures the screen shot.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        public void CaptureScreenShot(string imageName)
        {
            try
            {
                Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
                ss.SaveAsFile(CustomConfiguration.TempDirectory + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        /// <summary>
        /// Delete the given file
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
