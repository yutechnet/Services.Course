using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public static class Resources<T> where T : IResource
    {
        public static string ContextName { get; private set; }

        static Resources()
        {
            ContextName = typeof(T).Name;
        }

        public static void Add(string name, T resource)
        {
            GetDictionary().Add(name, resource);
        }

        public static T Get(string name)
        {
            return GetDictionary()[name];
        }

        public static Dictionary<string, T> Get()
        {
            return GetDictionary();
        }

        private static Dictionary<string, T> GetDictionary()
        {
            Dictionary<string, T> dict;
            if (!ScenarioContext.Current.TryGetValue(ContextName, out dict))
            {
                dict = new Dictionary<string, T>();
                ScenarioContext.Current.Add(ContextName, dict);
            }

            return dict;
        }
    }
}
