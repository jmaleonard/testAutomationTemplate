#region Directives

using System;
using System.Collections.Generic;
using ExampleMVPApplication.Models;
using ExampleMVPApplication.Structs;

#endregion

namespace ExampleMVPApplication.ViewModel
{
    /// <summary>
    /// Exchange Rate List ViewModel
    /// </summary>
    public class ExchangeRateListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateListViewModel"/> class.
        /// </summary>
        /// <param name="period">The period.</param>
        public ExchangeRateListViewModel(Period period)
            : this()
        {
            this.Period = period;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateListViewModel"/> class.
        /// </summary>
        /// <param name="period">The period.</param>
        public ExchangeRateListViewModel(int period)
            : this()
        {
            this.Period = period;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ExchangeRateListViewModel"/> class from being created.
        /// </summary>
        private ExchangeRateListViewModel()
        {
            this.ExchangeRateList = new List<ExchangeRateViewModel>();
        }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public Period Period { get; set; }

        /// <summary>
        /// Gets the exchange rate list.
        /// </summary>
        /// <value>
        /// The exchange rate list.
        /// </value>
        public IList<ExchangeRateViewModel> ExchangeRateList { get; private set; }

        /// <summary>
        /// Convert the ExchangeRateModel into an ExchangeRateViewModel and add it to the ExchangeRateList.
        /// </summary>
        /// <param name="exchangeRateModel">The exchange rate model.</param>
        public void AddExchangeRate(ExchangeRateModel exchangeRateModel)
        {
            if (exchangeRateModel.Period == this.Period)
            {
                this.ExchangeRateList.Add(new ExchangeRateViewModel(exchangeRateModel));
            }
            else
            {
                throw new ArgumentException("Cannot add an ExchangeRate when the Period does not match");
            }
        }
    }
}
