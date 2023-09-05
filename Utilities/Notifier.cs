using Gtpx.ModelSync.Services.Models;
using Serilog;
using System;
using System.Windows;
using Gtpx.ModelSync.DataModel.Enums;
using System.Windows.Documents;
using System.Collections.Generic;

namespace Gtpx.ModelSync.CAD.UI
{
    public class NotificationEventArgs
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public List<KeyValuePair<string,long>> TemplateIdRunTimeList { get; set; }
        public long Progress { get; set; }
        public long Total { get; set; }
    }

    public class Notifier
    {
        private readonly LocalFileContext localFileContext;
        private const string debugPrefix = "DEBUG:";
        private const string errorPrefix = "ERROR:";
        private const string informationPrefix = "INFORMATION:";
        private readonly ILogger logger;
        private const string warningPrefix = "WARNING:";
        private const string silentPrefix = "LOGSILENT:";
        public event EventHandler<NotificationEventArgs> NotificationReceived;
        public event EventHandler<NotificationEventArgs> StatsReceived;

        public ILogger Logger => logger;

        public Notifier(LocalFileContext localFileContext,
                        ILogger logger)
        {
            this.localFileContext = localFileContext;
            this.logger = logger;
            IsNotifyWindowLoaded = false;
        }

        public void Stats(List<KeyValuePair<string, long>> templateIdSeconds, long progress, long total)
        {
            StatsReceived?.Invoke(this, new NotificationEventArgs()
            {
                TemplateIdRunTimeList = templateIdSeconds
            });
        }

        public void Error(string message,
                          Exception ex = null,
                          bool allowMessageBox = true)
        {
            if (!localFileContext.IsSilentMode)
            {
                if (IsNotifyWindowLoaded)
                {
                    // A dialog with the Notifcation window is open, can send notications
                    NotificationReceived?.Invoke(this, new NotificationEventArgs()
                    {
                        LogLevel = LogLevel.Error,
                        Message = message
                    });
                }
                else if (allowMessageBox)
                {
                    // A dialog with the Notification window is not open, bring up a message box
                    MessageBox.Show(message,
                                    $"GTP STRATUS [v{GetType().Assembly.GetName().Version}-CodeKill]", 
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            logger.Error(message, ex);
        }

        public void Information(string message)
        {
            if (!localFileContext.IsSilentMode)
            {
                NotificationReceived?.Invoke(this, new NotificationEventArgs()
                {
                    LogLevel = LogLevel.Information,
                    Message = message
                });
            }
            logger.Information(message);
        }

        public bool IsNotifyWindowLoaded { get; set; }

        public void LogSilent(string message)
        {
            // Just log to file and not to dialog
            logger.Information(message);
        }

        public void ProcessOutputIntoNotifications(string processOutput)
        {
            var outputLines = processOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var outputLine in outputLines)
            {
                if (outputLine.StartsWith(debugPrefix))
                {
                    logger.Debug(outputLine.Remove(0, debugPrefix.Length));
                }
                else if (outputLine.StartsWith(errorPrefix))
                {
                    var msg = outputLine;

                    // Everything between 'ERROR:' and '!!' is the exception message
                    var index = outputLine.IndexOf("!!");
                    if (index >= 0)
                    {
                        // Replace double pipe strings in the error string with a new line character.  
                        // This will cause the exception string from the console to be formatted properly.  
                        Error(outputLine.Substring(0, index).Remove(0, errorPrefix.Length));
                        msg = outputLine.Substring(index + 2).Replace("||", Environment.NewLine);
                    }
                    else
                    {
                        Error(outputLine.Remove(0, errorPrefix.Length));
                        msg = outputLine.Replace("||", Environment.NewLine);
                    }
                    throw new Exception(msg);
                }
                else if (outputLine.StartsWith(informationPrefix))
                {
                    Information(outputLine.Remove(0, informationPrefix.Length));
                }
                else if (outputLine.StartsWith(warningPrefix))
                {
                    Warning(outputLine.Remove(0, warningPrefix.Length));
                }
                else if (outputLine.StartsWith(silentPrefix))
                {
                    LogSilent(outputLine.Remove(0, silentPrefix.Length));
                }
            }
        }

        public void Warning(string message)
        {
            if (!localFileContext.IsSilentMode)
            {
                NotificationReceived?.Invoke(this, new NotificationEventArgs()
                {
                    LogLevel = LogLevel.Warning,
                    Message = message
                });
            }
            logger.Warning(message);
        }
    }
}
