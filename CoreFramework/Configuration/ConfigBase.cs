using CoreFramework.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CoreFramework.Configuration
{
    public class ConfigBase
    {
        protected readonly IDictionary<string, string> AppSettings;
        protected readonly IDictionary<string, string> EnvironmentVariables;
        protected IConfigurationSection ConfigurationSection;

        public ConfigBase() : this(ConfigurationManager.AppSettings.ToDictionary(), Environment.GetEnvironmentVariables().ToDictionary())
        {

        }

        public ConfigBase(IConfigurationSection configurationSection) : this(ConfigurationManager.AppSettings.ToDictionary(), Environment.GetEnvironmentVariables().ToDictionary(), configurationSection)
        {

        }

        public ConfigBase(IDictionary<string, string> appsettings, IDictionary<string, string> environmentVariables, IConfigurationSection configurationSection = null)
        {
            ConfigurationSection = configurationSection;
            AppSettings = AppSettings ?? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        protected string GetFromEnvironmentWithAppSettingFallback(string key) =>
            (EnvironmentVariables.ContainsKey(key) ? EnvironmentVariables[key] : null)
            ?? (AppSettings.ContainsKey(key) ? AppSettings[key] : null)
            ?? throw new ArgumentNullException(key, $"Environment Variable {key} has not been configured.");

        protected string GetValueFromAppSettings(string key) =>
            AppSettings.ContainsKey(key)
                ? AppSettings[key]
                : throw new ArgumentNullException(key, $"AppSetting {key} has not been configured.");

        protected string GetValueFromEnvironmentVariable(string key) =>
            (EnvironmentVariables.ContainsKey(key) ? EnvironmentVariables[key] : null)
            ?? throw new ArgumentNullException(key, $"Environment Variable {key} has not been configured.");
    }
}
