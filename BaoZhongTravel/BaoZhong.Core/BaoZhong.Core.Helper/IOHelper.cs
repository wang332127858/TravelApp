using System;
using System.IO;
using System.Web;

namespace BaoZhong.Core.Helper
{
    /// <summary>
    /// desc:IO辅助类
    /// author:cgm
    /// </summary>
	public class IOHelper
	{
		public static string GetMapPath(string path)
		{
			string result;
			if (HttpContext.Current != null)
			{
				result = HttpContext.Current.Server.MapPath(path);
			}
			else
			{
				string applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                Log.Debug("applicationBase:" + applicationBase);
                if (!string.IsNullOrWhiteSpace(path))
				{
					path = path.Replace("/", "\\");
					if (!path.StartsWith("\\"))
					{
						path = "\\" + path;
					}
					path = path.Substring(path.IndexOf('\\') + (applicationBase.EndsWith("\\") ? 1 : 0));
				}
				result = applicationBase + path;
			}
			return result;
		}

		public static void CopyFile(string fileFullPath, string destination, bool isDeleteSourceFile = false, string fileName = "")
		{
			if (string.IsNullOrWhiteSpace(fileFullPath))
			{
				throw new ArgumentNullException("fileFullPath", "源文件全路径不能为空");
			}
			if (!File.Exists(fileFullPath))
			{
				throw new FileNotFoundException("找不到源文件", fileFullPath);
			}
			if (!Directory.Exists(destination))
			{
				throw new DirectoryNotFoundException("找不到目标目录 " + destination);
			}
			try
			{
				fileName = (string.IsNullOrWhiteSpace(fileName) ? Path.GetFileName(fileFullPath) : fileName);
				File.Copy(fileFullPath, Path.Combine(destination, fileName), true);
				if (isDeleteSourceFile)
				{
					File.Delete(fileFullPath);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static long GetDirectoryLength(string dirPath)
		{
			long result;
			if (!Directory.Exists(dirPath))
			{
				result = 0L;
			}
			else
			{
				long num = 0L;
				DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					FileInfo fileInfo = files[i];
					num += fileInfo.Length;
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				if (directories.Length > 0)
				{
					for (int j = 0; j < directories.Length; j++)
					{
						num += IOHelper.GetDirectoryLength(directories[j].FullName);
					}
				}
				result = num;
			}
			return result;
		}
	}
}
