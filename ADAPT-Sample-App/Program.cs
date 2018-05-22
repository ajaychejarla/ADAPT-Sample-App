﻿using System;
using AgGateway.ADAPT.ApplicationDataModel.ADM;
using AgGateway.ADAPT.PluginManager;

namespace ADAPT_Sample_App
{
    public class Program
    {
        private static Guid _applicationId = Guid.Empty;

        public static void Main(string[] args)
        {
            //This returns the \bin directory. 
            //ADAPT plugins do not need to be located in the application directory.
            //If you place the plugins in a separate folder, this variable should contain the path to that folder.
            var pluginLocation = AppDomain.CurrentDomain.BaseDirectory;
            var pluginManager = new PluginFactory(pluginLocation);

            var datacardLocation = SampleData.GetAdmDatacard();

            //The plugin factory automatically detects which plugin is able to load data from the given directory.
            //If the directory contains data in multiple formats (for example, ISOXml and 2630 data), this will return both plugins.
            //In that case, the ISO plugin would read the ISO data and the 2630 plugin would read the 2630 data.
            var supportedPlugins = pluginManager.GetSupportedPlugins(datacardLocation);
            foreach (var plugin in supportedPlugins)
            {
                InitializeWithLicense(plugin);
                var adaptDataModels = plugin.Import(datacardLocation);
                new AdaptDataModelProcessor().Process(adaptDataModels);
            }
        }

        //When you license the John Deere ADAPT plugins, you will receive a license file and an application id.
        //Initializing the plugin with your application id activates the license.
        //Keep the application id in a secure place. It is associated with your company.
        private static void InitializeWithLicense(IPlugin plugin)
        {
            plugin.Initialize(_applicationId.ToString());
        }
    }
}