using BaoZhong.Core.Helper;
using BaoZhong.Core.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BaoZhong.Core
{
    /// <summary>
    /// desc:插件管理
    /// author:cgm
    /// date:2016/8/4
    /// </summary>
	public static class PluginsManagement
	{
		private static Dictionary<PluginType, List<PluginInfo>> IntalledPlugins;

		private static bool registed;

		static PluginsManagement()
		{
            IntalledPlugins = new Dictionary<PluginType, List<PluginInfo>>();
			PluginsManagement.registed = false;
			foreach (object current in Enum.GetValues(typeof(PluginType)))
			{
                IntalledPlugins.Add((PluginType)current, new List<PluginInfo>());
			}
		}

		public static IEnumerable<PluginInfo> GetInstalledPluginInfos(PluginType pluginType)
		{
			return from item in PluginsManagement.IntalledPlugins[pluginType]
			select PluginsManagement.DeepClone(item);
		}

		public static PluginInfo GetPluginInfo(string pluginId)
		{
			PluginInfo pluginInfo = null;
			foreach (List<PluginInfo> current in PluginsManagement.IntalledPlugins.Values)
			{
				pluginInfo = current.FirstOrDefault((PluginInfo item) => item.PluginId == pluginId);
				if (pluginInfo != null)
				{
					break;
				}
			}
			return pluginInfo;
		}

		public static IEnumerable<T> GetInstalledPlugins<T>(PluginType pluginType) where T : IPlugin
		{
            IEnumerable<PluginInfo> installedPluginInfos = GetInstalledPluginInfos(pluginType);
			T[] array = new T[installedPluginInfos.Count<PluginInfo>()];
			int num = 0;
			foreach (PluginInfo current in installedPluginInfos)
			{
				array[num++] = Instance.Get<T>(current.ClassFullName);
			}
			return array;
		}

		public static T GetInstalledPlugin<T>(string pluginId) where T : IPlugin
		{
			T result = default(T);
			PluginInfo pluginInfo = PluginsManagement.GetPluginInfo(pluginId);
			if (pluginInfo != null)
			{
				result = Instance.Get<T>(pluginInfo.ClassFullName);
			}
			return result;
		}

		public static void RegistAtStart()
		{
			if (!PluginsManagement.registed)
			{
				PluginsManagement.registed = true;
				string mapPath = IOHelper.GetMapPath("/plugins");
                List<string> list = PluginsManagement.GetPluginFiles(mapPath).ToList<string>();
				mapPath = IOHelper.GetMapPath("/Strategies");
				list.AddRange(PluginsManagement.GetPluginFiles(mapPath));
				foreach (string current in list)
				{
                    Assembly assembly = PluginsManagement.InstallDll(current);
				}
			}
		}

		private static void EnablePlugin_Private(string pluginId, bool enable)
		{
			PluginInfo pluginInfo = PluginsManagement.GetPluginInfo(pluginId);
			if (pluginInfo == null)
			{
				throw new PluginNotFoundException(pluginId);
			}
			pluginInfo.Enable = enable;
			XmlHelper.SerializeToXml(pluginInfo, IOHelper.GetMapPath("/plugins/configs/") + pluginId + ".config");
		}

		public static void InstallPlugin(string pluginFullDirectory)
		{
            IEnumerable<string> pluginFiles = PluginsManagement.GetPluginFiles(pluginFullDirectory);
			foreach (string current in pluginFiles)
			{
				try
				{
					PluginsManagement.InstallDll(current);
				}
				catch (Exception exception)
				{
					Log.Error("插件安装失败(" + current + ")", exception);
				}
			}
		}

		public static void UnInstallPlugin(string classFullName)
		{
            List<PluginInfo> list = new List<PluginInfo>();
			foreach (List<PluginInfo> current in PluginsManagement.IntalledPlugins.Values)
			{
				list.AddRange(current);
			}
			PluginInfo pluginInfo = list.FirstOrDefault((PluginInfo item) => item.ClassFullName == classFullName);
			if (pluginInfo == null)
			{
				Log.Warn(string.Format("卸载插件{0}时没有找到插件信息", classFullName));
			}
			else
			{
				foreach (PluginType current2 in pluginInfo.PluginTypes)
				{
					PluginsManagement.IntalledPlugins[current2].Remove(pluginInfo);
				}
				try
				{
                    Directory.Delete(pluginInfo.PluginDirectory, true);
				}
				catch
				{
					Log.Warn(string.Format("移除插件{0}时没有找到对应的插件目录", pluginInfo.PluginId));
				}
			}
		}

		public static IEnumerable<Plugin<T>> GetPlugins<T>() where T : IPlugin
		{
			PluginType pluginTypeByType = PluginsManagement.GetPluginTypeByType(typeof(T));
            IEnumerable<PluginInfo> installedPluginInfos = PluginsManagement.GetInstalledPluginInfos(pluginTypeByType);
			int num = installedPluginInfos.Count<PluginInfo>();
			Plugin<T>[] array = new Plugin<T>[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new Plugin<T>
				{
					Biz = Instance.Get<T>(installedPluginInfos.ElementAt(i).ClassFullName),
					PluginInfo = installedPluginInfos.ElementAt(i)
				};
			}
			return array;
		}

		public static Plugin<T> GetPlugin<T>(string pluginId) where T : IPlugin
		{
			PluginInfo pluginInfo = PluginsManagement.GetPluginInfo(pluginId);
			return new Plugin<T>
			{
				PluginInfo = pluginInfo,
				Biz = Instance.Get<T>(pluginInfo.ClassFullName)
			};
		}

		public static void EnablePlugin(string pluginId, bool enable)
		{
			try
			{
				Plugin<IPlugin> plugin = PluginsManagement.GetPlugin<IPlugin>(pluginId);
				if (enable)
				{
					plugin.Biz.CheckCanEnable();
				}
				PluginsManagement.EnablePlugin_Private(pluginId, enable);
			}
			catch
			{
				throw;
			}
		}

		public static void UninstallPlugin(string pluginId)
		{
			throw new NotImplementedException();
		}

		public static IEnumerable<Plugin<T>> GetPlugins<T>(bool onlyEnabled) where T : IPlugin
		{
            IEnumerable<Plugin<T>> enumerable = PluginsManagement.GetPlugins<T>();
			if (onlyEnabled)
			{
				enumerable = from item in enumerable
				where item.PluginInfo.Enable
				select item;
			}
			return enumerable;
		}

		private static PluginType GetPluginTypeByType(Type pluginType)
		{
			PluginType result = PluginType.OauthPlugin;
            //暂未开放以下插件
			//if (pluginType == typeof(IPaymentPlugin))
			//{
			//	result = PluginType.PayPlugin;
			//}
			//else if (pluginType == typeof(IExpress))
			//{
			//	result = PluginType.Express;
			//}
			//else if (pluginType == typeof(IOAuthPlugin))
			//{
			//	result = PluginType.OauthPlugin;
			//}
			//else if (pluginType == typeof(IMessagePlugin))
			//{
			//	result = PluginType.Message;
			//}
			//else if (pluginType == typeof(ISMSPlugin))
			//{
			//	result = PluginType.SMS;
			//}
			//else
			//{
			//	if (!(pluginType == typeof(IEmailPlugin)))
			//	{
			//		throw new NotSupportedException("暂不支持" + pluginType.Name + "类型的插件");
			//	}
			//	result = PluginType.Email;
			//}
			return result;
		}

		private static Assembly InstallDll(string dllFileName)
		{
			string text = dllFileName;
            FileInfo fileInfo = new FileInfo(dllFileName);
            DirectoryInfo directoryInfo;
			if (!string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.DynamicDirectory))
			{
				directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.DynamicDirectory);
			}
			else
			{
				directoryInfo = new DirectoryInfo(IOHelper.GetMapPath(""));
			}
			text = directoryInfo.FullName + "\\" + fileInfo.Name;
            Assembly assembly = null;
			PluginInfo pluginInfo = null;
			try
			{
				try
				{
                    File.Copy(dllFileName, text, true);
				}
				catch
				{
                    File.Move(text, text + Guid.NewGuid().ToString("N") + ".locked");
                    File.Copy(dllFileName, text, true);
				}
				assembly = Assembly.Load(AssemblyName.GetAssemblyName(text));
				if (assembly.FullName.StartsWith("BaoZhong.Plugin"))//可以支持以前成熟的插件
				{
					pluginInfo = PluginsManagement.AddPluginInfo(fileInfo);
					IPlugin plugin = Instance.Get<IPlugin>(pluginInfo.ClassFullName);
					plugin.WorkDirectory = fileInfo.Directory.FullName;
				}
			}
			catch (IOException exception)
			{
				Log.Error("插件复制失败(" + dllFileName + ")！", exception);
				if (pluginInfo != null)
				{
					PluginsManagement.RemovePlugin(pluginInfo);
				}
			}
			catch (Exception exception2)
			{
				Log.Error("插件加载失败(" + dllFileName + ")！", exception2);
				if (pluginInfo != null)
				{
					PluginsManagement.RemovePlugin(pluginInfo);
				}
			}
			return assembly;
		}

		private static PluginInfo AddPluginInfo(FileInfo dllFile)
		{
			string text = dllFile.Name.Replace(".dll", "");
			string text2 = IOHelper.GetMapPath("/plugins/configs/") + text + ".config";
			PluginInfo pluginInfo;
			if (!File.Exists(text2))
			{
                FileInfo[] files = dllFile.Directory.GetFiles("plugin.config", SearchOption.TopDirectoryOnly);
				if (files.Length <= 0)
				{
					throw new FileNotFoundException("未找到插件" + text + "的配置文件");
				}
				pluginInfo = (PluginInfo)XmlHelper.DeserializeFromXML(typeof(PluginInfo), files[0].FullName);
				pluginInfo.PluginId = text;
				pluginInfo.PluginDirectory = dllFile.Directory.FullName;
				pluginInfo.AddedTime = new System.DateTime?(DateTime.Now);
				XmlHelper.SerializeToXml(pluginInfo, text2);
			}
			else
			{
				pluginInfo = (PluginInfo)XmlHelper.DeserializeFromXML(typeof(PluginInfo), text2);
			}
			PluginsManagement.UpdatePluginList(pluginInfo);
			return pluginInfo;
		}

		private static void UpdatePluginList(PluginInfo plugin)
		{
			foreach (PluginType current in plugin.PluginTypes)
			{
				PluginsManagement.IntalledPlugins[current].Add(plugin);
			}
		}

		private static void RemovePlugin(PluginInfo plugin)
		{
			foreach (PluginType current in plugin.PluginTypes)
			{
				PluginsManagement.IntalledPlugins[current].Remove(plugin);
			}
		}

		private static IEnumerable<string> GetPluginFiles(string pluginDirectory)
		{
			if (!Directory.Exists(pluginDirectory))
			{
				throw new BaoZhongException("未能找到指定的插件目录:" + pluginDirectory);
			}
			return Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);
		}

		private static PluginInfo DeepClone(PluginInfo plugin)
		{
			string value = JsonConvert.SerializeObject(plugin);
			return JsonConvert.DeserializeObject<PluginInfo>(value);
		}
	}
}
