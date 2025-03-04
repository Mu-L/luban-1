﻿using Luban.Plugin;
using NLog;

namespace PluginDemo;

public class PluginEntry : IPlugin
{
    private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
    
    public string Name => "Demo";
    
    public void Init(string jsonStr)
    {
        s_logger.Info($"plugin [{Name}] inits success");
    }
}