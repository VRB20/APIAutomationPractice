using CoreFramework.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.IO.Abstractions;

namespace CoreFramework.Helpers
{
    public class AppHelperConfig : ConfigBase, IAppConfig
    {
        private static readonly string AppUriKey = "URI";
        private static readonly string AppUserNameKey = "UserName";
        private static readonly string AppPasswordKey = "Password";

        public AppHelperConfig() =>
            ConfigurationSection = new ConfigurationBuilder()
                .SetBasePath(new FileSystem().Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", true, true)
                .Build()
                .GetSection("Appsettings");

        public Uri uri => new Uri(GetValueFromAppSettings(AppUriKey));

        public string AuthToken
        {
            get
            {
                //TODO: read from config
                var username = "vrb";
                var password = GetValueFromEnvironmentVariable(AppPasswordKey);

                return Convert.ToBase64String(Encoding.Default.GetBytes($"{username}:{password}"));
            }
        }
    }

    public interface IAppConfig
    {
        Uri uri { get; }
        string AuthToken { get; }
    }
}
