#region Directives

using System.Collections.Generic;
using ExampleMVPApplication.ViewModel;

#endregion

namespace ExampleMVPApplication.Interfaces
{
    /// <summary>
    /// This is Interface for Web API Call
    /// </summary>
    public interface IExchangeRatesControllerView
    {
        /// <summary>
        /// Saves the exchange rates.
        /// </summary>
        /// <param name="exchangeRateList">The exchange rate list.</param>
        /// <returns></returns>
        ExchangeRateControllerViewModel SaveExchangeRates(IList<ExchangeRateViewModel> exchangeRateList);
    }
}
