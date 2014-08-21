namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Defines tab pages
    /// </summary>
    public class TabPage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPage" /> class.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="pageElement">The page element.</param>
        /// <param name="locator">The locator.</param>
        public TabPage(string tabName, string locatorString, string pageElement, Enums.Locator locator)
        {
            this.TabName = tabName;
            this.LocatorString = locatorString;
            this.PageElement = pageElement;
            this.Locator = locator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPage" /> class.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="pageElement">The page element.</param>
        /// <param name="nestedFrame">The nested frame.</param>
        /// <param name="locator">The locator.</param>
        public TabPage(string tabName, string locatorString, string pageElement, string nestedFrame, Enums.Locator locator)
        {
            this.TabName = tabName;
            this.LocatorString = locatorString;
            this.PageElement = pageElement;
            this.FirstNestedFrame = nestedFrame;
            this.Locator = locator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPage" /> class.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        /// <param name="locatorString">The locator string.</param>
        /// <param name="pageElement">The page element.</param>
        /// <param name="nestedFrame">The nested frame.</param>
        /// <param name="secondFrame">The second frame.</param>
        /// <param name="locator">The locator.</param>
        public TabPage(string tabName, string locatorString, string pageElement, string nestedFrame, string secondFrame, Enums.Locator locator)
        {
            this.TabName = tabName;
            this.LocatorString = locatorString;
            this.PageElement = pageElement;
            this.FirstNestedFrame = nestedFrame;
            this.SecondNestedFrame = secondFrame;
            this.Locator = locator;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the tab.
        /// </summary>
        /// <value>
        /// The name of the tab.
        /// </value>
        public string TabName { get; set; }

        /// <summary>
        /// Gets or sets the locator string.
        /// </summary>
        /// <value>
        /// The locator string.
        /// </value>
        public string LocatorString { get; set; }

        /// <summary>
        /// Gets or sets the page element that verifies that you are on the correct page.
        /// </summary>
        /// <value>
        /// The page element.
        /// </value>
        public string PageElement { get; set; }

        /// <summary>
        /// Gets or sets the nested frame.
        /// </summary>
        /// <value>
        /// The nested frame.
        /// </value>
        public string FirstNestedFrame { get; set; }

        /// <summary>
        /// Gets or sets the nested frame.
        /// </summary>
        /// <value>
        /// The nested frame.
        /// </value>
        public string SecondNestedFrame { get; set; }

        /// <summary>
        /// Gets or sets the locator.
        /// </summary>
        /// <value>
        /// The locator.
        /// </value>
        public Enums.Locator Locator { get; set; }

        #endregion
    }
}
