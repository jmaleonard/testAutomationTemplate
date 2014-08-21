using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationTemplate.Core;

namespace TestAutomationTemplate.Helpers
{
    /// <summary>
    /// This will be used as a steps helper. Will allow to remove the current pattern
    /// </summary>
    public static class Steps
    {
        #region Public Methods

        /// <summary>
        /// Stepses the helper.
        /// </summary>
        /// <param name="method">The method that you are calling on a class</param>
        /// <param name="theTestResult">The test result object. This is used to monitor the state of the test result object</param>
        /// <param name="args">The arguments that are passed to the method. This parameter can be left as null</param>
        /// <returns>The method if successful else ends the test if an exception is thrown</returns>
        public static object Invoke(Delegate method, TestResult theTestResult, params object[] args) 
        {
            try
            {
                return method.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                // The reason we are catching Exceptions here are as follows
                // WebDriver API throws a number of exceptions when the webpage behaves that is un inteded
                // This way it is impossible at the time to account for all of them when writing the UI test as 
                // the reason we are testing is to get errors
                // So over here we are catching the exception, printing it to a log,
                // which can examined in real time on Jenkins or after the test run executes. 
                // If you have a better way of handling this error please update this approach. 
                string exceptionMessage = ExceptionHelper.GetDetailMessage(e.GetBaseException());
                theTestResult.FailedResult(ExceptionHelper.FailedMethodDescription);
                return null;
            }
        }

        #endregion
    }
}
