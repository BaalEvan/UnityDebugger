﻿using System;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Code added by koosemose between 11-30-2016 to add Channels and support for Unity Editor Window to access Channel Settings.



namespace UnityDebugger
{
    /// <summary>
    /// Enum describing logging level of Debugger. 
    /// Info > Warning > Error > Exception > None
    /// </summary>
    public enum LogLevel
    {
        None, 
        Exception,
        Error,
        Warning,
        Info
    }

    public class AssertException : Exception
    {
        public AssertException() { }
        public AssertException(string message) : base(message) { }
        public AssertException(string message, Exception innerException) : base(message, innerException) { }
    }
    
    public static class Debugger
    {
        public static bool exists = false;
        /// <summary>
        /// If false, Debugger does nothing. 
        /// Default value: Debug.isDebugBuild
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Log level of Debugger.
        /// Default value: LogLevel.Info
        /// </summary>
        public static LogLevel LogLevel { get; set; }

        public static bool DefaultState;

        public static Dictionary<string, bool> Channels { get; set; }

        static Debugger()
        {
            Enabled = Debug.isDebugBuild;
            LogLevel = LogLevel.Info;
            Channels = new Dictionary<string, bool>();
            exists = true;

            DefaultState = true;
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
            {
                ChannelSettingsSO channelSettings = Resources.Load<ChannelSettingsSO>("ChannelSettings");
                if (channelSettings != null)
                {
                    DefaultState = channelSettings.DefaultState;
                    foreach (string channelName in channelSettings.ChannelState.Keys.AsEnumerable())
                    {
                        Channels.Add(channelName, channelSettings.ChannelState[channelName]);
                    }
                }
            }
        }

        #region Asserts

        /// <summary>
        /// Throws AssertException if statement is false. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="statement">Statement to check</param>
        /// <param name="errorText">Name of statement</param>
        public static void Assert(bool statement, string errorText = "Statement is false!")
        {
            if (Enabled && !statement)
            {
                throw new AssertException(errorText);
            }
        }

        /// <summary>
        /// Asserts if obj is null. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="obj">Object to check.</param>
        /// <param name="name">Name of a object</param>
        /// <param name="context">Context for a check (usually containing class).</param>
        public static void AssertNotNull(object obj, string name, Object context)
        {
            if (Enabled && (obj == null || obj.Equals(null)))
            {
                Debug.LogError(name + " in object " + context.name + " ( " + context.GetType() + " ) is null!", context);
            }
        }

        #endregion

        #region Logging

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="message">Info to write.</param>
        /// <param name="context">Context.</param>
        public static void Log(string message, Object context = null)
        {
            if (LogLevelEnabled(LogLevel.Info))
            {
                Debug.Log(message, context);
            }
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="message">Info to write.</param>
        /// <param name="context">Context.</param>
        public static void Log(string channel, string message, Object context = null)
        {
            if (ChannelEnabled(channel))
            {
                Log(message, context);
            }
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void LogFormat(Object context, string channel, string format, params object[] args)
        {
            Log(channel, string.Format(format, args), context);
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void LogFormat(string channel, string format, params object[] args)
        {
            LogFormat(null, channel, string.Format(format, args));
        }

        /// <summary>
        /// Writes warning to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="message">Warning to write.</param>
        /// <param name="context">Context.</param>
        public static void LogWarning(string message, Object context = null)
        {
            if (LogLevelEnabled(LogLevel.Warning))
            {
                Debug.LogWarning(message, context);
            }
        }

        /// <summary>
        /// Writes warning to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="message">Warning to write.</param>
        /// <param name="context">Context.</param>
        public static void LogWarning(string channel, string message, Object context = null)
        {
            if (ChannelEnabled(channel))
            {
                LogWarning(message, context);
            }
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void LogWarningFormat(Object context, string channel, string format, params object[] args)
        {
            LogWarning(channel, string.Format(format, args), context);
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void LogWarningFormat(string channel, string format, params object[] args)
        {
            LogWarningFormat(null, channel, string.Format(format, args));
        }

        /// <summary>
        /// Writes error to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="message">Error to write.</param>
        /// <param name="context">Context.</param>
        public static void LogError(string message, Object context = null)
        {
            if (LogLevelEnabled(LogLevel.Error))
            {
                Debug.LogError(message, context);
            }
        }

        /// <summary>
        /// Writes error to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="message">Error to write.</param>
        /// <param name="context">Context.</param>
        public static void LogError(string channel, string message, Object context = null)
        {
            if (ChannelEnabled(channel))
            {
                LogError(message, context);
            }
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void LogErrorFormat(Object context, string channel, string format, params object[] args)
        {
            LogError(channel, string.Format(format, args), context);
        }

        /// <summary>
        /// Writes info to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void LogErrorFormat(string channel, string format, params object[] args)
        {
            LogErrorFormat(null, channel, string.Format(format, args));
        }

        /// <summary>
        /// Writes exception to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name="exception">Exception to write.</param>
        /// <param name="context">Context.</param>
        public static void LogException(Exception exception, Object context = null)
        {
            if (LogLevelEnabled(LogLevel.Exception))
            {
                Debug.LogException(exception, context);
            }
        }

        /// <summary>
        /// Writes exception to console. Works only when Debugger is Enabled.
        /// </summary>
        /// <param name = "channel">Channel to log to.</param>
        /// <param name="exception">Exception to write.</param>
        /// <param name="context">Context.</param>
        public static void LogException(string channel, Exception exception, Object context = null)
        {
            if (ChannelEnabled(channel))
            {
                LogException(exception, context);
            }
        }

        #endregion

        #region HelperFunctions

        private static bool LogLevelEnabled(LogLevel logLevel)
        {
            return Enabled && LogLevel >= logLevel;
        }

        private static bool ChannelEnabled(string channel)
        {
            if (Channels.ContainsKey(channel))
            {
                return Channels[channel];
            }
            else
            {
                bool channelSetting = DefaultState;
                Channels.Add(channel, channelSetting);
                return channelSetting;
            }
        }
        #endregion
    }
}
