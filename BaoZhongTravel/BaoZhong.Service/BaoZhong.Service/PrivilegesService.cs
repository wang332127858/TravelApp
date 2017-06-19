using BaoZhong.Core;
using BaoZhong.Entity;
using BaoZhong.IServices;
using BaoZhong.Model;
using System;
using System.Linq;

namespace BaoZhong.Service
{
	public class PrivilegesService : ServiceBase, IPrivilegesService, IService, System.IDisposable
	{
		public void AddPlatformRole(RoleInfo model)
		{
			model.ShopId = 0L;
			if (string.IsNullOrEmpty(model.Description))
			{
				model.Description = model.RoleName;
			}
			bool flag = this.context.RoleInfo.Any((RoleInfo a) => a.RoleName == model.RoleName && a.ShopId == model.ShopId);
			if (flag)
			{
				throw new BaoZhongException("已存在相同名称的权限组");
			}
			this.context.RoleInfo.Add(model);
			this.context.SaveChanges();
		}

		public void UpdatePlatformRole(RoleInfo model)
		{
			RoleInfo updatemodel = this.context.RoleInfo.FindBy((RoleInfo a) => a.ShopId == 0L && a.Id == model.Id).FirstOrDefault<RoleInfo>();
			if (updatemodel == null)
			{
				throw new BaoZhongException("找不到该权限组");
			}
			bool flag = this.context.RoleInfo.Any((RoleInfo a) => a.RoleName == model.RoleName && a.ShopId == 0L && a.RoleName != updatemodel.RoleName);
			if (flag)
			{
				throw new BaoZhongException("已存在相同名称的权限组");
			}
			updatemodel.RoleName = model.RoleName;
			updatemodel.Description = model.Description;
			if (string.IsNullOrEmpty(model.Description))
			{
				updatemodel.Description = model.RoleName;
			}
			this.context.RolePrivilegeInfo.RemoveRange(updatemodel.RolePrivilegeInfo);
			updatemodel.RolePrivilegeInfo = model.RolePrivilegeInfo;
			this.context.SaveChanges();
		}

		public void UpdateSellerRole(RoleInfo model)
		{
			RoleInfo updatemodel = this.context.RoleInfo.FindBy((RoleInfo a) => a.ShopId == model.ShopId && a.Id == model.Id).FirstOrDefault<RoleInfo>();
			if (updatemodel == null)
			{
				throw new BaoZhongException("找不到该权限组");
			}
			bool flag = this.context.RoleInfo.Any((RoleInfo a) => a.RoleName == model.RoleName && a.ShopId == model.ShopId && a.RoleName != updatemodel.RoleName);
			if (flag)
			{
				throw new BaoZhongException("已存在相同名称的权限组");
			}
			updatemodel.RoleName = model.RoleName;
			updatemodel.Description = model.Description;
			if (string.IsNullOrEmpty(model.Description))
			{
				updatemodel.Description = model.RoleName;
			}
			this.context.RolePrivilegeInfo.RemoveRange(updatemodel.RolePrivilegeInfo);
			updatemodel.RolePrivilegeInfo = model.RolePrivilegeInfo;
			this.context.SaveChanges();
		}

		public void DeletePlatformRole(long id)
		{
			RoleInfo entity = (from a in this.context.RoleInfo
			where a.Id == id && a.ShopId == 0L
			select a).FirstOrDefault<RoleInfo>();
			this.context.RoleInfo.Remove(entity);
			this.context.SaveChanges();
		}

		public RoleInfo GetPlatformRole(long id)
		{
			return (from a in this.context.RoleInfo
			where a.Id == id && a.ShopId == 0L
			select a).FirstOrDefault<RoleInfo>();
		}

		public RoleInfo GetSellerRole(long id, long shopid)
		{
			return (from a in this.context.RoleInfo
			where a.Id == id && a.ShopId == shopid
			select a).FirstOrDefault<RoleInfo>();
		}

		public IQueryable<RoleInfo> GetPlatformRoles()
		{
			return this.context.RoleInfo.FindBy((RoleInfo item) => item.ShopId == 0L);
		}

		public IQueryable<RoleInfo> GetSellerRoles(long shopId)
		{
			return from item in this.context.RoleInfo
			where item.ShopId == shopId && item.ShopId != 0L
			select item;
		}

		public void AddSellerRole(RoleInfo model)
		{
			if (string.IsNullOrEmpty(model.Description))
			{
				model.Description = model.RoleName;
			}
			bool flag = this.context.RoleInfo.Any((RoleInfo a) => a.RoleName == model.RoleName && a.ShopId == model.ShopId);
			if (flag)
			{
				throw new BaoZhongException("已存在相同名称的权限组");
			}
			this.context.RoleInfo.Add(model);
			this.context.SaveChanges();
		}

		public void DeleteSellerRole(long id, long shopId)
		{
			RoleInfo entity = (from a in this.context.RoleInfo
			where a.Id == id && a.ShopId == shopId
			select a).FirstOrDefault<RoleInfo>();
			this.context.RoleInfo.Remove(entity);
			this.context.SaveChanges();
		}
	}
}
