using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Ambitus.Internals
{
	public static class Cache
	{
		private const string prefix = "Ambitus_";
		private static ObjectCache cache;
		private static List<string> keyHistory;

		static Cache()
		{
			cache = MemoryCache.Default;
			keyHistory = new List<string>();
		}

		public static T Get<T>()
		{
			return Get<T>(null);
		}

		public static T Get<T>(string subkey)
		{
			return (T)cache[FormatKey<T>(subkey)];
		}

		public static void Set<T>(T data)
		{
			Set<T>(null, data, false);
		}

		public static void Set<T>(T data, bool useShortCache)
		{
			Set<T>(null, data, useShortCache);
		}

		public static void Set<T>(string subkey, T data)
		{
			Set<T>(subkey, data, false);
		}

		public static void Set<T>(string subkey, T data, bool useShortCache)
		{
			if (data == null)
			{
				return;
			}
			int offsetMinutes = useShortCache ? ConfigSection.Settings.Caching.ShortCacheMinutes
				: ConfigSection.Settings.Caching.LongCacheMinutes;
			CacheItemPolicy policy = new CacheItemPolicy();
			policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(offsetMinutes);
			string key = FormatKey<T>(subkey);
			cache.Set(key, data, policy);
			if (!keyHistory.Contains(key))
			{
				keyHistory.Add(key);
			}
		}

		public static void Clear()
		{
			foreach (string key in keyHistory)
			{
				cache.Remove(key);
			}
			keyHistory.Clear();
		}

		private static string FormatKey<T>(string subkey)
		{
			return prefix + typeof(T).ToString()
					+ (String.IsNullOrWhiteSpace(subkey) ? String.Empty : ("_" + subkey));
		}
	}
}