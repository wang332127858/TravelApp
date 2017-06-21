using BaoZhong.Core;
using BaoZhong.Core.Helper;
using BaoZhong.Entity;
using BaoZhong.IServices;
using BaoZhong.IServices.QueryModel;
using BaoZhong.Model;
using BaoZhong.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace BaoZhong.Service
{
    /// <summary>
    /// desc:管理员服务类
    /// author:cgm
    /// date:2016/11/8
    /// </summary>
	public class ManagerService : ServiceBase, IManagerService, IService, IDisposable
    {
        public PageModel<ManagerInfo> GetPlatformManagers(ManagerQuery query)
        {
            IQueryable<ManagerInfo> disable = this.context.ManagerInfo.AsQueryable();
            if (!string.IsNullOrEmpty(query.keyWords))
            {
                disable = from a in disable where a.UserName == query.keyWords select a;
            }

            if (query.RoleId >= 0)
            {
                disable = from a in disable where a.RoleId == query.RoleId select a;
            }

            int total = 0;
            IQueryable<ManagerInfo> models = disable.FindBy((ManagerInfo item) => item.ShopId == 0L, query.PageNo, query.PageSize, out total, (ManagerInfo item) => item.RoleId, false);
            return new PageModel<ManagerInfo>
            {
                Models = models,
                Total = total
            };
        }

        

        public ManagerInfo GetPlatformManager(long userId)
        {
            string key = CacheKeyCollection.Manager(userId);
            ManagerInfo managerInfo;
            if (Cache.Exists(key))
            {
                managerInfo = Cache.Get<ManagerInfo>(key);
            }
            else
            {
                managerInfo = this.context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.Id == userId && item.ShopId == 0L);
                if (managerInfo == null)
                {
                    return null;
                }
                if (managerInfo.RoleId == 0L)
                {
                    List<AdminPrivilege> list = new List<AdminPrivilege>();
                    list.Add((AdminPrivilege)0);
                    managerInfo.RoleName = "系统管理员";
                    managerInfo.AdminPrivileges = list;
                    managerInfo.Description = "系统管理员";
                }
                else
                {
                    RoleInfo roleInfo = this.context.RoleInfo.FindById(managerInfo.RoleId);
                    if (roleInfo != null)
                    {
                        List<AdminPrivilege> AdminPrivileges = new List<AdminPrivilege>();
                        //roleInfo.RolePrivilegeInfo.ToList<RolePrivilegeInfo>().ForEach(delegate (RolePrivilegeInfo a)
                        //{
                        //    AdminPrivileges.Add((AdminPrivilege)a.Privilege);
                        //});
                        managerInfo.RoleName = roleInfo.RoleName;
                        managerInfo.AdminPrivileges = AdminPrivileges;
                        managerInfo.Description = roleInfo.Description;
                    }
                }
                Cache.Insert<ManagerInfo>(key, managerInfo);
            }
            return managerInfo;
        }

        public ManagerInfo GetManager(long userId, bool isPlatFormManager = true)
        {

            return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == userId).FirstOrDefault();
            
        }


        public ManagerInfo Login(string username, string password)
        {
            ManagerInfo managerInfo = null;

        
            managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName == username ).FirstOrDefault<ManagerInfo>();
        
            if (managerInfo != null)
            {
                string passwrodWithTwiceEncode = this.GetPasswrodWithTwiceEncode(password, managerInfo.PasswordSalt);
                if (passwrodWithTwiceEncode.ToLower() != managerInfo.Password)
                {
                    managerInfo = null;
                    return managerInfo;
                }
                this.context.SaveChanges();
            }

            return managerInfo;
        }



        public void Logout(string username)
        {
            ManagerInfo man = GetPlatformManagerByName(username);
            if (man != null)
            {
                man.IsLogin = 0;
                this.context.SaveChanges();

                string key = CacheKeyCollection.Manager(man.Id);
                if (Cache.Exists(key))
                {
                    Cache.Get<ManagerInfo>(key).IsLogin = 0;
                }
            }
        }

        public void ChangePlatformManagerPassword(long id, string password, long roleId)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id).FirstOrDefault<ManagerInfo>();
            if (managerInfo == null)
            {
                throw new BaoZhongException("该管理员不存在，或者已被删除!");
            }
            if (roleId != 0L && managerInfo.RoleId != 0L)
            {
                managerInfo.RoleId = roleId;
            }
            if (!string.IsNullOrWhiteSpace(password))
            {
                string str = SecureHelper.MD5(password);
                managerInfo.Password = SecureHelper.MD5(str + managerInfo.PasswordSalt);
            }

            this.context.SaveChanges();
            string key = CacheKeyCollection.Manager(id);
            Cache.Remove(key);
        }

        public bool ChangePassword(string username, string password)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName.Equals(username)).FirstOrDefault();
            if (managerInfo == null)
            {
                return false;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(password))
                {
                    string str = SecureHelper.MD5(password);
                    managerInfo.Password = SecureHelper.MD5(str + managerInfo.PasswordSalt);
                }
                this.context.SaveChanges();
                return true;
            }
        }


        public long AddPlatformManager(ManagerInfo model)
        {
            if (model.RoleId == 0L)
            {
                throw new BaoZhongException("权限组选择不正确!");
            }
            if (this.CheckUserNameExist(model.UserName, true))
            {
                throw new BaoZhongException("该用户名已存在！");
            }
            model.ShopId = 0L;
            model.PasswordSalt = Guid.NewGuid().ToString();
            model.CreateDate = DateTime.Now;
            string str = SecureHelper.MD5(model.Password);
            model.Password = SecureHelper.MD5(str + model.PasswordSalt);

            this.context.ManagerInfo.Add(model);
            this.context.SaveChanges();

            return model.Id;
        }

        public void DeletePlatformManager(long id)
        {
            ManagerInfo entity = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0L && item.RoleId != 0L).FirstOrDefault<ManagerInfo>();
            if (entity == null)
            {
                return;
            }
            this.context.ManagerInfo.Remove(entity);
            this.context.SaveChanges();
            string key = CacheKeyCollection.Manager(id);
            Cache.Remove(key);
        }

        public void BatchDeletePlatformManager(long[] ids)
        {
            IQueryable<ManagerInfo> entities = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == 0L && item.RoleId != 0L && ids.Contains(item.Id));
            this.context.ManagerInfo.RemoveRange(entities);
            this.context.SaveChanges();
            long[] ids2 = ids;
            for (int i = 0; i < ids2.Length; i++)
            {
                long managerId = ids2[i];
                string key = CacheKeyCollection.Manager(managerId);
                Cache.Remove(key);
            }
        }

        public IQueryable<ManagerInfo> GetPlatformManagerByRoleId(long roleId)
        {
            return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.RoleId == roleId);
        }

        public ManagerInfo GetPlatformManagerByName(string name)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.UserName == name);
            return managerInfo;
        }


        public ManagerInfo GetManagerInfoByName(string name)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.UserName == name);
            return managerInfo;
        }

        public IQueryable<ManagerInfo> GetManagers(string keyWords)
        {
            return this.context.ManagerInfo.FindBy((ManagerInfo item) => keyWords == null || keyWords == "" || item.UserName.Contains(keyWords));
        }

        private string GetPasswrodWithTwiceEncode(string password, string salt)
        {
            string str = SecureHelper.MD5(password);
            return SecureHelper.MD5(str + salt);
        }

        public bool CheckUserNameExist(string username, bool isPlatFormManager = false)
        {
            return this.context.ManagerInfo.Any((ManagerInfo item) => item.UserName.ToLower() == username.ToLower());
        }


        public long GetManagersId(string UserName)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName == UserName).FirstOrDefault<ManagerInfo>();
            if (managerInfo == null)
            {
                return 0;
            }
            else
            {
                return managerInfo.Id;
            }
        }

    }
}
