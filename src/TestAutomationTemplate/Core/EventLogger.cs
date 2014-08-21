using System;
using TestAutomationTemplate.Enums;

namespace TestAutomationTemplate.Core
{
    /// <summary>
    /// Handles the logging throughout the application
    /// </summary>
    public class EventLogger
    {
        /// <summary>
        /// This logs a given event providing a string.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogEvent(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Logs a given event providing a string.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="eventType">Type of Event.</param>
        public static void LogEvent(string message, EventType eventType)
        {
            Console.WriteLine(eventType.ToString() + " " + message);
        }
    }
}
