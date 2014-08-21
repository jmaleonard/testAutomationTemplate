using System;
using NUnit.Framework;

namespace TestAutomationTemplate.CustomTestCategoryAttributes
{
    /// <summary>
    /// Functional UI Test Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FunctionalUIAttribute : CategoryAttribute
    {
    }
}