using System;
using System.Text.RegularExpressions;

namespace BaoZhong.Core.Helper
{
    /// <summary>
    /// desc:–£—È∏®÷˙¿‡
    /// author:cgm
    /// </summary>
	public class ValidateHelper
	{
		private static Regex _emailregex = new Regex("^(\\w)+(\\.\\w+)*@(\\w)+((\\.\\w+)+)$", RegexOptions.IgnoreCase);

		private static Regex _mobileregex = new Regex("^(13|14|15|16|17|18|19)[0-9]{9}$");

		private static Regex _phoneregex = new Regex("^(\\d{3,4}-?)?\\d{7,8}$");

		private static Regex _ipregex = new Regex("^(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])$");

		private static Regex _dateregex = new Regex("(\\d{4})-(\\d{1,2})-(\\d{1,2})");

		private static Regex _numericregex = new Regex("^[-]?[0-9]+(\\.[0-9]+)?$");

		private static Regex _zipcoderegex = new Regex("^\\d{6}$");

		public static bool IsEmail(string s)
		{
			return string.IsNullOrEmpty(s) || ValidateHelper._emailregex.IsMatch(s);
		}

		public static bool IsMobile(string s)
		{
			return string.IsNullOrEmpty(s) || ValidateHelper._mobileregex.IsMatch(s);
		}

		public static bool IsPhone(string s)
		{
			return string.IsNullOrEmpty(s) || ValidateHelper._phoneregex.IsMatch(s);
		}

		public static bool IsIP(string s)
		{
			return ValidateHelper._ipregex.IsMatch(s);
		}

		public static bool IsIdCard(string id)
		{
			bool result;
			if (string.IsNullOrEmpty(id))
			{
				result = true;
			}
			else if (id.Length == 18)
			{
				result = ValidateHelper.CheckIDCard18(id);
			}
			else
			{
				result = (id.Length == 15 && ValidateHelper.CheckIDCard15(id));
			}
			return result;
		}

		private static bool CheckIDCard18(string Id)
		{
			long num = 0L;
			bool result;
			if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
			{
				result = false;
			}
			else
			{
				string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
				if (text.IndexOf(Id.Remove(2)) == -1)
				{
					result = false;
				}
				else
				{
					string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                    DateTime dateTime = default(DateTime);
					if (!DateTime.TryParse(s, out dateTime))
					{
						result = false;
					}
					else
					{
						string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new char[]
						{
							','
						});
						string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new char[]
						{
							','
						});
						char[] array3 = Id.Remove(17).ToCharArray();
						int num2 = 0;
						for (int i = 0; i < 17; i++)
						{
							num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
						}
						int num3 = -1;
                        Math.DivRem(num2, 11, out num3);
						result = !(array[num3] != Id.Substring(17, 1).ToLower());
					}
				}
			}
			return result;
		}

		private static bool CheckIDCard15(string Id)
		{
			long num = 0L;
			bool result;
			if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
			{
				result = false;
			}
			else
			{
				string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
				if (text.IndexOf(Id.Remove(2)) == -1)
				{
					result = false;
				}
				else
				{
					string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                    DateTime dateTime = default(System.DateTime);
					result = DateTime.TryParse(s, out dateTime);
				}
			}
			return result;
		}

		public static bool IsDate(string s)
		{
			return ValidateHelper._dateregex.IsMatch(s);
		}

		public static bool IsNumeric(string numericStr)
		{
			return ValidateHelper._numericregex.IsMatch(numericStr);
		}

		public static bool IsZipCode(string s)
		{
			return string.IsNullOrEmpty(s) || ValidateHelper._zipcoderegex.IsMatch(s);
		}

		public static bool IsImgFileName(string fileName)
		{
			bool result;
			if (fileName.IndexOf(".") == -1)
			{
				result = false;
			}
			else
			{
				string text = fileName.Trim().ToLower();
				string a = text.Substring(text.LastIndexOf("."));
				result = (a == ".png" || a == ".bmp" || a == ".jpg" || a == ".jpeg" || a == ".gif");
			}
			return result;
		}

		public static bool InIP(string sourceIP, string targetIP)
		{
			bool result;
			if (string.IsNullOrEmpty(sourceIP) || string.IsNullOrEmpty(targetIP))
			{
				result = false;
			}
			else
			{
				string[] array = StringHelper.SplitString(sourceIP, ".");
				string[] array2 = StringHelper.SplitString(targetIP, ".");
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					if (array2[i] == "*")
					{
						result = true;
						return result;
					}
					if (array[i] != array2[i])
					{
						result = false;
						return result;
					}
					if (i == 3)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		public static bool InIPList(string sourceIP, string[] targetIPList)
		{
			bool result;
			if (targetIPList != null && targetIPList.Length > 0)
			{
				for (int i = 0; i < targetIPList.Length; i++)
				{
					string targetIP = targetIPList[i];
					if (ValidateHelper.InIP(sourceIP, targetIP))
					{
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}

		public static bool InIPList(string sourceIP, string targetIPStr)
		{
			string[] targetIPList = StringHelper.SplitString(targetIPStr, "\n");
			return ValidateHelper.InIPList(sourceIP, targetIPList);
		}

		public static bool BetweenPeriod(string[] periodList, out string liePeriod)
		{
			bool result;
			if (periodList != null && periodList.Length > 0)
			{
				DateTime now = DateTime.Now;
				DateTime date = now.Date;
				for (int i = 0; i < periodList.Length; i++)
				{
					string text = periodList[i];
					int num = text.IndexOf("-");
					DateTime dateTime = TypeHelper.StringToDateTime(text.Substring(0, num));
					DateTime t = TypeHelper.StringToDateTime(text.Substring(num + 1));
					if (dateTime < t)
					{
						if (now > dateTime && now < t)
						{
							liePeriod = text;
							result = true;
							return result;
						}
					}
					else if ((now > dateTime && now < date.AddDays(1.0)) || now < t)
					{
						liePeriod = text;
						result = true;
						return result;
					}
				}
			}
			liePeriod = string.Empty;
			result = false;
			return result;
		}

		public static bool BetweenPeriod(string periodStr, out string liePeriod)
		{
			string[] periodList = StringHelper.SplitString(periodStr, "\n");
			return ValidateHelper.BetweenPeriod(periodList, out liePeriod);
		}

		public static bool BetweenPeriod(string periodList)
		{
			string empty = string.Empty;
			return ValidateHelper.BetweenPeriod(periodList, out empty);
		}

		public static bool IsNumericArray(string[] numericStrList)
		{
			bool result;
			if (numericStrList != null && numericStrList.Length > 0)
			{
				for (int i = 0; i < numericStrList.Length; i++)
				{
					string numericStr = numericStrList[i];
					if (!ValidateHelper.IsNumeric(numericStr))
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool IsNumericRule(string numericRuleStr, string splitChar)
		{
			return ValidateHelper.IsNumericArray(StringHelper.SplitString(numericRuleStr, splitChar));
		}

		public static bool IsNumericRule(string numericRuleStr)
		{
			return ValidateHelper.IsNumericRule(numericRuleStr, ",");
		}
	}
}
