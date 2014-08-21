#region Directives

using ExampleMVPApplication.Models;
using ExampleMVPApplication.Structs;

#endregion

namespace ExampleMVPApplication.ViewModel
{
    /// <summary>
    /// The ExchangeRateViewModel class
    /// </summary>
    public class ExchangeRateViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateViewModel"/> class.
        /// </summary>
        public ExchangeRateViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateViewModel"/> class.
        /// </summary>
        /// <param name="exchangeRateModel">The exchange rate model.</param>
        public ExchangeRateViewModel(ExchangeRateModel exchangeRateModel)
        {
            this.Period = exchangeRateModel.Period;
            this.CurrencyCode = exchangeRateModel.CurrencyCode;
            this.Rate = exchangeRateModel.Rate;
        }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public Period Period { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>
        /// The rate.
        /// </value>
        public decimal Rate { get; set; }
    }
}
