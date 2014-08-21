using System;
using NUnit.Framework;

namespace TestAutomationTemplate.CustomTestCategoryAttributes
{
    /// <summary>
    /// Unit Test Attribute
    /// </summary>
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class UnitAttribute : CategoryAttribute
    {
    }
}
