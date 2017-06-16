using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BaoZhong.Core.Helper
{
    /// <summary>
    /// desc:×Ö·û´®¸¨ÖúÀà
    /// author:cgm
    /// </summary>
	public class StringHelper
	{
		public static int GetStringLength(string s)
		{
			int result;
			if (!string.IsNullOrEmpty(s))
			{
				result = Encoding.Default.GetBytes(s).Length;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public static string[] SplitString(string sourceStr, string splitStr)
		{
			string[] result;
			if (string.IsNullOrEmpty(sourceStr) || string.IsNullOrEmpty(splitStr))
			{
				result = new string[0];
			}
			else if (sourceStr.IndexOf(splitStr) == -1)
			{
				result = new string[]
				{
					sourceStr
				};
			}
			else if (splitStr.Length == 1)
			{
				result = sourceStr.Split(new char[]
				{
					splitStr[0]
				});
			}
			else
			{
				result = Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);
			}
			return result;
		}

		public static string[] SplitString(string sourceStr)
		{
			return StringHelper.SplitString(sourceStr, ",");
		}

		public static string SubString(string sourceStr, int startIndex, int length)
		{
			string result;
			if (!string.IsNullOrEmpty(sourceStr))
			{
				if (sourceStr.Length >= startIndex + length)
				{
					result = sourceStr.Substring(startIndex, length);
				}
				else
				{
					result = sourceStr.Substring(startIndex);
				}
			}
			else
			{
				result = "";
			}
			return result;
		}

		public static string SubString(string sourceStr, int length)
		{
			return StringHelper.SubString(sourceStr, 0, length);
		}

		public static string TrimStart(string sourceStr, string trimStr)
		{
			return StringHelper.TrimStart(sourceStr, trimStr, true);
		}

		public static string TrimStart(string sourceStr, string trimStr, bool ignoreCase)
		{
			string result;
			if (string.IsNullOrEmpty(sourceStr))
			{
				result = string.Empty;
			}
			else if (string.IsNullOrEmpty(trimStr) || !sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
			{
				result = sourceStr;
			}
			else
			{
				result = sourceStr.Remove(0, trimStr.Length);
			}
			return result;
		}

		public static string TrimEnd(string sourceStr, string trimStr)
		{
			return StringHelper.TrimEnd(sourceStr, trimStr, true);
		}

		public static string TrimEnd(string sourceStr, string trimStr, bool ignoreCase)
		{
			string result;
			if (string.IsNullOrEmpty(sourceStr))
			{
				result = string.Empty;
			}
			else if (string.IsNullOrEmpty(trimStr) || !sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
			{
				result = sourceStr;
			}
			else
			{
				result = sourceStr.Substring(0, sourceStr.Length - trimStr.Length);
			}
			return result;
		}

		public static string Trim(string sourceStr, string trimStr)
		{
			return StringHelper.Trim(sourceStr, trimStr, true);
		}

		public static string Trim(string sourceStr, string trimStr, bool ignoreCase)
		{
			string result;
			if (string.IsNullOrEmpty(sourceStr))
			{
				result = string.Empty;
			}
			else if (string.IsNullOrEmpty(trimStr))
			{
				result = sourceStr;
			}
			else
			{
				if (sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
				{
					sourceStr = sourceStr.Remove(0, trimStr.Length);
				}
				if (sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
				{
					sourceStr = sourceStr.Substring(0, sourceStr.Length - trimStr.Length);
				}
				result = sourceStr;
			}
			return result;
		}

		public static int IndexOf(string s, int order)
		{
			return StringHelper.IndexOf(s, '-', order);
		}

		public static int IndexOf(string s, char c, int order)
		{
			int length = s.Length;
			int result;
			for (int i = 0; i < length; i++)
			{
				if (c == s[i])
				{
					if (order == 1)
					{
						result = i;
						return result;
					}
					order--;
				}
			}
			result = -1;
			return result;
		}
	}
}
