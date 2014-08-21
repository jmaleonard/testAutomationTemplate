using System;
using NUnit.Framework;

namespace TestAutomationTemplate.CustomTestCategoryAttributes
{
    /// <summary>
    /// Integration Test Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IntegrationAttribute : CategoryAttribute
    {
    }
}
