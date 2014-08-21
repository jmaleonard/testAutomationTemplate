using System.Collections.Generic;
using ExampleMVPApplication.Models;
using ExampleMVPApplication.Structs;

namespace ExampleMVPApplication.Interfaces
{
    /// <summary>
    /// Dao interface. For the sake of this demo we would not be creating an implementation of the dao
    /// </summary>
    public interface IExchangeRatesDao
    {
        /// <summary>
        /// Finds all exchange rates.
        /// </summary>
        /// <returns></returns>
        IList<ExchangeRateModel> FindAllExchangeRates();

        /// <summary>
        /// Finds the exchange rates for periods.
        /// </summary>
        /// <param name="periodList">The period list.</param>
        /// <returns></returns>
        IList<ExchangeRateModel> FindExchangeRatesForPeriods(IList<int> periodList);

        /// <summary>
        /// Finds the latest cash flow period.
        /// </summary>
        /// <returns></returns>
        Period FindLatestCashFlowPeriod();

        /// <summary>
        /// Finds the latest co investor period.
        /// </summary>
        /// <returns></returns>
        Period FindLatestCoInvestorPeriod();

        /// <summary>
        /// Finds the latest fund info period.
        /// </summary>
        /// <returns></returns>
        Period FindLatestFundInfoPeriod();

        /// <summary>
        /// Finds the latest exchange rate period.
        /// </summary>
        /// <returns></returns>
        Period FindLatestExchangeRatePeriod();

        /// <summary>
        /// Updates the exchange rates.
        /// </summary>
        /// <param name="exchangeRateModels">The exchange rate models.</param>
        /// <returns></returns>
        IList<string> UpdateExchangeRates(IList<ExchangeRateModel> exchangeRateModels);

        /// <summary>
        /// Projects the exchange rates.
        /// </summary>
        /// <param name="startPeriod">The start period.</param>
        /// <param name="endPeriod">The end period.</param>
        /// <returns></returns>
        IList<string> ProjectExchangeRates(int startPeriod, int endPeriod);
    }
}
