using BaoZhong.Core;
using BaoZhong.Core.Helper;
using BaoZhong.IServices;
using BaoZhong.ServiceProvider;
using System;
using System.Web;

namespace BaoZhong.Web.Framework
{
	public class UserCookieEncryptHelper
	{
		public static string Encrypt(long userId, string controllerName)
		{
            string text = Instance<ISiteSettingService>.Create.GetSiteSettings().UserCookieKey;
            if (string.IsNullOrEmpty(text))
            {
                text = SecureHelper.MD5(Guid.NewGuid().ToString());
                Instance<ISiteSettingService>.Create.SaveSetting("UserCookieKey", text);
            }

            string text2 = string.Empty;
			string result;
			try
			{
				string encryptStr = controllerName + "," + userId.ToString();
				text2 = SecureHelper.AESEncrypt(encryptStr, text);
				text2 = SecureHelper.EncodeBase64(text2);
				result = text2;
			}
			catch (Exception exception)
			{
				Log.Error(string.Format("加密用户标识Cookie出错", text2), exception);
				throw;
			}
			return result;
		}

		public static long Decrypt(string userIdCookie, string controllerName)
		{
            string text = Instance<ISiteSettingService>.Create.GetSiteSettings().UserCookieKey;
            if (string.IsNullOrEmpty(text))
            {
                text = SecureHelper.MD5(Guid.NewGuid().ToString());
                Instance<ISiteSettingService>.Create.SaveSetting("UserCookieKey", text);
            }

            string text2 = string.Empty;
			try
			{
				if (!string.IsNullOrWhiteSpace(userIdCookie))
				{
					userIdCookie = HttpUtility.UrlDecode(userIdCookie);
					userIdCookie = SecureHelper.DecodeBase64(userIdCookie);
					text2 = SecureHelper.AESDecrypt(userIdCookie, text);
					text2 = text2.Replace(controllerName + ",", "");
				}
			}
			catch (Exception exception)
			{
				Log.Error(string.Format("解密用户标识Cookie出错，Cookie密文：{0}", userIdCookie), exception);
			}
			long result = 0L;
			long.TryParse(text2, out result);
			return result;
		}
	}
}
