using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CoreFramework.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection) =>
           collection.Keys.Cast<object>().ToDictionary(k => k.ToString(), v => collection[v.ToString()].ToString());
    }
}
