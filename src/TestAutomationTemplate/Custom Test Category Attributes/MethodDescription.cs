using System;

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Method Description Attribute
    /// </summary>
    public class MethodDescription : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescription"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public MethodDescription(string description)
        {
            this.Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        #endregion
    }
}
