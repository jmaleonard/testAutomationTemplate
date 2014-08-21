using System;
using NUnit.Framework;

namespace TestAutomationTemplate.CustomTestCategoryAttributes
{
    /// <summary>
    /// Smoke Test Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SmokeTestAttribute : CategoryAttribute
    {
    }
}
