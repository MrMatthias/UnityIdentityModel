// Copyright (c) Duende Software. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace IdentityModel.Client
{


	/// <summary>
	/// Extensions for JObject
	/// </summary>
	public static class JsonElementExtensions
	{

		/// <summary>
		/// Converts a JSON claims object to a list of Claim
		/// </summary>
		/// <param name="json">The json.</param>
		/// <param name="issuer">Optional issuer name to add to claims.</param>
		/// <param name="excludeKeys">Claims that should be excluded.</param>
		/// <returns></returns>

		public static IEnumerable<Claim> ToClaims(this JToken json, string? issuer = null,
			params string[] excludeKeys)
		{
			var claims = new List<Claim>();
			var excludeList = excludeKeys.ToList();
			
			if(json.Type != JTokenType.Object)
			{
				return claims;
			}

			foreach (var x in json as JObject)
			{
				if (excludeList.Contains(x.Key)) continue;

				if (x.Value.Type == JTokenType.Array)
				{
					JArray jArray = (JArray)x.Value;
					foreach (var item in jArray)
					{
						claims.Add(new Claim(x.Key, Stringify(item), ClaimValueTypes.String, issuer));
					}
				}
				else
				{
					claims.Add(new Claim(x.Key, Stringify(x.Value), ClaimValueTypes.String, issuer));
				}
			}

			return claims;
		}
		
		public static bool HasValue(this JToken json)
		{
			return (json != null && json.Type != JTokenType.Null);
		}

		private static string Stringify(JToken item)
		{
			// String is special because item.ToString(Formatting.None) will result in "/"string/"". The quotes will be added.
			// Boolean needs item.ToString otherwise 'true' => 'True'
			var value = item.Type == JTokenType.String
				? item.ToString()
				: item.ToString(Newtonsoft.Json.Formatting.None);

			return value;
		}

		/// <summary>
		/// Tries to get a value from a JObject
		/// </summary>
		/// <param name="json">The json.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static JToken TryGetValue(this JToken json, string name)
		{
			if (json.Type == JTokenType.Undefined)
			{
				return default;
			}
			
			if(json.Type == JTokenType.Object)
			{
				return json[name];
			}
			
			return default;
		}

		/// <summary>
		/// Tries to get an int from a JObject
		/// </summary>
		/// <param name="json">The json.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static int? TryGetInt(this JToken json, string name)
		{
			var value = json.TryGetString(name);

			if (value != null)
			{
				if (int.TryParse(value, out int intValue))
				{
					return intValue;
				}
			}

			return null;
		}

		/// <summary>
		/// Tries to get a string from a JObject
		/// </summary>
		/// <param name="json">The json.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static string? TryGetString(this JToken json, string name)
		{
			JToken value = json.TryGetValue(name);
			if (value == null)
			{
				return null;
			}
			return value.Type == JTokenType.Undefined ? null : value.ToString();
		}

		/// <summary>
		/// Tries to get a boolean from a JObject
		/// </summary>
		/// <param name="json">The json.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static bool? TryGetBoolean(this JToken json, string name)
		{
			var value = json.TryGetString(name);

			if (bool.TryParse(value, out bool result))
			{
				return result;
			}

			return null;
		}

		/// <summary>
		/// Tries to get a string array from a JObject
		/// </summary>
		/// <param name="json">The json.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static IEnumerable<string> TryGetStringArray(this JToken json, string name)
		{
			var values = new List<string>();

			var array = json.TryGetValue(name);
			if(array == null)
			{
				return values;
			}
			if (array.Type == JTokenType.Array)
			{
				JArray jArray = (JArray)array;
				foreach (var item in jArray)
				{
					values.Add(item.ToString());
				}
			}

			return values;
		}

	}


}