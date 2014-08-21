using System;
using System.Configuration;
using TestAutomationTemplate.Enums;

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Application Configuration Data is managed in this class
    /// </summary>
    public class CustomConfiguration
    {
        /// <summary>
        /// Gets the base site URL.
        /// </summary>
        /// <value>
        /// The base site URL.
        /// </value>
        public static string BaseSiteUrl
        {
            get { return ConfigurationManager.AppSettings["BaseUrl"]; }
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        /// <summary>
        /// Gets the error image path.
        /// </summary>
        /// <value>
        /// The error image path.
        /// </value>
        public static string ErrorImagePath
        {
            get { return ConfigurationManager.AppSettings["ErrorImagePath"]; }
        }

        /// <summary>
        /// Gets the wait for response.
        /// </summary>
        /// <value>
        /// The wait for response.
        /// </value>
        public static int WaitForResponse
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["ResponseTimeInSeconds"]);
                }
                catch
                {
                    return 10;
                }
            }
        }

        /// <summary>
        /// Gets the type of the browser.
        /// </summary>
        /// <value>
        /// The type of the browser.
        /// </value>
        public static WebBrowser BrowserType
        {
            get
            {
                try
                {
                    return (WebBrowser)Enum.Parse(typeof(WebBrowser), ConfigurationManager.AppSettings["BrowserType"], true);
                }
                catch
                {
                    return WebBrowser.Ie;
                }
            }
        }

        /// <summary>
        /// Gets the path to desktop driver.
        /// </summary>
        /// <value>
        /// The path to desktop driver.
        /// </value>
        public static string PathToWebDriverDrivers
        {
            get { return ConfigurationManager.AppSettings["PathToDesktopIeDriver"]; }
        }

        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public static string Devices
        {
            get { return ConfigurationManager.AppSettings["PathToDevicesXML"]; }
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ITRS"].ConnectionString; }
        }

        /// <summary>
        /// Gets the mobile URL.
        /// </summary>
        /// <value>
        /// The mobile URL.
        /// </value>
        public static string MobileUrl
        {
            get { return ConfigurationManager.AppSettings["MobileUrl"]; }
        }

        /// <summary>
        /// Gets a value indicating whether [record video].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [record video]; otherwise, <c>false</c>.
        /// </value>
        public static bool RecordVideo
        {
            get { return VideoRecordingStatus(); }
        }

        /// <summary>
        /// Gets the path to VLC.
        /// </summary>
        /// <value>
        /// The path to VLC.
        /// </value>
        public static string PathToVLC
        {
            get { return ConfigurationManager.AppSettings["PathToVLC"]; }
        }

        /// <summary>
        /// Gets the width of the screen resolution_.
        /// </summary>
        /// <value>
        /// The width of the screen resolution_.
        /// </value>
        public static string ScreenResolution_Width
        {
            get { return ConfigurationManager.AppSettings["ScreenResolution_Width"]; }    
        }

        /// <summary>
        /// Gets the height of the screen resolution_.
        /// </summary>
        /// <value>
        /// The height of the screen resolution_.
        /// </value>
        public static string ScreenResolution_Height
        {
            get { return ConfigurationManager.AppSettings["ScreenResolution_Height"]; }
        }

        /// <summary>
        /// Gets the temporary directory.
        /// </summary>
        /// <value>
        /// The temporary directory.
        /// </value>
        public static string TempDirectory
        {
            get { return ConfigurationManager.AppSettings["TempDirectory"]; }
        }

        /// <summary>
        /// Gets the path to selenium grid server.
        /// </summary>
        /// <value>
        /// The path to selenium grid server.
        /// </value>
        public static string PathToSeleniumGridServer
        {
            get { return ConfigurationManager.AppSettings["SeleniumGridServerPath"]; }
        }

        /// <summary>
        /// Videoes the recording status.
        /// </summary>
        /// <returns></returns>
        private static bool VideoRecordingStatus()
        {
            if (ConfigurationManager.AppSettings["VideoRecording"].ToLower().Equals("true"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}