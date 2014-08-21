using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleMVPApplication.Interfaces;
using ExampleMVPApplication.Structs;
using ExampleMVPApplication.ViewModel;

namespace ExampleMVPApplication.BusinessLayer.Presenters
{
    /// <summary>
    /// Presenter for IExchangeRatesView
    /// </summary>
    public class ExchangeRatesPresenter : GenericPresenterBase<IExchangeRatesView>
    {
         private IExchangeRatesDao _exchangeRatesDao;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRatesPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="exchangeRatesDao">The exchange rates DAO.</param>
        public ExchangeRatesPresenter(IExchangeRatesView view, IExchangeRatesDao exchangeRatesDao)
             : base(view)
        {
            this._exchangeRatesDao = exchangeRatesDao;
        }

        /// <summary>
        /// Called when the view is ready.
        /// </summary>
        public void ViewReady()
        {
            // Ensure that we have generated exchange rate items until the latest CashFlow, Co-investor, FundInfo or 6 years into the future
            var cashFlowPeriod = this._exchangeRatesDao.FindLatestCashFlowPeriod();
            var CoInvestorPeriod = this._exchangeRatesDao.FindLatestCoInvestorPeriod();
            var fundInfoPeriod = this._exchangeRatesDao.FindLatestFundInfoPeriod();
            var projectedPeriod = Period.GetQuarterPeriod(DateTime.Now.AddYears(6));

            var latestPeriod = new Period[] { cashFlowPeriod, CoInvestorPeriod, fundInfoPeriod, projectedPeriod }.Max();

            var latestExchangeRatePeriod = this._exchangeRatesDao.FindLatestExchangeRatePeriod();
            if (latestExchangeRatePeriod < latestPeriod)
            {
                // We need to create projection exchangeRates from the latestExchangeRatePeriod until the latestPeriod
                var errors = this._exchangeRatesDao.ProjectExchangeRates((int)latestExchangeRatePeriod, (int)latestPeriod);
                
                // Report any errors to the user...
                if (errors != null && errors.Count > 0)
                {
                    this.View.AddError("Could not create projected Exchange Rates - please contact an Administrator.");
                }
            }
            
            var exchangeRateListItems = this.GetExchangeRateListItems();
           
            this.View.BindExchangeRatesList(exchangeRateListItems);
        }

        /// <summary>
        /// Gets the exchange rate list items.
        /// </summary>
        /// <returns></returns>
        private IList<ExchangeRateListViewModel> GetExchangeRateListItems()
        {
            var exchangeRateListItems = new List<ExchangeRateListViewModel>();

            // Loop through each ExchangeRateModel and create the associated ExchangeRateViewModel
            var exchangeRateModels = this._exchangeRatesDao.FindAllExchangeRates();
            foreach (var exchangeRateModel in exchangeRateModels)
            {
                // Check if an ExchangeRateListViewModel exists for this exchangeRateModel's period
                ExchangeRateListViewModel exchangeRateListItem = exchangeRateListItems.FirstOrDefault(e => e.Period == exchangeRateModel.Period);

                // If not, then create a new ExchangeRateListViewModel for this period, and add it to the exchangeRateListItems
                if (exchangeRateListItem == null)
                {
                    exchangeRateListItem = new ExchangeRateListViewModel(exchangeRateModel.Period);

                    exchangeRateListItems.Add(exchangeRateListItem);
                }

                exchangeRateListItem.AddExchangeRate(exchangeRateModel);
            }

            return exchangeRateListItems.OrderBy(e => e.Period)
                                        .ToList();
        }
    }    
}
