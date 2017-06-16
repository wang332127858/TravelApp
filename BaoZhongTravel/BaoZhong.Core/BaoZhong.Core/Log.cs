using log4net;
using System;
using System.Diagnostics;

namespace BaoZhong.Core
{
	public static class Log
	{
		public static void Error(object message)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Error(message);
		}

		public static void Error(object message, Exception exception)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Error(message, exception);
		}

		public static void Info(object message)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Info(message);
		}

		public static void Info(object message, Exception ex)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Info(message, ex);
		}

		public static void Warn(object message)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Warn(message);
		}

		public static void Warn(object message, Exception ex)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Warn(message, ex);
		}

		public static void Debug(object message)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Debug(message);
		}

		public static void Debug(object message, Exception ex)
		{
			ILog logger = LogManager.GetLogger(Log.GetCurrentMethodFullName());
			logger.Debug(message, ex);
		}

		private static string GetCurrentMethodFullName()
		{
			string result;
			try
			{
				int num = 2;
				StackTrace stackTrace = new StackTrace();
				int num2 = stackTrace.GetFrames().Length;
				StackFrame frame;
				string text;
				do
				{
					frame = stackTrace.GetFrame(num++);
					Type declaringType = frame.GetMethod().DeclaringType;
					text = declaringType.ToString();
				}
				while (text.EndsWith("Exception") && num < num2);
				string name = frame.GetMethod().Name;
				result = text + "." + name;
			}
			catch
			{
				result = null;
			}
			return result;
		}
	}
}
