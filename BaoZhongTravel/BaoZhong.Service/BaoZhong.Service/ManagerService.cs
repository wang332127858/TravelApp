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

        public void UpdatePlatformManagerRegion(long ManId, string Region)
        {
            ManagerInfo manager = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == ManId).FirstOrDefault();
            if (manager != null)
            {
                if (string.IsNullOrEmpty(manager.Remark))
                {
                    manager.Remark = Region;
                }
                else
                {
                    manager.Remark += "|";
                    manager.Remark += Region;
                }
                this.context.SaveChanges();
            }
        }

        public PageModel<ManagerInfo> GetGroupMembers(ManagerQuery query)
        {
            IQueryable<ManagerInfo> queryable = this.context.ManagerInfo.AsQueryable<ManagerInfo>();
            queryable = from item in queryable
                        where item.ShopId == 0L
                        select item;
            if (!string.IsNullOrWhiteSpace(query.keyWords))
            {
                queryable = from item in queryable
                            where item.UserName.Contains(query.keyWords)
                            select item;
            }

            if (!string.IsNullOrWhiteSpace(query.ParentName))
            {
                ManagerInfo m = GetPlatformManagerByName(query.ParentName);
                if (m != null)
                {
                    queryable = from item in queryable
                                where item.ParentId == m.Id
                                select item;
                }
            }

            if (query.GroupId > 0L)
            {
                queryable = from item in queryable
                            where item.GroupId == query.GroupId.Value
                            select item;
            }
            IQueryable<GroupInfo> groups = null;
            if (query.IsMarket)
            {
                groups = this.context.GroupInfo.FindBy((GroupInfo item) => item.Description == -1 || item.Description > 0);
            }
            else
            {
                groups = this.context.GroupInfo.FindBy((GroupInfo item) => item.Description == 0);
            }
            queryable = from item in queryable join item1 in groups on item.GroupId equals item1.Id select item;
            int total = queryable.Count<ManagerInfo>();
            queryable = queryable.GetPage(out total, query.PageNo, query.PageSize);
            return new PageModel<ManagerInfo>
            {
                Models = queryable,
                Total = total
            };
        }

        public PageModel<ManagerInfo> GetBusinessManagers(ManagerQuery query)
        {
            int total = 0;
            IQueryable<ManagerInfo> disabled = this.context.ManagerInfo.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.keyWords))
            {
                disabled = from a in disabled where a.UserName.Equals(query.keyWords) select a;
            }

            IQueryable<ManagerInfo> models = disabled.FindBy((ManagerInfo item) => item.ShopId > 0L, query.PageNo, query.PageSize, out total, (ManagerInfo item) => item.RoleId, true);
            return new PageModel<ManagerInfo>
            {
                Models = models,
                Total = total
            };
        }

        public IQueryable<ManagerInfo> GetMarketMembers()
        {
            GroupInfo group = this.context.GroupInfo.FindBy((GroupInfo item) => item.GroupName == "市场专员").FirstOrDefault();
            IQueryable<ManagerInfo> disable1 = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.GroupId == group.Id);
            return disable1;
        }

        public IQueryable<ManagerInfo> GetAgents()
        {
            GroupInfo group = this.context.GroupInfo.FindBy((GroupInfo item) => item.GroupName == "代理").FirstOrDefault();
            IQueryable<ManagerInfo> disable1 = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.GroupId == group.Id);
            return disable1;
        }

        public IQueryable<ManagerInfo> GetAgentsOrChannels(long parentId)
        {
            IQueryable<ManagerInfo> disable1 = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.ParentId == parentId);
            return disable1;
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
                        roleInfo.RolePrivilegeInfo.ToList<RolePrivilegeInfo>().ForEach(delegate (RolePrivilegeInfo a)
                        {
                            AdminPrivileges.Add((AdminPrivilege)a.Privilege);
                        });
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
            if (isPlatFormManager)
            {
                return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == userId && item.ShopId == 0L).FirstOrDefault();
            }
            else
            {
                return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == userId && item.ShopId > 0L).FirstOrDefault();
            }
        }

        public ManagerInfo GetManagerByShopId(long shopId)
        {
            return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == shopId && item.ShopId > 0L).FirstOrDefault();
            //if (isPlatFormManager)
            //{
            //    return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == userId && item.ShopId == 0L).FirstOrDefault();
            //}
            //else
            //{
            //    return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == userId && item.ShopId > 0L).FirstOrDefault();
            //}
        }

        public ManagerInfo Login(string username, string password, bool isPlatFormManager = false)
        {
            ManagerInfo managerInfo = null;

            if (isPlatFormManager)
            {
                managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName == username && item.ShopId == 0L).FirstOrDefault<ManagerInfo>();
            }
            else
            {
                managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName == username && item.ShopId != 0L).FirstOrDefault<ManagerInfo>();
            }

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
        public bool SetClientId(string username, string ClientId)
        {
            ManagerInfo managerInfo = null;
            managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName == username && item.ShopId != 0L).FirstOrDefault<ManagerInfo>();
            if (managerInfo == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(ClientId))
            {
                managerInfo.ParentName = ClientId;
            }
            else
            {
                return false;
            }

            base.context.SaveChanges();

            return true;
        }
        public ManagerInfo LoginForMarket(string username, string password)
        {
            ManagerInfo managerInfo = null;
            //todo:此处还应判断其group是否为“xxx”,但目前暂时省略
            managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.UserName == username && item.ShopId == 0L).FirstOrDefault<ManagerInfo>();
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
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0L).FirstOrDefault<ManagerInfo>();
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

        public void ChangePlatformManagerPassword(long id, string password, long roleId, string realname, string phone, string BankAccountName, string BankAccountNumber, string Address)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0L).FirstOrDefault<ManagerInfo>();
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
            if (!string.IsNullOrWhiteSpace(realname))
            {
                managerInfo.RealName = realname;
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                managerInfo.Phone = phone;
            }
            if (!string.IsNullOrWhiteSpace(BankAccountName))
            {
                managerInfo.BankAccountName = BankAccountName;
            }
            if (!string.IsNullOrWhiteSpace(BankAccountNumber))
            {
                managerInfo.BankAccountNumber = BankAccountNumber;
            }
            if (!string.IsNullOrWhiteSpace(Address))
            {
                managerInfo.Address = Address;
            }
            this.context.SaveChanges();
            string key = CacheKeyCollection.Manager(id);
            Cache.Remove(key);
        }

        public void ChangeShopManagerPassword(long id, string password)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId > 0L).FirstOrDefault<ManagerInfo>();
            if (managerInfo == null)
            {
                throw new BaoZhongException("该商家不存在，或者已被删除!");
            }
            if (!string.IsNullOrWhiteSpace(password))
            {
                string str = SecureHelper.MD5(password);
                managerInfo.Password = SecureHelper.MD5(str + managerInfo.PasswordSalt);
            }
            this.context.SaveChanges();
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

        public long AddShopManager(ManagerInfo model)
        {
            if (model.RoleId == 0L)
            {
                throw new BaoZhongException("权限组选择不正确!");
            }
            if (this.CheckUserNameExist(model.UserName, true))
            {
                throw new BaoZhongException("该用户名已存在！");
            }
            model.CreateDate = DateTime.Now;
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
            return this.context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == 0L && item.RoleId == roleId);
        }
        public ManagerInfo GetChannelManager(long id)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.ShopId == 0L && item.Id == id);
            GroupInfo groupInfo = Instance<IGroupService>.Create.GetGroup(managerInfo.GroupId);
            if (groupInfo == null)
            {
                return null;
            }
            if (string.Compare(groupInfo.GroupName, "渠道") != 0)
            {
                return null;
            }
            return managerInfo;
        }
        public ManagerInfo GetPlatformManagerByName(string name)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.ShopId == 0L && item.UserName == name);
            return managerInfo;
        }

        public ManagerInfo GetBusinessByName(string name)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.ShopId > 0L && item.UserName == name);
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

        public void UpdateManagerGroup(long id, long groupId, long parentId)
        {
            ManagerInfo managerInfo = this.context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0L).FirstOrDefault<ManagerInfo>();
            if (managerInfo == null)
            {
                throw new BaoZhongException("该管理员不存在，或者已被删除!");
            }
            managerInfo.GroupId = groupId;
            managerInfo.ParentId = parentId;
            this.context.SaveChanges();
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

        public int UpdateMarketMember(long id, long parentId, long remark)
        {
            ManagerInfo man = GetManager(id);

            man.ParentId = parentId;
            man.Remark = remark.ToString();

            this.context.SaveChanges();
            return 0;
        }
    }
}
