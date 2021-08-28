using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace CoreFramework.Utils
{
    public class JsonConfigProvider
    {
        private const string FilePath = @"..\..\..\TestSettings.json";
        private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FilePath);

        private static T Load<T>(string sectionName) =>
            JObject.Parse(File.ReadAllText(SettingsPath)).SelectToken(sectionName).ToObject<T>();
    }
}
