namespace ExampleMVPApplication.Interfaces
{
    /// <summary>
    /// IErrorView: Any view that supports displaying errors should add this
    /// </summary>
    public interface IErrorView
    {
        /// <summary>
        /// Adds an error message to the view.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        void AddError(string errorMessage);
    }
}
