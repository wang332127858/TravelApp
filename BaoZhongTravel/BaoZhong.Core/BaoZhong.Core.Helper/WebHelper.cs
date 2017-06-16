using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BaoZhong.Core.Helper
{
    /// <summary>
    /// desc:网站辅助类
    /// author:cgm
    /// </summary>
	public class WebHelper
	{
		private static string[] _browserlist = new string[]
		{
			"ie",
			"chrome",
			"mozilla",
			"netscape",
			"firefox",
			"opera",
			"konqueror"
		};

		private static string[] _searchenginelist = new string[]
		{
			"baidu",
			"google",
			"360",
			"sogou",
			"bing",
			"msn",
			"sohu",
			"soso",
			"sina",
			"163",
			"yahoo",
			"jikeu"
		};

		private static Regex _metaregex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		public static string HtmlDecode(string s)
		{
			return HttpUtility.HtmlDecode(s);
		}

		public static string HtmlEncode(string s)
		{
			return HttpUtility.HtmlEncode(s);
		}

		public static string UrlDecode(string s)
		{
			return HttpUtility.UrlDecode(s);
		}

		public static string UrlEncode(string s)
		{
			return HttpUtility.UrlEncode(s);
		}

		public static void DeleteCookie(string name)
		{
			HttpCookie httpCookie = new HttpCookie(name);
			httpCookie.Expires = DateTime.Now.AddYears(-1);
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static string GetCookie(string name)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			string result;
			if (httpCookie != null)
			{
				result = httpCookie.Value;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		public static string GetCookie(string name, string key)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			string result;
			if (httpCookie != null && httpCookie.HasKeys)
			{
				string text = httpCookie[key];
				if (text != null)
				{
					result = text;
					return result;
				}
			}
			result = string.Empty;
			return result;
		}

		public static void SetCookie(string name, string value)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie != null)
			{
				httpCookie.Value = value;
			}
			else
			{
				httpCookie = new HttpCookie(name, value);
			}
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static void SetCookie(string name, string value, DateTime dt)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(name);
			}
			httpCookie.Value = value;
			httpCookie.Expires = dt;
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static void SetCookie(string name, string key, string value)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(name);
			}
			httpCookie[key] = value;
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static void SetCookie(string name, string key, string value, DateTime dt)
		{
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(name);
			}
			httpCookie[key] = value;
			httpCookie.Expires = dt;
			HttpContext.Current.Response.AppendCookie(httpCookie);
		}

		public static bool IsGet()
		{
			return HttpContext.Current.Request.HttpMethod == "GET";
		}

		public static bool IsPost()
		{
			return HttpContext.Current.Request.HttpMethod == "POST";
		}

		public static bool IsAjax()
		{
			return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}

		public static string GetQueryString(string key, string defaultValue)
		{
			string text = HttpContext.Current.Request.QueryString[key];
			string result;
			if (!string.IsNullOrWhiteSpace(text))
			{
				result = text;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		public static string GetQueryString(string key)
		{
			return WebHelper.GetQueryString(key, "");
		}

		public static int GetQueryInt(string key, int defaultValue)
		{
			return TypeHelper.StringToInt(HttpContext.Current.Request.QueryString[key], defaultValue);
		}

		public static int GetQueryInt(string key)
		{
			return WebHelper.GetQueryInt(key, 0);
		}

		public static string GetFormString(string key, string defaultValue)
		{
			string text = HttpContext.Current.Request.Form[key];
			string result;
			if (!string.IsNullOrWhiteSpace(text))
			{
				result = text;
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}

		public static string GetFormString(string key)
		{
			return WebHelper.GetFormString(key, "");
		}

		public static int GetFormInt(string key, int defaultValue)
		{
			return TypeHelper.StringToInt(HttpContext.Current.Request.Form[key], defaultValue);
		}

		public static int GetFormInt(string key)
		{
			return WebHelper.GetFormInt(key, 0);
		}

		public static string GetUrlReferrer()
		{
			Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;
			string result;
			if (urlReferrer == null)
			{
				result = string.Empty;
			}
			else
			{
				result = urlReferrer.ToString();
			}
			return result;
		}

		public static string GetHost()
		{
			return HttpContext.Current.Request.Url.Host;
		}

		public static string GetUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		public static string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		public static string GetIP()
		{
			string text = string.Empty;
			if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
			{
				text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
			}
			else
			{
				text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			}
			if (string.IsNullOrEmpty(text) || !ValidateHelper.IsIP(text))
			{
				text = "127.0.0.1";
			}
			return text;
		}

		public static string GetBrowserType()
		{
			string type = HttpContext.Current.Request.Browser.Type;
			string result;
			if (string.IsNullOrEmpty(type))
			{
				result = "未知";
			}
			else
			{
				result = type.ToLower();
			}
			return result;
		}

		public static string GetBrowserName()
		{
			string browser = HttpContext.Current.Request.Browser.Browser;
			string result;
			if (string.IsNullOrEmpty(browser))
			{
				result = "未知";
			}
			else
			{
				result = browser.ToLower();
			}
			return result;
		}

		public static string GetBrowserVersion()
		{
			string version = HttpContext.Current.Request.Browser.Version;
			string result;
			if (string.IsNullOrEmpty(version))
			{
				result = "未知";
			}
			else
			{
				result = version;
			}
			return result;
		}

		public static string GetOSType()
		{
			string result = "未知";
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (userAgent.Contains("NT 6.1"))
			{
				result = "Windows 7";
			}
			else if (userAgent.Contains("NT 5.1"))
			{
				result = "Windows XP";
			}
			else if (userAgent.Contains("NT 6.2"))
			{
				result = "Windows 8";
			}
			else if (userAgent.Contains("android"))
			{
				result = "Android";
			}
			else if (userAgent.Contains("iphone"))
			{
				result = "IPhone";
			}
			else if (userAgent.Contains("Mac"))
			{
				result = "Mac";
			}
			else if (userAgent.Contains("NT 6.0"))
			{
				result = "Windows Vista";
			}
			else if (userAgent.Contains("NT 5.2"))
			{
				result = "Windows 2003";
			}
			else if (userAgent.Contains("NT 5.0"))
			{
				result = "Windows 2000";
			}
			else if (userAgent.Contains("98"))
			{
				result = "Windows 98";
			}
			else if (userAgent.Contains("95"))
			{
				result = "Windows 95";
			}
			else if (userAgent.Contains("Me"))
			{
				result = "Windows Me";
			}
			else if (userAgent.Contains("NT 4"))
			{
				result = "Windows NT4";
			}
			else if (userAgent.Contains("Unix"))
			{
				result = "UNIX";
			}
			else if (userAgent.Contains("Linux"))
			{
				result = "Linux";
			}
			else if (userAgent.Contains("SunOS"))
			{
				result = "SunOS";
			}
			return result;
		}

		public static string GetOSName()
		{
			string platform = HttpContext.Current.Request.Browser.Platform;
			string result;
			if (string.IsNullOrEmpty(platform))
			{
				result = "未知";
			}
			else
			{
				result = platform;
			}
			return result;
		}

		public static bool IsBrowser()
		{
			string browserName = WebHelper.GetBrowserName();
			string[] browserlist = WebHelper._browserlist;
			bool result;
			for (int i = 0; i < browserlist.Length; i++)
			{
				string value = browserlist[i];
				if (browserName.Contains(value))
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static bool IsMobile()
		{
			bool result;
			if (HttpContext.Current.Request.Browser.IsMobileDevice)
			{
				result = true;
			}
			else
			{
				bool flag = false;
				result = (bool.TryParse(HttpContext.Current.Request.Browser["IsTablet"], out flag) && flag);
			}
			return result;
		}

		public static bool IsCrawler()
		{
			bool crawler = HttpContext.Current.Request.Browser.Crawler;
			bool result;
			if (!crawler)
			{
				string urlReferrer = WebHelper.GetUrlReferrer();
				if (urlReferrer.Length > 0)
				{
					string[] searchenginelist = WebHelper._searchenginelist;
					for (int i = 0; i < searchenginelist.Length; i++)
					{
						string value = searchenginelist[i];
						if (urlReferrer.Contains(value))
						{
							result = true;
							return result;
						}
					}
				}
			}
			result = crawler;
			return result;
		}

		public static NameValueCollection GetParmList(string data)
		{
			NameValueCollection nameValueCollection = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
			if (!string.IsNullOrEmpty(data))
			{
				int length = data.Length;
				for (int i = 0; i < length; i++)
				{
					int num = i;
					int num2 = -1;
					while (i < length)
					{
						char c = data[i];
						if (c == '=')
						{
							if (num2 < 0)
							{
								num2 = i;
							}
						}
						else if (c == '&')
						{
							break;
						}
						i++;
					}
					string name;
					string value;
					if (num2 >= 0)
					{
						name = data.Substring(num, num2 - num);
						value = data.Substring(num2 + 1, i - num2 - 1);
					}
					else
					{
						name = data.Substring(num, i - num);
						value = string.Empty;
					}
					nameValueCollection[name] = value;
					if (i == length - 1 && data[i] == '&')
					{
						nameValueCollection[name] = string.Empty;
					}
				}
			}
			return nameValueCollection;
		}

		public static string GetRequestData(string url, string postData)
		{
			return WebHelper.GetRequestData(url, "post", postData);
		}

		public static string GetRequestData(string url, string method, string postData)
		{
			return WebHelper.GetRequestData(url, method, postData, Encoding.UTF8);
		}

		public static string GetRequestData(string url, string method, string postData, Encoding encoding)
		{
			return WebHelper.GetRequestData(url, method, postData, encoding, 20000);
		}

		public static string GetRequestData(string url, string method, string postData, Encoding encoding, int timeout)
		{
			string result;
			try
			{
				using (HttpWebResponse uRLResponse = WebHelper.GetURLResponse(url, method, postData, encoding, timeout))
				{
					if (uRLResponse == null)
					{
						result = "error";
					}
					else if (encoding == null)
					{
						MemoryStream memoryStream = new MemoryStream();
						if (uRLResponse.ContentEncoding != null && uRLResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
						{
							new GZipStream(uRLResponse.GetResponseStream(), CompressionMode.Decompress).CopyTo(memoryStream, 10240);
						}
						else
						{
							uRLResponse.GetResponseStream().CopyTo(memoryStream, 10240);
						}
						byte[] array = memoryStream.ToArray();
						string @string = Encoding.Default.GetString(array, 0, array.Length);
						Match match = WebHelper._metaregex.Match(@string);
						string text = (match.Groups.Count > 2) ? match.Groups[2].Value : string.Empty;
						text = text.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
						if (text.Length > 0)
						{
							text = text.ToLower().Replace("iso-8859-1", "gbk");
							encoding = Encoding.GetEncoding(text);
						}
						else if (uRLResponse.CharacterSet.ToLower().Trim() == "iso-8859-1")
						{
							encoding = Encoding.GetEncoding("gbk");
						}
						else if (string.IsNullOrEmpty(uRLResponse.CharacterSet.Trim()))
						{
							encoding = Encoding.UTF8;
						}
						else
						{
							encoding = Encoding.GetEncoding(uRLResponse.CharacterSet);
						}
						result = encoding.GetString(array);
					}
					else
					{
						StreamReader streamReader2;
						StreamReader streamReader;
						if (uRLResponse.ContentEncoding != null && uRLResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
						{
							streamReader = (streamReader2 = new StreamReader(new GZipStream(uRLResponse.GetResponseStream(), CompressionMode.Decompress), encoding));
							try
							{
								result = streamReader.ReadToEnd();
								return result;
							}
							finally
							{
								if (streamReader2 != null)
								{
									((IDisposable)streamReader2).Dispose();
								}
							}
						}
						streamReader = (streamReader2 = new StreamReader(uRLResponse.GetResponseStream(), encoding));
						try
						{
							result = streamReader.ReadToEnd();
						}
						catch
						{
							result = "close";
						}
						finally
						{
							if (streamReader2 != null)
							{
								((IDisposable)streamReader2).Dispose();
							}
						}
					}
				}
			}
			catch
			{
				result = "error";
			}
			return result;
		}

		public static HttpWebResponse GetURLResponse(string url, string method = "get", string postData = "", Encoding encoding = null, int timeout = 20000)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			if (!url.Contains("http://") && !url.Contains("https://"))
			{
				url = "http://" + url;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = method.Trim().ToLower();
			httpWebRequest.Timeout = timeout;
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.ContentType = "text/html";
			httpWebRequest.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
			httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
			httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			HttpWebResponse result;
			try
			{
				if (!string.IsNullOrEmpty(postData) && httpWebRequest.Method == "post")
				{
					byte[] bytes = encoding.GetBytes(postData);
					httpWebRequest.ContentLength = (long)bytes.Length;
					httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				result = httpWebResponse;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			AspNetHostingPermissionLevel result = AspNetHostingPermissionLevel.None;
			AspNetHostingPermissionLevel[] array = new AspNetHostingPermissionLevel[]
			{
				AspNetHostingPermissionLevel.Unrestricted,
				AspNetHostingPermissionLevel.High,
				AspNetHostingPermissionLevel.Medium,
				AspNetHostingPermissionLevel.Low,
				AspNetHostingPermissionLevel.Minimal
			};
			AspNetHostingPermissionLevel[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				AspNetHostingPermissionLevel aspNetHostingPermissionLevel = array2[i];
				try
				{
					new AspNetHostingPermission(aspNetHostingPermissionLevel).Demand();
					result = aspNetHostingPermissionLevel;
					break;
				}
				catch (SecurityException)
				{
				}
			}
			return result;
		}
	}
}
