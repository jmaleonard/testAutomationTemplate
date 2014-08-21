using NUnit.Framework;

namespace TestAutomationTemplate.CustomTestCategoryAttributes
{
    /// <summary>
    /// Custom test category to associate test to a particular bug
    /// </summary>
    public class BugId : CategoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BugId" /> class.
        /// </summary>
        /// <param name="bugNumber">The bug number.</param>
        public BugId(int bugNumber) 
            : base("Test for Bug #" + bugNumber) 
        { 
        }
    }
}
