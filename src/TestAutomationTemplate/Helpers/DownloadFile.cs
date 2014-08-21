#region Directives

using System;
using System.Threading;
using System.Windows.Automation;
using TestAutomationTemplate.Core;

#endregion

namespace TestAutomationTemplate.Helpers
{
    /// <summary>
    /// Download File in Internet Explorer
    /// </summary>
  public class DownloadFile
  {
      /// <summary>
      /// The I e9 window
      /// </summary>
        private static AutomationElement IE9Window;

        /// <summary>
        /// The save completed
        /// </summary>
        private bool saveCompleted = false;

        /// <summary>
        /// The file download tool bar
        /// </summary>
        private static AutomationElement fileDownloadToolBar;

        /// <summary>
        /// The time out value
        /// </summary>
        private static int timeOutValue = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFile"/> class.
        /// </summary>
        public DownloadFile()
        {
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="WindowTitle">The window title.</param>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public bool SaveFile(string WindowTitle, string FileName)
        {
            IE9Window = SelectIE9Window(WindowTitle);
            if (IE9Window != null && this.IsfileDownloadToolBarPresent())
            { 
                try
                { 
                    //// Find and click the splitbutton before selecting the Save As option 
                    AutomationElement saveAsSplitButton = this.FindSaveAsSplitButton(fileDownloadToolBar);
                    this.InvokeClickPattern(saveAsSplitButton);
                    Thread.Sleep(1000);

                    //// Traverse back to desktop to find menu popup and click the Save As menu item  
                    AutomationElement desktop = AutomationElement.RootElement;
                    AutomationElement saveAsMenuItem = this.FindSaveAsPopupMenuItem(desktop, saveAsSplitButton);

                    this.InvokeClickPattern(saveAsMenuItem);

                    //// Find file save dialog and enter file name for download
                    var fileSaveDialog = this.FindFileSaveDialog();
                    
                    var fileNameTextBox = this.FindElementWithinParentUsingAutomationId(fileSaveDialog, "FileNameControlHost");
                    this.InvokeSetValuePattern(fileNameTextBox, FileName);
                    //// Save the file by clicking the save button

                    var saveButton = this.FindElementWithinParentUsingNameProperty(fileSaveDialog, "Save");
                    this.InvokeClickPattern(saveButton);

                    this.saveCompleted = true;
                    return this.saveCompleted;
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                    return this.saveCompleted;
                }
            }
            else
            {
                return this.saveCompleted;
            }
        }

        /// <summary>
        /// Selects the I e9 window.
        /// </summary>
        /// <param name="windowTitle">The window title.</param>
        /// <returns></returns>
        public static AutomationElement SelectIE9Window(string windowTitle)
        {
            AutomationElement browser = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, windowTitle));
            return browser;
        }

        /// <summary>
        /// Invokes the click pattern.
        /// </summary>
        /// <param name="elementToClick">The element to click.</param>
        public void InvokeClickPattern(AutomationElement elementToClick)
        {
            InvokePattern clickPattern = elementToClick.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            clickPattern.Invoke();
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Invokes the set value pattern.
        /// </summary>
        /// <param name="elementToInvoke">The element to invoke.</param>
        /// <param name="TextToEnter">The text to enter.</param>
        public void InvokeSetValuePattern(AutomationElement elementToInvoke, string TextToEnter)
        {
            ValuePattern setValuePattern = elementToInvoke.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            setValuePattern.SetValue(TextToEnter);
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Invokes the deselect pattern.
        /// </summary>
        /// <param name="elementToDeselect">The element to deselect.</param>
        public void InvokeDeselectPattern(AutomationElement elementToDeselect)
        {
            SelectionItemPattern setValuePattern = elementToDeselect.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            setValuePattern.RemoveFromSelection();
        }

        /// <summary>
        /// Is the download tool bar present.
        /// </summary>
        /// <returns></returns>
        public bool IsfileDownloadToolBarPresent()
        {
            int timer = 0;
            bool toolBarFound = false;
            while (timer < timeOutValue)
            {
                try
                {
                    AutomationElementCollection panels = IE9Window.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane));
                    foreach (AutomationElement panel in panels)
                    {
                        fileDownloadToolBar = panel.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                        if (fileDownloadToolBar != null && fileDownloadToolBar.Current.Name.Equals("Notification bar"))
                        {
                            break;
                        }
                    }

                    if (fileDownloadToolBar != null)
                    {
                        toolBarFound = true;
                        break;
                    }                       
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                }
                //// IE9Window.ReInitialize(InitializeOption.WithCache);
                System.Threading.Thread.Sleep(1000);
                timer++;
            }

            return toolBarFound;      
        }

        /// <summary>
        /// Finds the file save dialog.
        /// </summary>
        /// <returns></returns>
        public AutomationElement FindFileSaveDialog()
        {
            int timer = 0;
            AutomationElement fileSaveDialog = null;
            while (timer < timeOutValue)
            {
                try
                {
                    fileSaveDialog = IE9Window.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Save As"));
                    if (fileSaveDialog != null)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                }

                System.Threading.Thread.Sleep(1000);
                timer++;
            }

            return fileSaveDialog;     
        }

        /// <summary>
        /// Finds the element within parent using automation id.
        /// </summary>
        /// <param name="ParentElement">The parent element.</param>
        /// <param name="AutomationId">The automation id.</param>
        /// <returns></returns>
        public AutomationElement FindElementWithinParentUsingAutomationId(AutomationElement ParentElement, string AutomationId)
        {
            int timer = 0;
            AutomationElement targetElement = null;
            while (timer < timeOutValue)
            {
                try
                {
                    targetElement = ParentElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, AutomationId));
                    if (targetElement != null)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                }

                System.Threading.Thread.Sleep(1000);
                timer++;
            }

            return targetElement;  
        }

        /// <summary>
        /// Finds the element within parent using name property.
        /// </summary>
        /// <param name="ParentElement">The parent element.</param>
        /// <param name="ElementName">Name of the element.</param>
        /// <returns></returns>
        public AutomationElement FindElementWithinParentUsingNameProperty(AutomationElement ParentElement, string ElementName)
        {
            int timer = 0;
            AutomationElement targetElement = null;
            while (timer < timeOutValue)
            {
                try
                {
                    targetElement = ParentElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ElementName, PropertyConditionFlags.IgnoreCase));
                    if (targetElement != null)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                }

                System.Threading.Thread.Sleep(1000);
                timer++;
            }

            return targetElement;
        }

        /// <summary>
        /// Finds the save as popup menu item.
        /// </summary>
        /// <param name="ParentElement">The parent element.</param>
        /// <param name="saveAsSplitButton">The save as split button.</param>
        /// <returns></returns>
        public AutomationElement FindSaveAsPopupMenuItem(AutomationElement ParentElement, AutomationElement saveAsSplitButton)
        {
            int timer = 0;
            AutomationElement targetElement = null;
            while (timer < timeOutValue)
            {
                try
                {
                    AutomationElementCollection desktopMenus = ParentElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Menu));
                    AutomationElementCollection desktopMenuItems;
                    foreach (AutomationElement menu in desktopMenus)
                    {
                        desktopMenuItems = menu.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));
                        foreach (AutomationElement menuItem in desktopMenuItems)
                        {
                            if (menuItem != null && menuItem.Current.Name.Equals("Save as"))
                            {
                                targetElement = menuItem;
                                return targetElement;
                            }
                        }                       
                    }
                    //// Try to click the split button again incase it failed to register the first time.
                    this.InvokeClickPattern(saveAsSplitButton); 
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                }

                System.Threading.Thread.Sleep(1000);
                timer++;
            }

            return targetElement;
        }

        /// <summary>
        /// Finds the save as split button.
        /// </summary>
        /// <param name="ParentElement">The parent element.</param>
        /// <returns></returns>
        public AutomationElement FindSaveAsSplitButton(AutomationElement ParentElement)
        {
            int timer = 0;
            AutomationElement targetElement = null;
            while (timer < timeOutValue)
            {
                try
                {
                    AutomationElement splitButtonParentButton;
                    
                    splitButtonParentButton = ParentElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.SplitButton));
                    if (splitButtonParentButton != null)
                    {
                        targetElement = splitButtonParentButton.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.SplitButton));
                        if (targetElement != null)
                        {
                            return targetElement;    
                        }
                    }          
                }
                catch (Exception e)
                {
                    EventLogger.LogEvent(e.Message.ToString());
                }

                System.Threading.Thread.Sleep(1000);
                timer++;
            }

            return targetElement;
        }

        /// <summary>
        /// Deselects all items in element.
        /// </summary>
        /// <param name="ParentElement">The parent element.</param>
        public void DeselectAllItemsInElement(AutomationElement ParentElement)
        {
            AutomationElement itemsView = ParentElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Items View"));

            if (itemsView != null)
            {
                AutomationElementCollection listItems = itemsView.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
                foreach (AutomationElement listItem in listItems)
                {
                    try
                    {
                        this.InvokeDeselectPattern(listItem);
                    }
                    catch (Exception e)
                    {
                        EventLogger.LogEvent(e.Message.ToString());
                    }
                }
            }
        }
    }
}
