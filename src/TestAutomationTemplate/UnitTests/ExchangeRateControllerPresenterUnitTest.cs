#region Directives

using System.Collections.Generic;
using System.Linq;
using ExampleMVPApplication.BusinessLayer.Presenters;
using ExampleMVPApplication.Interfaces;
using ExampleMVPApplication.Models;
using ExampleMVPApplication.Structs;
using ExampleMVPApplication.ViewModel;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TestAutomationTemplate.CustomTestCategoryAttributes;

#endregion

namespace TestAutomationTemplate.UnitTests
{
    /// <summary>
    /// The exchange controler presenter unit tests
    /// </summary>
    [TestFixture]
   public class ExchangeRateControllerPresenterUnitTest
   {
       #region Fields

        private IExchangeRatesDao _dao;
        private IExchangeRatesControllerView _view;
        private ExchangeRatesControllerPresenter _presenter;

       #endregion

       #region Setup

        /// <summary>
        /// Test setup. Will run once before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this._dao = Substitute.For<IExchangeRatesDao>();
            this._view = Substitute.For<IExchangeRatesControllerView>();
            this._presenter = new ExchangeRatesControllerPresenter(this._view, this._dao);
        }

        #endregion

        #region Tests

        /// <summary>
        /// This test will be called on the exchangeRates controller presenter. 
        /// When doing this test we will be using the NSubstitute Asserts (.Received)
        /// </summary>
        [Test, Unit]
        public void ExchangeRatesControllerPresenter_SaveExchangeRates_HitsDao_Using_NSubstituteAsserts()
        {
            //// Arrange ///
            var mockedExchangeRateViewModels = GenerateMockedExchangeRateViewModels(GenerateMockedExchangeRateModel(1));
            this._dao.FindExchangeRatesForPeriods(Arg.Any<IList<int>>()).Returns(GenerateMockedExchangeRateModel(1));

            //// Act ////
            this._presenter.SaveExchangeRates(mockedExchangeRateViewModels);

           //// Assert ////
            this._dao.Received().UpdateExchangeRates(Arg.Is<IList<ExchangeRateModel>>(x => x[0].Rate == 1));            
        }

        /// <summary>
        /// This test will be called on the exchangeRates controller presenter. 
        /// When doing this test we will be using the NUnit Asserts (.Assert)
        /// </summary>
        [Test, Unit]
        public void ExchangeRatesControllerPresenter_SaveExchangeRates_HitsDao_Using_NUnitAsserts()
        {
            //// Arrange ///
            var mockedExchangeRateViewModels = GenerateMockedExchangeRateViewModels(GenerateMockedExchangeRateModel(1));
            this._dao.FindExchangeRatesForPeriods(Arg.Any<IList<int>>()).Returns(GenerateMockedExchangeRateModel(1));

            List<IList<ExchangeRateModel>> updateExchangeArguments = new List<IList<ExchangeRateModel>>();
            this._dao.When(x => x.UpdateExchangeRates(Arg.Any<IList<ExchangeRateModel>>()))
                     .Do(x => updateExchangeArguments = x.Args().Cast<IList<ExchangeRateModel>>().ToList());

            //// Act ////
            this._presenter.SaveExchangeRates(mockedExchangeRateViewModels);

            //// Assert ////
            Assert.IsTrue(updateExchangeArguments.First().First().Rate == 1);
        }

        /// <summary>
        /// Exchanges the rates controller presenter_ save exchange rates_ hits dao_ using_ fluent assertions.
        /// </summary>
        [Test, Unit]
        public void ExchangeRatesControllerPresenter_SaveExchangeRates_HitsDao_Using_FluentAssertions()
        {
            //// Arrange ///
            var mockedExchangeRateViewModels = GenerateMockedExchangeRateViewModels(GenerateMockedExchangeRateModel(1));
            this._dao.FindExchangeRatesForPeriods(Arg.Any<IList<int>>()).Returns(GenerateMockedExchangeRateModel(1));

            List<IList<ExchangeRateModel>> updateExchangeArguments = new List<IList<ExchangeRateModel>>();
            this._dao.When(x => x.UpdateExchangeRates(Arg.Any<IList<ExchangeRateModel>>()))
                     .Do(x => updateExchangeArguments = x.Args().Cast<IList<ExchangeRateModel>>().ToList());

            //// Act ////
            this._presenter.SaveExchangeRates(mockedExchangeRateViewModels);

            //// Assert ////

            // This will fail as the one has an Exchage rate Id ////
            updateExchangeArguments.First().First().ShouldHave()
                                                   .AllProperties()
                                                   .EqualTo(mockedExchangeRateViewModels.First());

            //// This assert will pass and here we are able to compare all properties of the object ////

            updateExchangeArguments.First().First().ShouldHave()
                                                  .AllPropertiesBut(x => x.ExchangeRateId)
                                                  .EqualTo(mockedExchangeRateViewModels.First());
        }

        /// <summary>
        /// Exchanges the rates controller presenter_ save exchange rates_ returns_ error_for_ invalid_ exchage rates model.
        /// </summary>
        [Test, Unit]
        public void ExchangeRatesControllerPresenter_SaveExchangeRates_Returns_Error_for_Invalid_ExchageRatesModel()
        {
            //// Arragne invalid model ////

            var inValidModels = GenerateMockedExchangeRateViewModels(Generate_InValid_MockedExchangeRateModel(1));

            this._dao.FindExchangeRatesForPeriods(Arg.Any<IList<int>>()).Returns(Generate_InValid_MockedExchangeRateModel(1));

            //// Act ////
            var errors = this._presenter.SaveExchangeRates(inValidModels);

            //// Here we need to assert that the view model that is returned contains errors

            errors.Errors.Should().Contain("Currency Code is required.", "Exchange Rate cannot be zero.");
        }

       #endregion

        #region DataSetup

        /// <summary>
        /// Generates the mocked exchange rate model.
        /// </summary>
        /// <param name="numberToGenerate">The number to generate.</param>
        /// <returns></returns>
        private static IList<ExchangeRateModel> GenerateMockedExchangeRateModel(int numberToGenerate)
        {
            var uniqueIDGenerator = new SequentialGenerator<int> { Direction = GeneratorDirection.Ascending, Increment = 1 };
            uniqueIDGenerator.StartingWith(1);

            var RateGenerator = new SequentialGenerator<decimal> { Direction = GeneratorDirection.Ascending, Increment = 0.5M };
            RateGenerator.StartingWith(1);

            var dategeneratorForExchangeRates = new SequentialGenerator<int> { Direction = GeneratorDirection.Ascending, Increment = 1 };
            dategeneratorForExchangeRates.StartingWith(1980);

            var mockedExchangeRateList = Builder<ExchangeRateModel>.CreateListOfSize(numberToGenerate).All().WithConstructor(() => new ExchangeRateModel(uniqueIDGenerator.Generate()))
                                                                                              .With(x => x.Rate = RateGenerator.Generate())
                                                                                              .With(x => x.Period = new Period(dategeneratorForExchangeRates.Generate(), 4))
                                                                                              .With(x => x.CurrencyCode = "USD")
                                                                                              .Build();
            return mockedExchangeRateList;
        }

        /// <summary>
        /// Generates the in valid_ mocked exchange rate view models.
        /// </summary>
        /// <param name="numberToGenerate">The number to generate.</param>
        /// <returns></returns>
        private static IList<ExchangeRateModel> Generate_InValid_MockedExchangeRateModel(int numberToGenerate)
        {
            var uniqueIDGenerator = new SequentialGenerator<int> { Direction = GeneratorDirection.Ascending, Increment = 1 };
            uniqueIDGenerator.StartingWith(1);

            var RateGenerator = new SequentialGenerator<decimal> { Direction = GeneratorDirection.Ascending, Increment = 0.5M };
            RateGenerator.StartingWith(1);

            var dategeneratorForExchangeRates = new SequentialGenerator<int> { Direction = GeneratorDirection.Ascending, Increment = 1 };
            dategeneratorForExchangeRates.StartingWith(1980);

            var mockedExchangeRateList = Builder<ExchangeRateModel>.CreateListOfSize(numberToGenerate).All().WithConstructor(() => new ExchangeRateModel(uniqueIDGenerator.Generate()))
                                                                                              .With(x => x.Rate = RateGenerator.Generate())
                                                                                              .With(x => x.Period = new Period(dategeneratorForExchangeRates.Generate(), 4))
                                                                                              .With(x => x.CurrencyCode = string.Empty)
                                                                                              .With(x => x.Rate = 0)
                                                                                              .Build();

            return mockedExchangeRateList;
        }

        /// <summary>
        /// Generates the mocked exchange rate view models.
        /// </summary>
        /// <param name="listOfExchangeRateModels">The list of exchange rate models.</param>
        /// <returns></returns>
        private static IList<ExchangeRateViewModel> GenerateMockedExchangeRateViewModels(IList<ExchangeRateModel> listOfExchangeRateModels)
        {
            var mockedExchangeRateList = listOfExchangeRateModels;
            var mockedExchangeRateViewModels = new List<ExchangeRateViewModel>();

            for (int x = 0; x < mockedExchangeRateList.Count; x++)
            {
                var currentExchangeRateModel = new ExchangeRateViewModel(mockedExchangeRateList[x]);
                mockedExchangeRateViewModels.Add(currentExchangeRateModel);
            }

            return mockedExchangeRateViewModels;
        }

        /// <summary>
        /// Generates the latest exchange rate period.
        /// </summary>
        /// <returns></returns>
        private static Period GenerateLatestExchangeRatePeriod()
        {
            var mockedPeriod = Builder<Period>.CreateNew().WithConstructor(() => new Period("201001")).Build();

            return mockedPeriod;
        }

        #endregion
   } 
}
