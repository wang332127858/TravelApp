using System.Xml.Serialization;

namespace BaoZhong.Core.Helper
{
	public class XmlHelper
	{
		public static bool SerializeToXml(object obj, string filePath)
		{
			bool result = false;
			System.IO.FileStream fileStream = null;
			try
			{
				fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
				XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
				xmlSerializer.Serialize(fileStream, obj);
				result = true;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			return result;
		}

		public static object DeserializeFromXML(System.Type type, string filePath)
		{
			System.IO.FileStream fileStream = null;
			object result;
			try
			{
				fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				result = xmlSerializer.Deserialize(fileStream);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			return result;
		}
	}
}
