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
			bool flag = this.context.RoleInfo.Any((RoleInfo a) => a.RoleName == model.RoleName);
			if (flag)
			{
				throw new BaoZhongException("已存在相同名称的权限组");
			}
			this.context.RoleInfo.Add(model);
			this.context.SaveChanges();
		}

		public void UpdatePlatformRole(RoleInfo model)
		{
			RoleInfo updatemodel = this.context.RoleInfo.FindBy((RoleInfo a) => a.Id == model.Id).FirstOrDefault<RoleInfo>();
			if (updatemodel == null)
			{
				throw new BaoZhongException("找不到该权限组");
			}
			bool flag = this.context.RoleInfo.Any((RoleInfo a) => a.RoleName == model.RoleName && a.RoleName != updatemodel.RoleName);
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
			this.context.SaveChanges();
		}


		public void DeletePlatformRole(long id)
		{
			RoleInfo entity = (from a in this.context.RoleInfo
			where a.Id == id
			select a).FirstOrDefault<RoleInfo>();
			this.context.RoleInfo.Remove(entity);
			this.context.SaveChanges();
		}

		public RoleInfo GetPlatformRole(long id)
		{
			return (from a in this.context.RoleInfo
			where a.Id == id
			select a).FirstOrDefault<RoleInfo>();
		}


		public IQueryable<RoleInfo> GetPlatformRoles()
		{
			return this.context.RoleInfo.FindAll();
		}

	}
}
