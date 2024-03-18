using BlazorWinFormsGenericToolKit.Models;
using Microsoft.VisualBasic.Logging;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace BlazorWinFormsGenericToolKit.Core.Extensions
{
	public static class JsonExtensions
	{

		public static bool ToBoolean(this JsonNode obj)
		{
			if (obj == null) return false;
			bool result = false;
			bool.TryParse(obj.ToString(), out result);
			return result;
		}

		public static int ToInt(this JsonNode obj)
		{
			int result = 0;
			int.TryParse(obj.ToString(), out result);
			return result;
		}

		public static decimal ToDecimal(this JsonNode obj)
		{
			decimal result = 0;
			decimal.TryParse(obj.ToString(), out result);
			return result;
		}

		public static float ToFloat(this JsonNode obj)
		{
			float result = 0;
			float.TryParse(obj.ToString(), out result);
			return result;
		}

		///// <summary>
		///// 判斷是否為空,如果是回傳預設值,預設為字串
		///// </summary>
		///// <param name="node"></param>
		///// <param name="key"></param>
		///// <returns></returns>
		public static JsonNode ToJsonNode(this JsonNode node, string key)
		{
			return node?[key]?.CopyNode() ?? (JsonNode)string.Empty.ToString();
		}

		///// <summary>
		///// 判斷是否為空,如果是回傳預設值,預設為字串
		///// </summary>
		///// <param name="node"></param>
		///// <param name="key"></param>
		///// <returns></returns>
		public static JsonObject ToJsonObject(this JsonNode node, string key)
		{
			return node?[key]?.CopyNode()?.AsObject() ?? new JsonObject();
		}

		/// <summary>
		/// 轉型字串
		/// </summary>
		/// <typeparam name="T">轉換的類型</typeparam>
		/// <param name="node"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string ToValue(this JsonNode node, string key)
		{
			return node?[key]?.ToString() ?? "";
		}

		/// <summary>
		/// 動態轉型
		/// </summary>
		/// <typeparam name="T">轉換的類型</typeparam>
		/// <param name="node"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static dynamic ToValue<T>(this JsonNode node, string key)
		{
			if (typeof(T) == typeof(string))
			{
				return node?[key]?.ToString() ?? "";
			}

			if (typeof(T) == typeof(bool))
			{
				return node?[key]?.ToBoolean() ?? false;
			}

			if (typeof(T) == typeof(int))
			{
				return node?[key]?.ToInt() ?? 0;
			}

			if (typeof(T) == typeof(float))
			{
				return node?[key]?.ToFloat() ?? 0;
			}

			if (typeof(T) == typeof(decimal))
			{
				return node?[key]?.ToDecimal() ?? 0;
			}

			if (node?[key] != null || typeof(T).IsClass)
			{
				try
				{
					T result = JsonSerializer.Deserialize<T>(node?[key]);
					return result;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return default(T);
		}

		public static List<string> ToStringList(this JsonNode arr)
		{
			List<string> result = new List<string>();
			foreach (var item in arr.AsArray())
			{
				result.Add(item.ToString());
			}
			return result;
		}

		public static JsonArray ToJsonArray(this IEnumerable<string> list)
		{
			JsonArray array = new JsonArray();
			foreach (var item in list)
			{
				array.Add(item);
			}
			return array;
		}

		public static JsonArray ToJsonArray(this IEnumerable<JsonNode> list)
		{
			JsonArray array = new JsonArray();
			foreach (var item in list)
			{
				array.Add(item.CopyNode());
			}
			return array;
		}


		public static string GetJsonValue(this string jsonStr, string key)
		{
			try
			{
				JsonNode? jsonNode = JsonNode.Parse(jsonStr);
				return jsonNode.ToValue(key);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// 複製新的JsonNode
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static JsonNode? CopyNode(this JsonNode obj)
		{
			try
			{
				if (obj != null)
				{
					return JsonNode.Parse(obj.ToJsonString());
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return new JsonObject();
		}

		/// <summary>
		/// 判斷欄位是陣列還是物件
		/// </summary>
		/// <param name="node"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static JsonArray MapJsonArray(this JsonNode node, Action<JsonNode?, JsonNode?> mapAction)
		{
			JsonArray result = new JsonArray();
			if (node != null && node is JsonArray)
			{
				result = new JsonArray();
				foreach (JsonNode data in node?.AsArray())
				{
					var newData = new JsonObject();
					mapAction(data, newData);
					(result as JsonArray)?.Add(newData);
				}
			}
			else if (node != null && node is JsonObject)
			{
				JsonObject newData = new JsonObject();
				mapAction(node, newData);
				result.Add(newData);
			}
			return result;
		}

		/// <summary>
		/// 轉換JsonArray
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static JsonArray ToJsonArray(this JsonNode node)
		{
			JsonArray result = new JsonArray();
			if (node != null && node is JsonArray)
			{
				foreach (JsonNode data in node?.AsArray())
				{
					if (data != null)
					{
						(result as JsonArray)?.Add(data.CopyNode());
					}
				}
			}
			else if (node != null && node is JsonObject)
			{
				result.Add(node.CopyNode());
			}
			return result;
		}

		/// <summary>
		/// 轉換JsonArray
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static JsonArray ToJsonArray(this JsonNode node, string name)
		{
			JsonArray result = new JsonArray();
			var curNode = node.ToJsonNode(name);
			if (curNode != null && curNode is JsonArray)
			{
				foreach (JsonNode data in curNode?.AsArray())
				{
					if (data != null)
					{
						(result as JsonArray)?.Add(data.CopyNode());
					}
				}
			}
			else if (curNode != null && curNode is JsonObject)
			{
				result.Add(curNode.CopyNode());
			}
			return result;
		}

		public static string ToHtmlString(this string builder)
		{
			builder = builder.Replace("&", "&amp;")
							 .Replace("<", "&lt;")
							 .Replace(">", "&gt;")
							 //.Replace("\"", "&quot;")

							 .ToString();
			var breakPattern = $@"(\n)|(\r\n)";
			//var breakPattern = $@"(\n+)";
			var tabPattern = $@"(\t)";

			builder = builder.Replace(" ", "&nbsp;");
			builder = Regex.Replace(builder, tabPattern, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
			builder = Regex.Replace(builder, breakPattern, "<br>");

			return builder;
		}


		public static string GetLogColor(this string text, string logType)
		{
			var color = "white";
			var yellow = "#F9F1A5";
			var red = "#E74856";
			switch (logType)
			{
				case "ProcessId":
					color = yellow;
					break;
				case "StdOut":
					color = "white";
					break;

				case "StdErr":
					color = red;
					break;

				case "Exited":
					color = red;
					break;

				default:
					color = "white";
					break;
			}
			if (text.Contains("warning", StringComparison.InvariantCultureIgnoreCase))
			{
				text = $@"<span style=""color:{yellow}"">{text}</span>";
				return text;
			}
			if (text.Contains("error", StringComparison.InvariantCultureIgnoreCase))
			{
				text = $@"<span style=""color:{red}"">{text}</span>";
				return text;
			}
			text = Regex.Replace(text, @"info:+", @"<span style=""color:green"">info</span>:");
			//text = text.Replace("info", @"<span style=""color:green"">info</span>");
			text = Regex.Replace(text, @"warn:+", @$"<span style=""color:{yellow}"">warn</span>:");
			//text = text.Replace("warn", @$"<span style=""color:{yellow}"">warn</span>");
			text = text.Replace("error", @$"<span style=""color:{red}"">error</span>");
			text = $@"<span style=""color:{color}"">{text}</span>";
			return text;
		}

		public static string Encoding(this string text, Encoding encoding)
		{

			// 将字符串编码为 UTF-8 字节数组
			byte[] utf8Bytes = encoding.GetBytes(text);

			// 将字节数组解码为字符串
			string decodedString = encoding.GetString(utf8Bytes);
			return decodedString;
		}

		public static T Clone<T>(this T obj)
		{
			var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj));
			if (result == null)
			{
				return default(T);
			}
			return result;
		}

	}
}
