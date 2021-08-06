using System;
using UnityEngine;

namespace ComboSystem
{
    public static class Log
    {
        public static LogLevel Level { get; private set; }
#if UNITY_EDITOR
            = LogLevel.Info;
#else
            = LogLevel.Warning;
#endif

        public static void ChangeLogLevel(LogLevel level)
        {
            Info($"Set log level to: {level}, from {Level}", true);
            Level = level;
        }

        public static void Verbose(object message)
        {
            if (Level < LogLevel.Verbose)
                return;

            Debug.Log($"VERB: {message}");
        }

        /// <summary>
        ///     Logs a message
        /// </summary>
        /// <remarks>
        ///     Use <paramref name="strict"/> to log no matter the log level (specific command/terminal info/feedback)
        /// </remarks>
        public static void Info(object message, bool strict = false)
        {
            if (Level < LogLevel.Info && !strict)
                return;

            Debug.Log($"INFO: {message}");
        }

        public static void Warning(object message)
        {
            if (Level < LogLevel.Warning)
                return;

            Debug.LogWarning($"WARN: {message}");
        }

        public static void Error(object message)
        {
            if (Level < LogLevel.Error)
                return;

            Debug.LogError($"ERROR: {message}");
        }

        public static void Exception(Exception exception)
        {
            // always log Exceptions
            Debug.LogError($"EXCE: {exception.GetType().Name}.\tMessage: {exception.Message}\nStackTrace: {exception.StackTrace}");
        }
    }
}