namespace TestAutomationTemplate.Enums
{
    /// <summary>
    /// When performing logging select an event type to make logs as informative as possible
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Information messages logged using this Event type 
        /// </summary>
        Info,

        /// <summary>
        /// Warning messages logged using this event type
        /// </summary>
        Warn,

        /// <summary>
        /// Error messages logged using this event type
        /// </summary>
        Error,

        /// <summary>
        /// Fatal messages logged using this event type
        /// </summary>
        Fatal,

        /// <summary>
        /// General messages logged using this event type
        /// </summary>
        None
    }
}
