using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace CFT.CSOMS.BLL.SysManageModule
{
    using CFT.CSOMS.DAL.SysManageModule;
    public class SysManageService
    {
        //查询公告联系人所有分组
        public DataSet QueryAllContactsGroup(int limit, int offset)
        {
            return new SysManageData().QueryAllContactsGroup(limit, offset);
        }

        //添加公告联系人分组
        public void AddContactsGroup(string groupName, string createuser)
        {
            new SysManageData().AddContactsGroup(groupName, createuser);
        }

        //删除公告联系人分组
        public void DelContactsGroup(string id, string updateuser)
        {
            new SysManageData().DelContactsGroup(id, updateuser);
        }

        //查询某组公告联系人
        public DataSet QueryOneGroupContacts(string groupId, int limit, int offset)
        {
            return new SysManageData().QueryOneGroupContacts(groupId, limit, offset);
        }

        //添加公告联系人
        public void AddContacts(string groupId, string name, string email, string createuser)
        {
            new SysManageData().AddContacts(groupId, name, email, createuser);
        }

        //删除公告联系人
        public void DelContacts(string id, string updateuser)
        {
            new SysManageData().DelContacts(id, updateuser);
        }
    }
}
