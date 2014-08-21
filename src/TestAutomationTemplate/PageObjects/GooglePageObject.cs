using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationTemplate.Core;
using TestAutomationTemplate.Helpers;

namespace TestAutomationTemplate.PageObjects
{
    /// <summary>
    /// Google Page Object
    /// </summary>
   public class GooglePageObject : Navigation
   {
       #region Properties

       /// <summary>
        /// Gets the google search button.
        /// </summary>
        /// <value>
        /// The google search button.
        /// </value>
       private string GoogleSearchButton
       {
           get { return "gbqfb"; }
       }

       /// <summary>
       /// Gets the i am feeling lucky button.
       /// </summary>
       /// <value>
       /// The i am feeling lucky button.
       /// </value>
       private string IAmFeelingLuckyButton
       {
           get { return "gbqfbb"; }
       }

       /// <summary>
       /// Gets the search text box.
       /// </summary>
       /// <value>
       /// The search text box.
       /// </value>
       private string SearchTextBox
       {
           get { return "gbqfq"; }
       }

       #endregion

       #region Methods

       /// <summary>
       /// Googles the search for.
       /// </summary>
       /// <param name="searchTerm">The search term.</param>
       [MethodDescription("Do a standard Google Search using this search Term")]
       public void GoogleSearchFor(string searchTerm)
       {
            this.EnterText(this.SearchTextBox, Enums.Locator.ID, searchTerm);
            this.ClickElement(this.GoogleSearchButton, Enums.Locator.ID);   
       }

       /// <summary>
       /// Googles the search for.
       /// </summary>
       /// <param name="searchTerm">The search term.</param>
       [MethodDescription("Do a I am feeling lucky search")]
       public void IAmFeelingSearchFor(string searchTerm)
       {
            this.EnterText(this.SearchTextBox, Enums.Locator.ID, searchTerm);
            this.ClickElement(this.IAmFeelingLuckyButton, Enums.Locator.ID);
       }

       /// <summary>
       /// Searches the result div is present.
       /// </summary>
       [MethodDescription("Find the search Results div on the page")]
       public void SearchResultDivIsPresent()
       {
           this.FindElement("res", Enums.Locator.ID);
       }

       #endregion
   }
}
