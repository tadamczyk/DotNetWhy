﻿namespace DotNetWhy.Loggers;

public interface ILogger
{
    LoggerConfiguration Configuration { get; }
    void Log(string text = "", Color? color = null);
    void LogLine(string text = "", Color? color = null);
}