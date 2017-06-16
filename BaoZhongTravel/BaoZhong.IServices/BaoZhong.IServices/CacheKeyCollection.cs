namespace BaoZhong.IServices
{
	public static class CacheKeyCollection
	{
		public const string HomeCategory = "Cache-HomeCategories";
		
		public static string Manager(long managerId)
		{
			return string.Format("Cache-Manager-{0}", managerId);
		}

		public static string ManagerLoginError(string username)
		{
			return string.Format("Cache-Manager-Login-{0}", username);
		}

        public static string Member(long memberId)
        {
            return string.Format("Cache-Member-{0}", memberId);
        }

        public static string Seller(long sellerId)
        {
            return string.Format("Cache-Seller-{0}", sellerId);
        }
    }
}
