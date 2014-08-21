using System;
using NUnit.Framework;
using TestAutomationTemplate.Core;
using TestAutomationTemplate.CustomTestCategoryAttributes;

namespace TestAutomationTemplate.SmokeTest
{
    /// <summary>
    /// Example Test
    /// </summary>
    public class ExampleTest
    {
        /// <summary>
        /// Examples the test.
        /// </summary>
        [Test, BugId(1223)]
        public void ExampleSmokeTest()
        {
            SmokeTestResult cur = new SmokeTestResult("This", "This", "This", "This", Enums.TestStatus.Failed, "This", DateTime.Now, DateTime.Now);
            cur.TestName = "This is a crazy test";
            cur.PassedTest();
        }

        /// <summary>
        /// Examples the smoke test1.
        /// </summary>
        [Test, BugId(1223), Integration]
        public void ExampleSmokeTest1()
        {
            SmokeTestResult cur = new SmokeTestResult("This", "This", "This", "This", Enums.TestStatus.Failed, "This", DateTime.Now, DateTime.Now);
            cur.TestName = "This is a crazy test";
            cur.PassedTest();
        }

        /// <summary>
        /// Examples the smoke test2.
        /// </summary>
        [Test, BugId(1223), SmokeTest]
        public void ExampleSmokeTest2()
        {
            SmokeTestResult cur = new SmokeTestResult("This", "This", "This", "This", Enums.TestStatus.Failed, "This", DateTime.Now, DateTime.Now);
            cur.TestName = "This is a crazy test";
            cur.PassedTest();
        }

        /// <summary>
        /// Examples the smoke test3.
        /// </summary>
        [Test, BugId(1223), FunctionalUI]
        public void ExampleSmokeTest3()
        {
            SmokeTestResult cur = new SmokeTestResult("This", "This", "This", "This", Enums.TestStatus.Failed, "This", DateTime.Now, DateTime.Now);
            cur.TestName = "This is a crazy test";
            cur.PassedTest();
        }

        /// <summary>
        /// Chromes the smoke test.
        /// </summary>
        [Test]
        [Category("Chrome")]
        public void ChromeSmokeTest()
        { 
        }

        /// <summary>
        /// Ies the smoke test.
        /// </summary>
        [Test]
        [Category("IE")]
        public void IESmokeTest()
        {
        }

        /// <summary>
        /// Fires the fox smoke test.
        /// </summary>
        [Test]
        [Category("FireFox")]
        public void FireFoxSmokeTest()
        {
        }
    }
}
