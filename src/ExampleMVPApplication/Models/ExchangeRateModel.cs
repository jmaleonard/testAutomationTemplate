#region Directives

using ExampleMVPApplication.Structs;

#endregion

namespace ExampleMVPApplication.Models
{
    /// <summary>
    /// Model that we will use to interact with the Presenter
    /// </summary>
    public class ExchangeRateModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateModel"/> class.
        /// </summary>
        /// <param name="exchangeRate">The exchange rate.</param>
        internal ExchangeRateModel(ExchangeRate exchangeRate)
        {
            this.ExchangeRateId = exchangeRate.ExchangeRateId;
            this.Period = exchangeRate.Period;
            this.CurrencyCode = exchangeRate.CurrencyCode;
            this.Rate = exchangeRate.Rate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateModel" /> class.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="currencyCode">The currency code.</param>
        public ExchangeRateModel(Period period, string currencyCode)
        {
            this.Period = period;
            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateModel"/> class.
        /// </summary>
        /// <param name="exchangeRateId">The exchange rate id.</param>
        public ExchangeRateModel(int exchangeRateId)
        {
            this.ExchangeRateId = exchangeRateId;
        }

        /// <summary>
        /// Gets the exchange rate id.
        /// </summary>
        /// <value>
        /// The exchange rate id.
        /// </value>
        public int ExchangeRateId { get; internal set; }

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
