#region Directives

using System;
using System.Data;
using System.Drawing;

#endregion

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Mobile Device Metrics
    /// </summary>
   public static class MobileDeviceSizes
    {
        #region Fields

        /// <summary>
        /// The _data set
        /// </summary>
        private static DataSet _dataSet = new DataSet();

        /// <summary>
        /// The _data view
        /// </summary>
        private static DataView _dataView = new DataView();

        #endregion

        #region Methods

        /// <summary>
        /// Selects all.
        /// </summary>
        /// <returns></returns>
       private static DataView SelectAll()
       {
           try
           {
               _dataSet.Clear();
               _dataSet.ReadXml(CustomConfiguration.Devices, XmlReadMode.ReadSchema);
               _dataView = _dataSet.Tables[0].DefaultView;
               return _dataView;
           }
           catch (Exception ex)
           {
               EventLogger.LogEvent(ex.Message.ToString());
               return null;
           }
       }

       /// <summary>
       /// Selects the specified device name.
       /// </summary>
       /// <param name="DeviceName">Name of the device.</param>
       /// <returns></returns>
       public static Size Select(Enums.MobileDevice DeviceName)
       {
           try
           {
               MobileDeviceSizes.SelectAll();

               Size deviceSize = new Size();
               _dataView.RowFilter = "DeviceName='" + DeviceName.ToString() + "'"; 
               DataRow dataRow = null;
               if (_dataView.Count > 0)
               {
                   dataRow = _dataView[0].Row;
               }

               deviceSize.Width = (int)dataRow[1];
               deviceSize.Height = (int)dataRow[2];

               _dataView = null;
               return deviceSize;
           }
           catch (Exception ex)
           {
               EventLogger.LogEvent(ex.Message.ToString());
               return new Size(1280, 1024);
           }
       }
        #endregion
    }      
}       