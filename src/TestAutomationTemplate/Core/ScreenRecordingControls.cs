#region Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestAutomationTemplate.Helpers;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// This class controls the VLC controls to start recording the Videos
    /// </summary>
    public static class ScreenRecordingControls
    {
        /// <summary>
        /// Ends the recording.
        /// </summary>
        public static void EndRecording()
        {
            try
            {
                Wait.Pause(5);
                System.Diagnostics.Process.Start("taskkill", "/IM vlc.exe");
            }
            catch (Exception e)
            {
                EventLogger.LogEvent(e.Message.ToString());
            }
        }

        /// <summary>
        /// Starts the video recording with resolution.
        /// </summary>
        public static void StartVideoRecordingWithResolution()
        {
            string width, height;
            width = CustomConfiguration.ScreenResolution_Width;
            height = CustomConfiguration.ScreenResolution_Height;
            string currentfileName = string.Empty;
            System.Diagnostics.ProcessStartInfo si = new System.Diagnostics.ProcessStartInfo();
            si.CreateNoWindow = true;
            si.FileName = CustomConfiguration.PathToVLC;
            si.Arguments = "screen:// :screen-fps=25 :screen-width=" + width + " :screen-height=" + height + " :screen-caching=100 :sout=#transcode{venc=x264{bframes=0,nocabac,ref=1,nf,level=13,crf=24,partitions=none},vcodec=h264,fps=25,vb=3000,width=" + width + ",height=" + height + ",acodec=none}:duplicate{dst=std{mux=mp4,access=file,dst=C:\\Reports\\TestReportVideo.mp4}}";
            si.UseShellExecute = false;
            System.Diagnostics.Process.Start(si); 
        }
    }
}
