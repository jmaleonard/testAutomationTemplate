#region Directives

using System.Collections.Generic;

#endregion

namespace ExampleMVPApplication.ViewModel
{
    /// <summary>
    /// The View Model for the Exchange Rates Controller.
    /// At the moment we are only using it to return error message
    /// </summary>
    public class ExchangeRateControllerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateControllerViewModel"/> class.
        /// </summary>
        public ExchangeRateControllerViewModel()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IList<string> Errors { get; set; }
    }
}
