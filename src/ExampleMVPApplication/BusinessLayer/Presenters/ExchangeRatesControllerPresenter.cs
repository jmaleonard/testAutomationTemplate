using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleMVPApplication.Interfaces;
using ExampleMVPApplication.Models;
using ExampleMVPApplication.ViewModel;

namespace ExampleMVPApplication.BusinessLayer.Presenters
{
    /// <summary>
    /// Exchange Rates Controller Presenter
    /// </summary>
    public class ExchangeRatesControllerPresenter : IExchangeRatesControllerView
    {
        private IExchangeRatesDao _exchangeRatesDao;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRatesControllerPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="exchangeRatesDao">The exchange rates DAO.</param>
        public ExchangeRatesControllerPresenter(IExchangeRatesControllerView view, IExchangeRatesDao exchangeRatesDao)
        {
            this._exchangeRatesDao = exchangeRatesDao;
        }

        /// <summary>
        /// Saves the exchange rates.
        /// </summary>
        /// <param name="exchangeRateList">The exchange rate list.</param>
        /// <returns></returns>
        public ExchangeRateControllerViewModel SaveExchangeRates(IList<ExchangeRateViewModel> exchangeRateList)
        {
            // Load all ExchangeRateModels for the periods in the exchangeRateList (should only be one period, but just in case...)
            var periodList = exchangeRateList.Select(e => (int)e.Period).Distinct().ToList();
            var exchangeRateModels = this._exchangeRatesDao.FindExchangeRatesForPeriods(periodList);

            // Update models with Rate data from the exchangeRateList for each combination of Period and CurrencyCode
            foreach (ExchangeRateViewModel viewModel in exchangeRateList)
            {
                var exchangeRateModel = exchangeRateModels.FirstOrDefault(e => e.Period == viewModel.Period && e.CurrencyCode == viewModel.CurrencyCode);

                if (exchangeRateModel == null)
                {
                    // Create a new ExchangeRateModel to be saved
                    exchangeRateModel = new ExchangeRateModel(viewModel.Period, viewModel.CurrencyCode);

                    exchangeRateModels.Add(exchangeRateModel);
                }

                exchangeRateModel.Rate = viewModel.Rate;
            }

            // Get the return ViewModel
            var returnViewModel = ExchangeRatesControllerPresenter.ValidateExchangeRateModels(exchangeRateModels);

            // If we have no errors, then pass the exchangeRateModels to the DAO for saving
            if (returnViewModel.Errors.Count == 0)
            {
                var errors = this._exchangeRatesDao.UpdateExchangeRates(exchangeRateModels);

                if (errors.Count != 0)
                {
                    // We should pass each error into our error list, but we are instead just re-writting the error
                    returnViewModel.Errors.Add("Could not persist the changes to the datastore.");
                }
            }

            return returnViewModel;
        }

        /// <summary>
        /// Validates the exchange rate models.
        /// </summary>
        /// <param name="exchangeRateModels">The exchange rate models.</param>
        /// <returns></returns>
        private static ExchangeRateControllerViewModel ValidateExchangeRateModels(IList<ExchangeRateModel> exchangeRateModels)
        {
            var exchangeRateControllerViewModel = new ExchangeRateControllerViewModel();

            // Validate and Save
            foreach (var exchangeRateModel in exchangeRateModels)
            {
                // Validate Period
                if (exchangeRateModel.Period <= 190001)
                {
                    exchangeRateControllerViewModel.Errors.Add("Period is invalid.");
                }

                // Validate Currency Code
                if (string.IsNullOrEmpty(exchangeRateModel.CurrencyCode))
                {
                    exchangeRateControllerViewModel.Errors.Add("Currency Code is required.");
                }

                // Validate Rate
                if (exchangeRateModel.Rate == 0M)
                {
                    exchangeRateControllerViewModel.Errors.Add("Exchange Rate cannot be zero.");
                }
                else if (exchangeRateModel.Rate < 0M)
                {
                    exchangeRateControllerViewModel.Errors.Add("Exchange Rate cannot be negative.");
                }
            }

            return exchangeRateControllerViewModel;
        }
    }
}
