using BaoZhong.Model;
using System;
using System.Linq;

namespace BaoZhong.IServices
{
	public interface ISiteSettingService : IService, IDisposable
	{
		SiteSettingsInfo GetSiteSettings();

        void SetSiteSettings(SiteSettingsInfo siteSettingsInfo);

		void SaveSetting(string key, object value);
	}
}
