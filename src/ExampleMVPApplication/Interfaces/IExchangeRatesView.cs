#region Directives

using System.Collections.Generic;
using ExampleMVPApplication.ViewModel;

#endregion

namespace ExampleMVPApplication.Interfaces
{
    /// <summary>
    /// Exchange rates interfaces View
    /// </summary>
    public interface IExchangeRatesView : IView, IErrorView
    {
        /// <summary>
        /// Binds the exchange rates list.
        /// </summary>
        /// <param name="exchangeRates">The exchange rates.</param>
        void BindExchangeRatesList(IList<ExchangeRateListViewModel> exchangeRates);
    }
}
