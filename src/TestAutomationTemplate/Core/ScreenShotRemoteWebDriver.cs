#region Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Custom RemoteWebDriver
    /// </summary>
    public class ScreenShotRemoteWebDriver : RemoteWebDriver, ITakesScreenshot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenShotRemoteWebDriver" /> class.
        /// </summary>
        /// <param name="remoteAddress">The remote address.</param>
        /// <param name="desiredCapabilities">The desired capabilities.</param>
        public ScreenShotRemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities)
            : base(remoteAddress, desiredCapabilities)
        {
        }

        /// <summary>
        /// Gets a <see cref="T:OpenQA.Selenium.Screenshot" /> object representing the image of the page on the screen.
        /// </summary>
        /// <returns>
        /// A <see cref="T:OpenQA.Selenium.Screenshot" /> object containing the image.
        /// </returns>
        public Screenshot GetScreenshot()
        {
            // Get the screenshot as base64. 
            Response screenshotResponse = this.Execute(DriverCommand.Screenshot, null);
            string base64 = screenshotResponse.Value.ToString();
            return new Screenshot(base64);
        }
    }
}
