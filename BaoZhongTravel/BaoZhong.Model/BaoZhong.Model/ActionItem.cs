using System.Collections.Generic;

namespace BaoZhong.Model
{
    public class ActionItem
    {
        public string Name
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public List<Controllers> Controllers
        {
            get;
            set;
        }

        public int PrivilegeId
        {
            get;
            set;
        }

        public ActionItem()
        {
            this.Controllers = new List<Controllers>();
        }
    }
}
